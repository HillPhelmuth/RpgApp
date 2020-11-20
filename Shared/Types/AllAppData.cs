using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgApp.Shared.Types
{
    public class AllAppData
    {
        public List<Monster> Monsters { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Equipment> Equipment { get; set; }
    }

}
