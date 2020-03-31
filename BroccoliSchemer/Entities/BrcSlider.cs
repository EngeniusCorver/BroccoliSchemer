using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class BrcSlider : BaseComponent, IListable
    {
        public BrcSlider(string name, int height, int width) : base(name, height, width) 
        {

        }

        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
    }
}
