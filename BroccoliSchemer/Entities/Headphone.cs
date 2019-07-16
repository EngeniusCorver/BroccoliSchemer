using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class Headphone : BaseComponent
    {
        public Headphone(string name, string basePath) : base(name, basePath)
        {
        }

        public bool IsWireless { get; set; }
    }
}
