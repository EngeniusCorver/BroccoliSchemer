using BroccoliSchemer.Actions;
using BroccoliSchemer.Entities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BroccoliSchemer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<IListable> ComponentsPlacedOnScreen = new List<IListable>();
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
                GetPropertyValuesForComponent((IListable)Components.SelectedItem);
                BaseComponent selectedComponent = (BaseComponent)Components.SelectedItem;
                Image imageComponent = new Image();
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

        private void GetPropertyValuesForComponent(IListable selectedItem)
        {
            foreach (var item in selectedItem.GetType().GetProperties())
            {
                Type pt = item.PropertyType;
                if (pt == typeof(bool))
                {
                    CheckBox checkBox = (CheckBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                    item.SetValue(selectedItem, checkBox.IsChecked);
                }
                else if (pt == typeof(int))
                {
                    TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                    item.SetValue(selectedItem, Convert.ToInt32(textBox.Text));
                }
                else if (pt == typeof(decimal))
                {
                    TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                    item.SetValue(selectedItem, Convert.ToDecimal(textBox.Text));
                }
                else if (pt == typeof(Color))
                {
                    TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                    item.SetValue(selectedItem, (Color)ColorConverter.ConvertFromString(textBox.Text));
                }
            }
            ComponentsPlacedOnScreen.Add(selectedItem);
        }

        private void Components_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateComponentPropertiesControllers();
        }

        private void CreateComponentPropertiesControllers()
        {
            PropertiesPanel.Children.Clear();
            foreach (var item in Components.SelectedItem.GetType().GetProperties())
            {
                Type pt = item.PropertyType;
                if (pt == typeof(bool))
                {
                    bool temp = (bool)item.GetValue(Components.SelectedItem);
                    CreatePropertyControllers(PropertiesPanel, item.Name, new CheckBox() { IsChecked = temp, Style = FindResource("PropertyCheckbox") as Style, Name = item.Name });
                }
                else if (pt == typeof(int))
                {
                    int temp = (int)item.GetValue(Components.SelectedItem);
                    TextBox tb = new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style };
                    tb.SetValue(FrameworkElement.NameProperty, item.Name);
                    CreatePropertyControllers(PropertiesPanel, item.Name, tb);
                }
                else if (pt == typeof(decimal))
                {
                    decimal temp = (decimal)item.GetValue(Components.SelectedItem);
                    CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                }
                else if (pt == typeof(Color))
                {
                    Color temp = (Color)item.GetValue(Components.SelectedItem);
                    CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                }
            }
        }

        private void CreatePropertyControllers(StackPanel propertiesPanel, string propertyName, UIElement temp)
        {
            DockPanel dp = new DockPanel();
            dp.Children.Add(new Label() { Content = propertyName, Style = FindResource("CenterLabel") as Style });
            DockPanel.SetDock(temp, Dock.Right);
            dp.Children.Add(temp);
            propertiesPanel.Children.Add(new Border() { Style = FindResource("MenuBorder") as Style, Child = dp });
        }
    }
}
