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
        private readonly Dictionary<FrameworkElement,IListable> ComponentsPlacedOnScreen = new Dictionary<FrameworkElement, IListable>();
        private FrameworkElement previewItem = null;
        private KeyValuePair<FrameworkElement, IListable> componentToUpdate = new KeyValuePair<FrameworkElement, IListable>();
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
            Close();
        }

        private void MinimizingButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ChangeStateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState.Equals(WindowState.Normal))
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void CanvasBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Components.SelectedIndex == -1 && VisualTreeHelper.HitTest(this, e.GetPosition(this)).VisualHit.GetType() != typeof(Canvas)) {
                FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.HitTest(this, e.GetPosition(this)).VisualHit;
                foreach (var item in ComponentsPlacedOnScreen.Keys) {
                    
                    if (item.Name == frameworkElement.Name) {
                        Components.SelectedItem = ComponentsPlacedOnScreen[item];
                        componentToUpdate = new KeyValuePair<FrameworkElement, IListable>(item, ComponentsPlacedOnScreen[item]);
                        CreateComponentPropertiesControllers();
                        Components.UnselectAll();
                        break;
                    }
                }
            }
            else if (Components.SelectedIndex > -1)
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
            Border border = new Border()
            {
                Width = selectedComponent.ScreenWidth,
                Height = selectedComponent.ScreenHeight,
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
            ComponentsPlacedOnScreen.Add(border, (IListable)Components.SelectedItem);
        }

        private void SetComponentPositionOnCanvas(FrameworkElement frameworkElement)
        {
            if (componentToUpdate.Key != null) {
                Canvas.SetLeft(frameworkElement, (double)componentToUpdate.Key.GetValue(Canvas.LeftProperty));
                Canvas.SetTop(frameworkElement, (double)componentToUpdate.Key.GetValue(Canvas.TopProperty));
            }
            else {
                Canvas.SetLeft(frameworkElement, (double)previewItem.GetValue(Canvas.LeftProperty));
                Canvas.SetTop(frameworkElement, (double)previewItem.GetValue(Canvas.TopProperty));
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
                else if (pt == typeof(double))
                {
                    if (item.Name != "Width" && item.Name != "Height") 
                    {
                        TextBox textBox = (TextBox)LogicalTreeHelper.FindLogicalNode(PropertiesPanel, item.Name);
                        item.SetValue(selectedItem, Convert.ToDouble(textBox.Text));
                    }
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
                    if (textBox.Text != "#00000000" || string.IsNullOrWhiteSpace(textBox.Text))
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
                    if (item.Name != "Width" && item.Name != "Height") 
                    {
                        double temp = (double)item.GetValue(Components.SelectedItem);
                        CreatePropertyControllers(PropertiesPanel, item.Name, new TextBox() { Text = temp.ToString(), Style = FindResource("PropertyTextBox") as Style, Name = item.Name });
                    }
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
            ComponentsPlacedOnScreen.Remove(previewItem);
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

            if (SchemerCanvas.ActualWidth - frameworkElement.Width - 5 < x_pos)
            {
                x_pos = SchemerCanvas.ActualWidth - frameworkElement.Width;
            }
            if (SchemerCanvas.ActualHeight - frameworkElement.Height - 5 < y_pos)
            {
                y_pos = SchemerCanvas.ActualHeight - frameworkElement.Height;
            }
            if (x_pos < 10)
            {
                x_pos = 0;
            }
            if (y_pos < 10)
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

                if ((x - 5 < x_pos) && (x_pos < x + 55)) //left
                {
                    x_diff = Math.Abs(x - x_pos);
                }
                if ((y - 5 < y_pos) && (y_pos < y + 55)) //up
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

        private void CanvasBorder_RightClick(object sender, MouseButtonEventArgs e) {
            FrameworkElement itemToDelete = (FrameworkElement)VisualTreeHelper.HitTest(this, e.GetPosition(this)).VisualHit;
            ComponentsPlacedOnScreen.Remove(itemToDelete);
            SchemerCanvas.Children.Remove(itemToDelete);
        }

        private void Apply_Click(object sender, RoutedEventArgs e) {
            if (componentToUpdate.Key != null)
            {
                GetPropertyValuesForComponent(componentToUpdate.Value);
                Components.SelectedItem = componentToUpdate.Value;
                SchemerCanvas.Children.Remove(componentToUpdate.Key);
                AddComponentToCanvas(false);
                componentToUpdate = new KeyValuePair<FrameworkElement, IListable>(null, null);
                Components.UnselectAll();
            }
        }
    }
}
