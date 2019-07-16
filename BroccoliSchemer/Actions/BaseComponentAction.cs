using BroccoliSchemer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BroccoliSchemer.Actions
{
    public class BaseComponentAction
    {
        private const string BASE_PATH = "../../Resources/ComponentImages/";
        public static List<IListable> GetComponents()
        {
            //Read FileNames
            string[] componentNames = Directory.GetFiles(BASE_PATH).Select(Path.GetFileNameWithoutExtension).ToArray();
            List<IListable> Components = new List<IListable>();
            foreach (var item in componentNames)
            {
                switch (Regex.Replace(item, "[^a-zA-Z]", ""))
                {
                    case "GraphicsCard":
                        Components.Add(new GraphicsCard(item, BASE_PATH));
                        break;
                    case "Headphone":
                        Components.Add(new Headphone(item, BASE_PATH));
                        break;
                    case "Keyboard":
                        Components.Add(new Keyboard(item, BASE_PATH));
                        break;
                    case "Mainboard":
                        Components.Add(new Mainboard(item, BASE_PATH));
                        break;
                    case "Monitor":
                        Components.Add(new Monitor(item, BASE_PATH));
                        break;
                    default:
                        throw new Exception("Item Class Couldn't be found. Check item name or class existence!");
                }
            }
            return Components;
        }
    }
}
