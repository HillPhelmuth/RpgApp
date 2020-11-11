using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurnBasedRpg.Types.Enums
{
    public enum ClassType
    {
        None,
        Warrior,
        Ranger,
        Mage
    }

    public enum EffectType
    {
        None,
        Attack,
        Defend,
        Modify,
        ModifySelf,
        Heal,
        Status
    }

    public enum Rarity
    {
        VeryCommon,
        Common,
        Moderate,
        Rare,
        VeryRare
    }
 
}
