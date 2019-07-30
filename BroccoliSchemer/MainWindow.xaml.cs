using BroccoliSchemer.Actions;
using BroccoliSchemer.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        private readonly List<IListable> ComponentsPlacedOnScreen = new List<IListable>();
        private FrameworkElement previewItem = null;
        public MainWindow()
        {
            InitializeComponent();
            if (Components.Items.Count == 0)
            {
                Components.ItemsSource = BaseComponentAction.GetComponents();
            }
            ComponentFilter.ItemsSource = ComponentFilterAction.GetComponentFilters();
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

        private void CanvasBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Components.SelectedIndex > -1)
            {
                GetPropertyValuesForComponent((IListable)Components.SelectedItem);
                AddComponentToCanvas(false);
                RemovePreviewItem();
                Components.UnselectAll();
                PropertiesPanel.Children.Clear();
            }
        }

        private void AddComponentToCanvas(bool isPreviewItem)
        {
            BaseComponent selectedComponent = (BaseComponent)Components.SelectedItem;
            //Control background color of component from colorPicker. But first, generate colorPicker according to items color properties
            Border border = new Border()
            {
                Width = selectedComponent.Width,
                Height = selectedComponent.Height,
                BorderThickness = new Thickness(selectedComponent.BorderThickness),
                Background = new ImageBrush(new BitmapImage(new Uri(selectedComponent.ImagePath, UriKind.Relative))),
                Name = selectedComponent.Name + DateTime.Now.Ticks
            };
            if (isPreviewItem)
            {
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                previewItem = border;
            }
            else
            {
                border.BorderBrush = new SolidColorBrush(selectedComponent.BorderColor);
            }
            SchemerCanvas.Children.Add(border);
            SetComponentPositionOnCanvas(border);
        }

        private void SetComponentPositionOnCanvas(FrameworkElement frameworkElement)
        {
            Canvas.SetLeft(frameworkElement, (double)previewItem.GetValue(Canvas.LeftProperty));
            Canvas.SetTop(frameworkElement, (double)previewItem.GetValue(Canvas.TopProperty));
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
                else if (pt == typeof(double))
                {
                    TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                    item.SetValue(selectedItem, Convert.ToDouble(textBox.Text));
                }
                else if (pt == typeof(decimal))
                {
                    TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                    item.SetValue(selectedItem, Convert.ToDecimal(textBox.Text));
                }
                else if (pt == typeof(Color))
                {
                    TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, "tb" + item.Name);
                    ComboBox comboBox = (ComboBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, "cb" + item.Name);
                    if (textBox.Text != "#00000000")
                    {
                        item.SetValue(selectedItem, (Color)ColorConverter.ConvertFromString(textBox.Text));
                    }
                    else if (comboBox.SelectedIndex != 0)
                    {
                        item.SetValue(selectedItem, (Color)(comboBox.SelectedItem as PropertyInfo).GetValue(1));
                    }
                }
                else if (pt == typeof(string))
                {
                    if (item.Name != "ImagePath" && item.Name != "Name")
                    {
                        TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                        item.SetValue(selectedItem, textBox.Text);
                    }
                }
            }
            ComponentsPlacedOnScreen.Add(selectedItem);
        }

        private void Components_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Components.SelectedIndex > -1)
            {
                CreateComponentPropertiesControllers();
            }
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
                    CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                }
                else if (pt == typeof(double))
                {
                    double temp = (double)item.GetValue(Components.SelectedItem);
                    CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                }
                else if (pt == typeof(decimal))
                {
                    decimal temp = (decimal)item.GetValue(Components.SelectedItem);
                    CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                }
                else if (pt == typeof(Color))
                {
                    Color temp = (Color)item.GetValue(Components.SelectedItem);
                    CreatePropertyControllers(PropertiesPanel, item.Name, new ComboBox() { Style = FindResource("ComboBoxBorder") as Style, ItemsSource = typeof(Colors).GetProperties(), SelectedIndex = 0, ItemTemplate = FindResource("ColorPickerTemplate") as DataTemplate, Name = "cb" + item.Name });
                    CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = "tb" + item.Name });
                }
                else if (pt == typeof(string))
                {
                    if (item.Name != "Name" && item.Name != "ImagePath")
                    {
                        string temp = (string)item.GetValue(Components.SelectedItem);
                        CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp, Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                    }
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

        private void ComponentFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Filter components
        }

        private void SchemerCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (previewItem == null && Components.SelectedIndex > -1)
            {
                UpdatePreviewItem();
            }
        }

        private void SchemerCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (previewItem != null)
            {
                //SetComponentPositionOnCanvas(previewItem);
                AlignComponent(previewItem, Mouse.GetPosition(SchemerCanvas).X, Mouse.GetPosition(SchemerCanvas).Y);
            }
        }

        private void SchemerCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (previewItem != null)
            {
                RemovePreviewItem();
            }
        }

        private void RemovePreviewItem()
        {
            SchemerCanvas.Children.Remove(previewItem);
            previewItem = null;
        }

        private void UpdatePreviewItem()
        {
            GetPropertyValuesForComponent((IListable)Components.SelectedItem);
            AddComponentToCanvas(true);
        }
        public void AlignComponent(FrameworkElement frameworkElement, double x_pos, double y_pos)
        {
            double min_diff = 50000;
            double x = 0, y = 0;

            if (SchemerCanvas.ActualWidth - frameworkElement.Width - 20 < x_pos)
            {
                x_pos = SchemerCanvas.ActualWidth - frameworkElement.Width;
            }
            if (SchemerCanvas.ActualHeight - frameworkElement.Height - 20 < y_pos)
            {
                y_pos = SchemerCanvas.ActualHeight - frameworkElement.Height;
            }
            if (x_pos < 20)
            {
                x_pos = 0;
            }
            if (y_pos < 20)
            {
                y_pos = 0;
            }
            foreach (FrameworkElement item in SchemerCanvas.Children)
            {
                double y_diff = 100000;
                double x_diff = 1000000;
                double current_diff = 40000;
                x = (double)item.GetValue(Canvas.LeftProperty);
                y = (double)item.GetValue(Canvas.TopProperty);

                if ((x - 20 < x_pos) && (x_pos < x + 70)) //left
                {
                    x_diff = Math.Abs(x - x_pos);
                }
                if ((y - 20 < y_pos) && (y_pos < y + 70)) //up
                {
                    y_diff = Math.Abs(y - y_pos);
                }

                current_diff = Math.Min(x_diff, y_diff);
                if (min_diff > current_diff)
                {
                    min_diff = current_diff;
                    if (y_diff == current_diff)
                    {
                        y_pos = y;
                    }
                    else
                    {
                        x_pos = x;
                    }
                }
            }
            Canvas.SetLeft(frameworkElement, x_pos);
            Canvas.SetTop(frameworkElement, y_pos);
        }
    }
}
