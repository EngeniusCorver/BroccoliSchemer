using BroccoliSchemer.Actions;
using BroccoliSchemer.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            Components.ItemsSource = BaseComponentAction.GetComponents();
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

        private void SliderLeft_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                ComponentScroller.ScrollToHorizontalOffset(ComponentScroller.HorizontalOffset - 90);
            }
            else if (WindowState == WindowState.Maximized)
            {
                ComponentScroller.ScrollToHorizontalOffset(ComponentScroller.HorizontalOffset - 130);
            }
        }

        private void SliderRight_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                ComponentScroller.ScrollToHorizontalOffset(ComponentScroller.HorizontalOffset + 90);
            }
            else if (WindowState == WindowState.Maximized)
            {
                ComponentScroller.ScrollToHorizontalOffset(ComponentScroller.HorizontalOffset + 130);
            }
        }

        private void CanvasBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Components.SelectedIndex > -1)
            {
                BaseComponent selectedComponent = (BaseComponent)Components.SelectedItem;
                Image imageComponent = new Image();
                //TODO:Size values must be controlled from the right pane. These are just for testing
                imageComponent.Width = 50;
                imageComponent.Height = 50;
                Label labelComponent = new Label();
                imageComponent.Source = new BitmapImage(new System.Uri(selectedComponent.ImagePath, UriKind.Relative));
                labelComponent.Content = selectedComponent.Name;
                SchemerCanvas.Children.Add(imageComponent);
                Canvas.SetLeft(imageComponent, Mouse.GetPosition(SchemerCanvas).X);
                Canvas.SetTop(imageComponent, Mouse.GetPosition(SchemerCanvas).Y);
                SchemerCanvas.Children.Add(labelComponent);
                Canvas.SetLeft(labelComponent, Mouse.GetPosition(SchemerCanvas).X + imageComponent.Width / 2);
                Canvas.SetTop(labelComponent, Mouse.GetPosition(SchemerCanvas).Y + imageComponent.Height);
            }
        }
    }
}
