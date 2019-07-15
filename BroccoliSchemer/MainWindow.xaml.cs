using BroccoliSchemer.Entities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace BroccoliSchemer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Components.ItemsSource = GetComponents();
        }

        private List<Component> GetComponents()
        {
            return new List<Component>() {
                new Component()
                {
                    Name = "Graphics Card",
                    ImagePath = @"\Resources\ComponentImages\GraphicsCard.png"
                    //ImagePath = @"..\BroccoliSchemer.Resources\Resources\ComponentImages\GraphicsCard.png"
                },
                new Component()
                {
                    Name = "Headphones",
                    ImagePath = @"\Resources\ComponentImages\Headphones.png"
                    //ImagePath = @"..\BroccoliSchemer.Resources\Resources\ComponentImages\Headphones.png"
                },
                new Component()
                {
                    Name = "Keyboard",
                    ImagePath = @"\Resources\ComponentImages\Keyboard.png"
                    //ImagePath = @"..\BroccoliSchemer.Resources\Resources\ComponentImages\Keyboard.png"
                },
                new Component()
                {
                    Name = "Mainboard",
                    ImagePath = @"\Resources\ComponentImages\Mainboard.png"
                    //ImagePath = @"..\BroccoliSchemer.Resources\Resources\ComponentImages\Mainboard.png"
                },
                new Component()
                {
                    Name = "Mainboard 2",
                    ImagePath = @"\Resources\ComponentImages\Mainboard2.png"
                    //ImagePath = @"..\BroccoliSchemer.Resources\Resources\ComponentImages\Mainboard2.png"
                },
                new Component()
                {
                    Name = "Monitor",
                    ImagePath = @"\Resources\ComponentImages\Monitor.png"
                    //ImagePath = @"..\BroccoliSchemer.Resources\Resources\ComponentImages\Monitor.png"
                }
            };
        }

        private void CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinimizingButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ChangeStateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState.Equals(WindowState.Normal))
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
    }
}
