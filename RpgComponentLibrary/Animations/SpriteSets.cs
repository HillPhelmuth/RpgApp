using System.Collections.Generic;
using System.Linq;

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
        private static Dictionary<string, SpriteDataModel> _warriorSprites;
        private static Dictionary<string, SpriteDataModel> _wizardSprites;
        private static Dictionary<string, SpriteDataModel> _archerSprites;
        public static Dictionary<string, SpriteDataModel> OverheadSprites => GetOverheadSprites();
        public static Dictionary<string, SpriteDataModel> WarriorSprites => GetWarriorSprites();
        public static Dictionary<string, SpriteDataModel> WizardSprites => GetWizardSprites();
        public static Dictionary<string, SpriteDataModel> ArcherSprites => GetArcherSprites();
        public static Dictionary<string, SpriteDataModel> PinkSprites => GetPinkTopviewSprites();
        public static Dictionary<string, SpriteDataModel> BoyleSprites => GetBoyleSprites();
        public static Dictionary<string, SpriteDataModel> KingSprites => GetKingSprites();
        public static Dictionary<string, SpriteDataModel> CheerSprites => GetCheerLeaderSprites();
        public static Dictionary<string, SpriteDataModel> CopSprites => GetCopSprites();
        public static Dictionary<string, SpriteDataModel> SkeletonSprites => GetSkeletonSprites();

        public static List<SpriteDataModel> AllSpriteAssets()
        {
            var sprites = new List<SpriteDataModel>();
            sprites.AddRange(OverheadSprites.Values);
            sprites.AddRange(WarriorSprites.Values);
            sprites.AddRange(WizardSprites.Values);
            sprites.AddRange(ArcherSprites.Values);
            sprites.AddRange(PinkSprites.Values);
            sprites.AddRange(BoyleSprites.Values);
            sprites.AddRange(KingSprites.Values);
            sprites.AddRange(CheerSprites.Values);
            sprites.AddRange(CopSprites.Values);
            return sprites;

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
        private static Dictionary<string, SpriteDataModel> GetArcherSprites()
        {
            _archerSprites ??= new Dictionary<string, SpriteDataModel>
            {
                ["Attack1"] = AnimationHelper.GetSpriteData("ArcAttack1"),
                ["Attack2"] = AnimationHelper.GetSpriteData("ArcAttack2"),
                ["Dead"] = AnimationHelper.GetSpriteData("ArcDead"),
                ["Idle"] = AnimationHelper.GetSpriteData("ArcIdle"),
                ["Hit"] = AnimationHelper.GetSpriteData("ArcHit")
            };
            return _archerSprites;
        }
        private static Dictionary<string, SpriteDataModel> _skeletons;
        
        private static Dictionary<string, SpriteDataModel> GetSkeletonSprites()
        {
            _skeletons ??= new Dictionary<string, SpriteDataModel>
            {
                ["Attack"] = AnimationHelper.GetSpriteData("SkeletonAttack"),
                ["Dead"] = AnimationHelper.GetSpriteData("SkeletonDead"),
                ["Idle"] = AnimationHelper.GetSpriteData("SkeletonIdle")
            };
            return _skeletons;
        }
    }

    public static class SheetExtentions
    {
        public static int Width(this SpriteDataModel sheet) => sheet.Frames.Sum(x => x.W);
        public static int Height(this SpriteDataModel sheet)
        {
            return sheet.Frames.Select(x => x.H).FirstOrDefault();
        }
    }
}
