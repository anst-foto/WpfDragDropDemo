using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp3;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void panel_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent("Object"))
        {
            e.Effects = e.KeyStates == DragDropKeyStates.ControlKey 
                ? DragDropEffects.Copy 
                : DragDropEffects.Move;
        }
    }
    
    private void panel_Drop(object sender, DragEventArgs e)
    {
        if (e.Handled) return;
        
        var panel = sender as Panel;
        var element = e.Data.GetData("Object") as UIElement;

        if (panel == null || element == null) return;

        if (VisualTreeHelper.GetParent(element) is not Panel parent) return;
        
        if (e.KeyStates == DragDropKeyStates.ControlKey &&
            e.AllowedEffects.HasFlag(DragDropEffects.Copy))
        {
            //var circle = new Circle((Circle)element);
            var circle = new Rectangle()
            {
                Height = ((Circle)element).circleUI.Height,
                Width = ((Circle)element).circleUI.Width,
                Fill = ((Circle)element).circleUI.Fill,
            };
            panel.Children.Add(circle);
                        
            e.Effects = DragDropEffects.Copy;
        }
        else if (e.AllowedEffects.HasFlag(DragDropEffects.Move))
        {
            parent.Children.Remove(element);
            panel.Children.Add(element);
                        
            e.Effects = DragDropEffects.Move;
        }
    }
}