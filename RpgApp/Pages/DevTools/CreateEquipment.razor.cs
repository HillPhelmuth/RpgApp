using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TurnBasedRpg.CheatDevTools;
using TurnBasedRpg.Services;
using TurnBasedRpg.Types;
using TurnBasedRpg.Types.Enums;

namespace TurnBasedRpg.Pages.DevTools
{
    public partial class CreateEquipment
    {
        [Inject]
        public DnDApiService DnDApi { get; set; }
        [Inject]
        public RpgDataService RpgDataService { get; set; }
        private List<GeneralApiData> WeaponCategories { get; set; }
        private GeneralApiData WeaponCategory { get; set; }
        private List<GeneralApiData> ArmorCategories { get; set; }
        private GeneralApiData ArmorCategory { get; set; }
        //private EquipmentList CategoryItems { get; set; }
        private List<GeneralApiData> SelectedCategoryList { get; set; }
        private WeaponData WeaponData { get; set; } = new WeaponData();
        private ArmorData ArmorData { get; set; } = new ArmorData();
        private CreateItemForm CreateItemForm { get; set; } = new CreateItemForm();

        private string submitSucess = "";
        private string toggleLabel = "Weapons";
        private bool isWeapon = true;
        private bool isArmor => !isWeapon;

        protected override async Task OnInitializedAsync()
        {
            var allItems = await DnDApi.GetEquipmentGategories();
            WeaponCategory = allItems.FirstOrDefault(x => x.Name == "Weapon");
            var weaponItems = await DnDApi.GetSelectedList(WeaponCategory?.Url);
            WeaponCategories = weaponItems.EquipmentsData;
            ArmorCategory = allItems.FirstOrDefault(x => x.Name == "Armor");
            var armorItems = await DnDApi.GetSelectedList(ArmorCategory?.Url);
            ArmorCategories = armorItems.EquipmentsData;
            SelectedCategoryList = WeaponCategories;

        }
        
        //private async Task GetWeaponsList(string url)
        //{
        //    var categoryItems = await DnDApi.GetSelectedList(url);
        //    SelectedCategoryList = categoryItems.EquipmentsData;
        //    isWeapon = true;
        //    //isArmor = false;
        //    await InvokeAsync(StateHasChanged);
        //}
        //private async Task GetArmorList(string url)
        //{
        //    var categoryItems = await DnDApi.GetSelectedList(url);
        //    SelectedCategoryList = categoryItems.EquipmentsData;
        //    isWeapon = true;
        //    //isArmor = false;
        //    await InvokeAsync(StateHasChanged);
        //}

        private async void GetSelectedItem(object dataRow)
        {
            var selectedItem = (GeneralApiData)dataRow;
            if (isArmor)
                await GetSelectedArmor(selectedItem);
            if (isWeapon)
                await GetSelectedWeapon(selectedItem);
        }

        private void ToggleCategory(bool toggle)
        {
            isWeapon = !isWeapon;
            toggleLabel = isWeapon ? "Weapons" : "Armor";
            SelectedCategoryList = isWeapon ? WeaponCategories : ArmorCategories;
            StateHasChanged();
        }
        private async Task GetSelectedWeapon(GeneralApiData itemData)
        {
            WeaponData = await DnDApi.GetWeaponData(itemData.Url);
            
            var newItemForm = new CreateItemForm
            {
                Name = WeaponData.Name,
                Type = EffectType.Attack,
                Value = WeaponData.Damage.DamageDice,
                AllowedClassesData = "Warrior,Mage,Ranger",
                Rarity = Rarity.Common,
                Description = $"Category: {WeaponData.WeaponCategory} Range: {WeaponData.WeaponRange} Weight: {WeaponData.Weight}lbs"
            };
            CreateItemForm = newItemForm;
            await InvokeAsync(StateHasChanged);
        }
        private async Task GetSelectedArmor(GeneralApiData itemData)
        {
            ArmorData = await DnDApi.GetArmorData(itemData.Url);
            string allowedClasses = "";
            var newItemForm = new CreateItemForm
            {
                Name = ArmorData.Name,
                Type = EffectType.Defend,
                Value = $"{ArmorData.ArmorClass.Base - 9}",
                Rarity = Rarity.Common,
                Description = $"Category: {ArmorData.ArmorCategory} Stealth penalty: {ArmorData.StealthDisadvantage}"
            };
            allowedClasses = ArmorData.ArmorCategory switch
            {
                "Light" => "Warrior,Mage,Ranger",
                "Heavy" => "Warrior",
                _ => "Warrior,Ranger"
            };
            newItemForm.AllowedClassesData = allowedClasses;
            CreateItemForm = newItemForm;
            await InvokeAsync(StateHasChanged);
        }

        private async Task SubmitItemForm()
        {
            var newEquipment = new Equipment
            {
                Name = CreateItemForm.Name,
                Description = CreateItemForm.Description,
                AllowedClassesData = CreateItemForm.AllowedClassesData,
                GoldCost = CreateItemForm.GoldCost,
                Rarity = CreateItemForm.Rarity,
                EquipLocation = CreateItemForm.EquipLocation,
                Effects = new List<Effect> {new Effect { Type  = CreateItemForm.Type, Value = CreateItemForm.Value}}
            };
            var isSuccess = await RpgDataService.AddNewEquipment(newEquipment);
            submitSucess = isSuccess ? "Item successfully added to Db" : "Nope! an Item with this Name already exists in the Database";
        }

        private void ClearForm()
        {
            CreateItemForm = new CreateItemForm();
        }
    }
}
