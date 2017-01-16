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

        foreach (var prop in typeof(SystemColors).GetProperties().Where(p => p.PropertyType == typeof(SolidColorBrush)))
        {
            var brush = prop.GetValue(null) as SolidColorBrush;
            var rect = new Rectangle() { Width = 100, Height = 20, Fill = brush };
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(new TextBlock() { Text = prop.Name, Width = 200 });
            panel.Children.Add(rect);
            panel.Children.Add(new TextBlock() { Text = brush.Color.ToString(), Margin = new Thickness(8, 0, 8, 0) });
            content.Items.Add(panel);
        }

        foreach (var prop in typeof(Colors).GetProperties().Where(p => p.PropertyType == typeof(Color)))
        {
            var color = (Color)prop.GetValue(null);
            sb.AppendLine($"{color.R / 255.0}, {color.G / 255.0}, {color.B / 255.0},");
            //sb.AppendLine($"{color.ScR}, {color.ScG}, {color.ScB},");
        }

        // Clipboard.SetText(sb.ToString());

        window.Content = content;

        app.Run(window);
    }
}