using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace BroccoliSchemer.Entities
{
    public class BaseComponent : IListable
    {
        public const string BASE_PATH = "../../Resources/ComponentImages/";
        public BaseComponent(string name, int height, int width)
        {
            Name = Regex.Replace(Path.GetFileNameWithoutExtension(name), "[^a-zA-z]", "");
            ImagePath = BASE_PATH + GetType().Name + "/" + name;
            ScreenHeight = height;
            ScreenWidth = width;
        }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public double Width { get; set; } = 51.2;
        public double Height { get; set; } = 51.2;
        public Color BorderColor { get; set; }
        public double BorderThickness { get; set; } = 1;
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }
        public string Text { get; set; } = "";
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
    }
}
