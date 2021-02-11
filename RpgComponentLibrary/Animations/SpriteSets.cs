using System.Collections.Generic;

namespace RpgComponentLibrary.Animations
{
    public static class SpriteSets
    {

        private static Dictionary<string, SpriteDataModel> _allSpriteSets;
        private static Dictionary<string, SpriteDataModel> _pinkTopviewSets;
        private static Dictionary<string, SpriteDataModel> _boyleSets;
        private static Dictionary<string, SpriteDataModel> _burgerKingSets;
        private static Dictionary<string, SpriteDataModel> _cheerleaderSets;
        private static Dictionary<string, SpriteDataModel> _copSets;
        private static Dictionary<string, SpriteDataModel> _combatSprites;
        private static Dictionary<string, SpriteDataModel> _warriorSprites;
        private static Dictionary<string, SpriteDataModel> _wizardSprites;
        public static Dictionary<string, SpriteDataModel> OverheadSprites => GetOverheadSprites();
        public static Dictionary<string, SpriteDataModel> CombatSprites => GetCombatSprites();
        public static Dictionary<string, SpriteDataModel> WarriorSprites => GetWarriorSprites();
        public static Dictionary<string, SpriteDataModel> WizardSprites => GetWizardSprites();
        public static Dictionary<string, SpriteDataModel> PinkSprites => GetPinkTopviewSprites();
        public static Dictionary<string, SpriteDataModel> BoyleSprites => GetBoyleSprites();
        public static Dictionary<string, SpriteDataModel> KingSprites => GetKingSprites();
        public static Dictionary<string, SpriteDataModel> CheerSprites => GetCheerLeaderSprites();
        public static Dictionary<string, SpriteDataModel> CopSprites => GetCopSprites();

        public static Dictionary<string, SpriteDataModel> GetOverhead(string color)
        {
            return color == "Pink" ? PinkSprites : OverheadSprites;
        }
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
        private static Dictionary<string, SpriteDataModel> GetPinkTopviewSprites()
        {
            _pinkTopviewSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationHelper.GetSpriteData("PinkUp"),
                ["Down"] = AnimationHelper.GetSpriteData("PinkDown"),
                ["Left"] = AnimationHelper.GetSpriteData("PinkLeft"),
                ["Right"] = AnimationHelper.GetSpriteData("PinkRight")
            };
            return _pinkTopviewSets;
        }
        private static Dictionary<string, SpriteDataModel> GetBoyleSprites()
        {
            _boyleSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationHelper.GetSpriteData("BoyleUp"),
                ["Down"] = AnimationHelper.GetSpriteData("BoyleDown"),
                ["Left"] = AnimationHelper.GetSpriteData("BoyleLeft"),
                ["Right"] = AnimationHelper.GetSpriteData("BoyleRight")
            };
            return _boyleSets;
        }
        private static Dictionary<string, SpriteDataModel> GetKingSprites()
        {
            _burgerKingSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationHelper.GetSpriteData("BKUp"),
                ["Down"] = AnimationHelper.GetSpriteData("BKDown"),
                ["Left"] = AnimationHelper.GetSpriteData("BKLeft"),
                ["Right"] = AnimationHelper.GetSpriteData("BKRight")
            };
            return _burgerKingSets;
        }
        private static Dictionary<string, SpriteDataModel> GetCheerLeaderSprites()
        {
            _cheerleaderSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationHelper.GetSpriteData("CheerUp"),
                ["Down"] = AnimationHelper.GetSpriteData("CheerDown"),
                ["Left"] = AnimationHelper.GetSpriteData("CheerLeft"),
                ["Right"] = AnimationHelper.GetSpriteData("CheerRight")
            };
            return _cheerleaderSets;
        }
        private static Dictionary<string, SpriteDataModel> GetCopSprites()
        {
            _copSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationHelper.GetSpriteData("CopUp"),
                ["Down"] = AnimationHelper.GetSpriteData("CopDown"),
                ["Left"] = AnimationHelper.GetSpriteData("CopLeft"),
                ["Right"] = AnimationHelper.GetSpriteData("CopRight")
            };
            return _copSets;
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
                ["Dead"] = AnimationHelper.GetSpriteData("WarDead"),
                ["Hit"] = AnimationHelper.GetSpriteData("WarHit")
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
                ["Hit"] = AnimationHelper.GetSpriteData("WizardHit")
            };
            return _wizardSprites;
        }
    }
}
