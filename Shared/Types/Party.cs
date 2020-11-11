using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurnBasedRpg.Types.Enums;

namespace TurnBasedRpg.Types
{
    public class Party
    {
        List<Player> Members = new List<Player>();
        public string PartyName { get; set; }
        public void AddPartyMember(string name, ClassType classType)
        {
            Members.Add(new Player { Name = name, ClassType = classType });
        }
    }
}
