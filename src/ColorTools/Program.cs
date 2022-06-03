using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var app = new Application();
        var window = new Window();
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.WindowState = WindowState.Maximized;
        window.Title = "SystemColors";

        var content = new ListBox();

        var sb = new StringBuilder();

        var brushProperties = typeof(SystemColors)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(SolidColorBrush))
            .OrderBy(p => p.Name)
            .ToArray();

        var colorProperties = typeof(SystemColors)
            .GetProperties()
            .Where(p => p.PropertyType == typeof(Color))
            .OrderBy(p => p.Name)
            .ToArray();

        sb.AppendLine();

        foreach (var prop in brushProperties)
        {
            var brush = prop.GetValue(null) as SolidColorBrush;

            var name = prop.Name;
            var colorText = brush.Color.ToString().Replace("#FF", "#");
            var rect = new Rectangle() { Width = 100, Height = 20, Fill = brush };
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(new TextBlock() { Text = name, Width = 200 });
            panel.Children.Add(rect);
            panel.Children.Add(new TextBlock() { Text = colorText, Margin = new Thickness(8, 0, 8, 0) });
            content.Items.Add(panel);

            sb.AppendLine($"  <SolidColorBrush x:Key=\"{{x:Static SystemColors.{name}Key}}\" Color=\"{colorText}\" />");
        }

        sb.AppendLine();

        foreach (var prop in colorProperties)
        {
            var color = (Color)prop.GetValue(null);
            var name = prop.Name;
            //sb.AppendLine($"{color.R / 255.0:X2}, {color.G / 255.0:X2}, {color.B / 255.0:X2},");
            //sb.AppendLine($"{color.ScR}, {color.ScG}, {color.ScB},");

            sb.AppendLine($"  <Color x:Key=\"{{x:Static SystemColors.{name}Key}}\" R=\"{color.R}\" G=\"{color.G}\" B=\"{color.B}\" A=\"{color.A}\"/>");
        }

        var text = sb.ToString();

        text = string.Format(GetTemplate(), text);

        Clipboard.SetText(text);

        window.Content = content;

        app.Run(window);
    }

    public static string GetTemplate()
    {
        return @"<ResourceDictionary
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
{0}
</ResourceDictionary>";
    }
}