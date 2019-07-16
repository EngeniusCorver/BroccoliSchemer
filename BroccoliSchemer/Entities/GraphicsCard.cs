using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class GraphicsCard : BaseComponent
    {
        public GraphicsCard(string name, string basePath) : base(name, basePath)
        {
        }

        public string Ram { get; set; }
        public float ClockSpeed { get; set; }
    }
}
