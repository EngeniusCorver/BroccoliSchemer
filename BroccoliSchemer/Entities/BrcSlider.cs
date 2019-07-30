using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class BrcSlider : BaseComponent, IListable
    {
        public BrcSlider(string name) : base(name)
        {
        }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }
    }
}
