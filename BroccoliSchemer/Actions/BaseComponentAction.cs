using BroccoliSchemer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BroccoliSchemer.Actions
{
    public class BaseComponentAction
    {
        public static List<IListable> GetComponents()
        {
            //Read FileNames
            string[] componentNames = Directory.GetDirectories(BaseComponent.BASE_PATH);
            List<IListable> Components = new List<IListable>();
            foreach (var itemType in componentNames)
            {
                if (itemType.Contains("BrcButton"))
                {
                    foreach (var itemName in Directory.GetFiles(BaseComponent.BASE_PATH + itemType).Select(Path.GetFileNameWithoutExtension).ToArray())
                    {
                        Components.Add(new BrcButton(itemName));
                    }
                }
                else if (itemType.Contains("BrcContainer"))
                {
                    foreach (var itemName in Directory.GetFiles(BaseComponent.BASE_PATH + itemType).Select(Path.GetFileNameWithoutExtension).ToArray())
                    {
                        Components.Add(new BrcContainer(itemName));
                    }
                }
                else if (itemType.Contains("BrcGrid"))
                {
                    foreach (var itemName in Directory.GetFiles(BaseComponent.BASE_PATH + itemType).Select(Path.GetFileNameWithoutExtension).ToArray())
                    {
                        Components.Add(new BrcGrid(itemName));
                    }
                }
                else if (itemType.Contains("BrcSlider"))
                {
                    foreach (var itemName in Directory.GetFiles(BaseComponent.BASE_PATH + itemType).Select(Path.GetFileNameWithoutExtension).ToArray())
                    {
                        Components.Add(new BrcSlider(itemName));
                    }
                }
                else if (itemType.Contains("BrcTextBox"))
                {
                    foreach (var itemName in Directory.GetFiles(BaseComponent.BASE_PATH + itemType).Select(Path.GetFileNameWithoutExtension).ToArray())
                    {
                        Components.Add(new BrcTextBox(itemName));
                    }
                }
                else
                {
                    MessageBox.Show("Item Class Couldn't be found. Check item name or class existence!");
                }
            }
            return Components;
        }
    }
}
