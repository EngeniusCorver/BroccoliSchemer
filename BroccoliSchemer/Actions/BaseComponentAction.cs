using BroccoliSchemer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

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
                    foreach (var itemPath in Directory.GetFiles(BaseComponent.BASE_PATH + itemType))
                    {
                        using (Stream stream = File.OpenRead(itemPath)) {
                            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                            Components.Add(new BrcButton(Path.GetFileName(itemPath), decoder.Frames[0].PixelHeight, decoder.Frames[0].PixelWidth));
                        }
                    }
                }
                else if (itemType.Contains("BrcContainer"))
                {
                    foreach (var itemPath in Directory.GetFiles(BaseComponent.BASE_PATH + itemType)) {
                        using (Stream stream = File.OpenRead(itemPath)) {
                            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                            Components.Add(new BrcContainer(Path.GetFileName(itemPath), decoder.Frames[0].PixelHeight, decoder.Frames[0].PixelWidth));
                        }
                    }
                }
                else if (itemType.Contains("BrcGrid"))
                {
                    foreach (var itemPath in Directory.GetFiles(BaseComponent.BASE_PATH + itemType)) {
                        using (Stream stream = File.OpenRead(itemPath)) {
                            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                            Components.Add(new BrcGrid(Path.GetFileName(itemPath), decoder.Frames[0].PixelHeight, decoder.Frames[0].PixelWidth));
                        }
                    }
                }
                else if (itemType.Contains("BrcSlider"))
                {
                    foreach (var itemPath in Directory.GetFiles(BaseComponent.BASE_PATH + itemType)) {
                        using (Stream stream = File.OpenRead(itemPath)) {
                            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                            Components.Add(new BrcSlider(Path.GetFileName(itemPath), decoder.Frames[0].PixelHeight, decoder.Frames[0].PixelWidth));
                        }
                    }
                }
                else if (itemType.Contains("BrcTextBox"))
                {
                    foreach (var itemPath in Directory.GetFiles(BaseComponent.BASE_PATH + itemType)) {
                        using (Stream stream = File.OpenRead(itemPath)) {
                            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                            Components.Add(new BrcTextBox(Path.GetFileName(itemPath), decoder.Frames[0].PixelHeight, decoder.Frames[0].PixelWidth));
                        }
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
