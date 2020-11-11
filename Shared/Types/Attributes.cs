using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurnBasedRpg.Types
{
    public class Attributes
    {
        public int ID { get; set; }
        public int PlayerID { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Toughness { get; set; }
        public int Speed { get; set; }
    }
}
