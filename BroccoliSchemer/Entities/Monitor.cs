using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroccoliSchemer.Entities
{
    public class Monitor : BaseComponent
    {
        public Monitor(string name, string basePath) : base(name, basePath)
        {
        }

        public int Size { get; set; }
        public ScreenResolution Resolution { get; set; }
    }
    public enum ScreenResolution
    {
        SD = 480,
        HD = 720,
        FHD = 1080,
        UHD = 2160
    }
}
