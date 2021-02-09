using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;
using RpgApp.Shared.Types.PlayerExtensions;
using RpgComponentLibrary.Animations;

namespace RpgApp.Client.Pages.StyleTests
{
    public partial class CompLibraryTester
    {
        [Inject]
        private AppStateManager AppState { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        private double life;
        private double mana;
        private double stamina;
        private bool showProgress = true;
        private bool showMenu = true;
        private List<string> TestLog = new();
        protected override async Task OnInitializedAsync()
        {
            _imagesEquipPairs = AddImages(AppState.AllEquipment);
            _imagesSkillPairs = AddImages(AppState.AllSkills);
            classType = ClassType.Warrior;
            SetCombatAnimationData();
            Player = await CreateCharacter.CreateNewCharacter(classType);
            await base.OnInitializedAsync();
        }
        // This is the event handler for the a variety of actions
        private void AddToLog(string info)
        {
            TestLog.Add(info);
            StateHasChanged();
        }
        #region RpgItemsMenu.razor

        private List<KeyValuePair<string, Equipment>> _imagesEquipPairs = new();
        private List<KeyValuePair<string, Equipment>> AddImages(List<Equipment> equipment)
        {
            return equipment.Select(eq => eq.AddImagePath()).ToList();
        }
        private List<KeyValuePair<string, Skill>> _imagesSkillPairs = new();
        private List<KeyValuePair<string, Skill>> AddImages(List<Skill> skills)
        {
            return skills.Select(sk => sk.AddImagePath()).ToList();
        }

        #endregion
        #region RpgProgressBar

        private async Task UpdateBar(string meterName)
        {
            var random = new Random();
            double newVal = random.Next(1, 101);
            switch (meterName)
            {
                case "Life":
                    life = newVal;
                    break;
                case "Mana":
                    mana = newVal;
                    break;
                case "Stamina":
                    stamina = newVal;
                    break;
            }

            await InvokeAsync(StateHasChanged);
        }
        #endregion
        #region RpgSelectDropdown.razor

        private void SelectedEquipmentHanlder(Equipment item)
        {
            AddToLog(item.AsDisplayString());
        }

        private void SelectedSkillHandler(Skill skill)
        {
            AddToLog(skill.AsDisplayString());
        }
        // private fields to bind to SelectedValue property
        private ClassType selectedClass;
        private string selectedMonster;
        private Skill selectedSkill = new();
        private Equipment selectedEquipment = new();
        private List<string> selectStrings => AppState.AllMonsters.Select(x => x.Name).ToList();
        private void SelectedStringHandler(string str)
        {
            AddToLog(str);
        }
        #endregion
        #region RpgSlider.razor

        private double slideLife;
        private double slideMana;
        private double lifeDisplayValue;
        private double manaDisplayValue;
        private async Task UpdateProgress(string meterName, double newVal)
        {
            switch (meterName)
            {
                case "Life":
                    slideLife = newVal;
                    lifeDisplayValue = newVal;
                    break;
                case "Mana":
                    slideMana = newVal;
                    manaDisplayValue = newVal;
                    break;
            }

            await InvokeAsync(StateHasChanged);
        }


        #endregion
        #region RpgTextInput.razor and RpgNumberInput.razor

        private string textInput;
        private string textAreaInput;
        private int intInput;
        private decimal decInput;
        private double doubleInput;

        #endregion
        #region RpgCheckbox.razor

        private bool isChecked1;
        private bool isChecked2;

        #endregion
        #region RpgCombatAnimation.razor

        private AnimationCombatActions combatActions = new();
        private AnimationModel combatAnimation;
        private ClassType classType;
        private CanvasBackgound canvasBackgound;
        private void SetCombatAnimationData()
        {
            combatAnimation = new AnimationModel
            {
                Sprites = classType == ClassType.Mage ? SpriteSets.WizardSprites : SpriteSets.WarriorSprites,
                Scale = 3
            };
            combatAnimation.CurrentSprite = combatAnimation.Sprites["Idle"];
        }
        private void RunAnimation(string animation)
        {
            combatActions.TriggerAnimation(animation);
        }

        private void ChangeAnimationCanvas(ClassType type, CanvasBackgound background)
        {
            combatAnimation.Sprites = type == ClassType.Mage ? SpriteSets.WizardSprites : SpriteSets.WarriorSprites;
            canvasBackgound = background;
            StateHasChanged();
        }
        #endregion
        #region RpgGlobalAnimation.razor

        private AnimationModel moveAnimation = new(SpriteSets.OverheadSprites, "Right", 3, 4);
        private List<CollisionBlock> collitionsBlocks = new()
        {
            new CollisionBlock("TargeCollide", "_content/RpgComponentLibrary/img/icons/foreign/x.png", 64, 64, 400, 300),
            new CollisionBlock("HitHome", "home-48.png", 48, 54, 725, 525)
        };
        //54x48
        private CanvasSpecs canvasSpecs = new(600, 800);
        private KeyValuePair<string, string> background = new("village1", "/css/Images/Village1.png");
        private bool hasCollided;
        private bool stopTimer;
        private void HandleMove((double x, double y) pos)
        {
            AddToLog($"moved to {pos.x}-{pos.y}");
            
        }

        private List<string> collisionList = new();
        private async void HandleCollision(string name)
        {
            if (collisionList.Contains(name)) return;
            collisionList.Add(name);
            var result = await ModalService.ShowMessageBoxAsync("Collision!", $"You collided with a {name} collision Block! Continue Animation?");
            // hasCollided = true;
            if (result == MessageBoxDialogResult.OK || result == MessageBoxDialogResult.None)
            {
                stopTimer = false;
                await InvokeAsync(StateHasChanged);
            }

        }
        #endregion

        #region RpgInfoBox.razor

        private Player Player { get;set; }

        private Dictionary<int, string> Attributes = new()
        {
            {1, "Name"},
            {2, "Level"},
            {3, "MaxAbilityPoints"},
            {4, "AbilityPoints"}
        };

        #endregion
    }
}
