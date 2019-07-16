using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class Mainboard : BaseComponent
    {
        public Mainboard(string name, string basePath) : base(name, basePath)
        {
        }

        public int RamSlotNumber { get; set; }
        public int RamCapacity { get; set; }
        public MainboardFormFactors FormFactor { get; set; }
    }
    public enum MainboardFormFactors
    {
        MicroATX,
        ITX,
        ATX
    }
}
