using System.Windows.Media;

namespace BroccoliSchemer.Entities
{
    public class BaseComponent : IListable
    {
        public BaseComponent(string name, string basePath)
        {
            Name = name;
            ImagePath = basePath + name + ".png";
        }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public Color BackgroundColor { get; set; }
        public Color Foreground { get; set; }
        public bool HasRgb { get; set; }
        public decimal Price { get; set; }
    }
}
