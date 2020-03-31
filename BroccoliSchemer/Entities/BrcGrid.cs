using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class BrcGrid : BaseComponent, IListable {
        public BrcGrid(string name, int height, int width) : base(name, height, width) 
        {

        }
    }
}
