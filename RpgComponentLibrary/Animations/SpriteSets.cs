using System.Collections.Generic;

namespace RpgComponentLibrary.Animations
{
    public static class SpriteSets
    {

        private static Dictionary<string, SpriteDataModel> _allSpriteSets;
        private static Dictionary<string, SpriteDataModel> _combatSprites;
        private static Dictionary<string, SpriteDataModel> _warriorSprites;
        private static Dictionary<string, SpriteDataModel> _wizardSprites;
        public static Dictionary<string, SpriteDataModel> OverheadSprites => GetOverheadSprites();
        public static Dictionary<string, SpriteDataModel> CombatSprites => GetCombatSprites();
        public static Dictionary<string, SpriteDataModel> WarriorSprites => GetWarriorSprites();
        public static Dictionary<string, SpriteDataModel> WizardSprites => GetWizardSprites();

        private static Dictionary<string, SpriteDataModel> GetOverheadSprites()
        {
            _allSpriteSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationHelper.GetSpriteData("OverheadUp"),
                ["Down"] = AnimationHelper.GetSpriteData("OverheadDown"),
                ["Left"] = AnimationHelper.GetSpriteData("OverheadLeft"),
                ["Right"] = AnimationHelper.GetSpriteData("OverheadRight")
            };
            return _allSpriteSets;
        }

        private static Dictionary<string, SpriteDataModel> GetCombatSprites()
        {
            _combatSprites ??= new Dictionary<string, SpriteDataModel>
            {
                ["WizardAttack1"] = AnimationHelper.GetSpriteData("WizardAttack1"),
                ["WizardAttack2"] = AnimationHelper.GetSpriteData("WizardAttack2"),
                ["WizardDead"] = AnimationHelper.GetSpriteData("WizardDead"),
                ["WizardIdle"] = AnimationHelper.GetSpriteData("WizardIdle"),
                ["WarAttack1"] = AnimationHelper.GetSpriteData("WarAttack1"),
                ["WarAttack2"] = AnimationHelper.GetSpriteData("WarAttack2"),
                ["WarIdle"] = AnimationHelper.GetSpriteData("WarIdle"),
                ["WarDead"] = AnimationHelper.GetSpriteData("WarDead")
                
            };
            return _combatSprites;
        }

        private static Dictionary<string, SpriteDataModel> GetWarriorSprites()
        {
            _warriorSprites ??= new Dictionary<string, SpriteDataModel>
            {
                ["Attack1"] = AnimationHelper.GetSpriteData("WarAttack1"),
                ["Attack2"] = AnimationHelper.GetSpriteData("WarAttack2"),
                ["Idle"] = AnimationHelper.GetSpriteData("WarIdle"),
                ["Dead"] = AnimationHelper.GetSpriteData("WarDead")
            };
            return _warriorSprites;
        }
        private static Dictionary<string, SpriteDataModel> GetWizardSprites()
        {
            _wizardSprites ??= new Dictionary<string, SpriteDataModel>
            {
                ["Attack1"] = AnimationHelper.GetSpriteData("WizardAttack1"),
                ["Attack2"] = AnimationHelper.GetSpriteData("WizardAttack2"),
                ["Dead"] = AnimationHelper.GetSpriteData("WizardDead"),
                ["Idle"] = AnimationHelper.GetSpriteData("WizardIdle"),
            };
            return _wizardSprites;
        }
    }
}
