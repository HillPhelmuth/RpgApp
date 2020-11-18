using System.Collections.Generic;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.Types
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
