using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp3;

public partial class Circle : UserControl
{
    private Brush? _previousFill;
    
    public Circle()
    {
        _previousFill = null;
        
        InitializeComponent();
    }
    
    public Circle(Circle c)
    {
        InitializeComponent();
        
        this.circleUI.Height = c.circleUI.Height;
        this.circleUI.Width = c.circleUI.Height;
        this.circleUI.Fill = c.circleUI.Fill;
    }
    
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (e.LeftButton != MouseButtonState.Pressed) return;
        
        var data = new DataObject();
        //data.SetData(DataFormats.StringFormat, this.GetType().Name);
        data.SetData(DataFormats.StringFormat, circleUI.Fill.ToString());
        data.SetData("Double", circleUI.Height);
        data.SetData("Object", this);
            
        DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
    }
    
    protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
    {
        base.OnGiveFeedback(e);
        
        if (e.Effects.HasFlag(DragDropEffects.Copy))
        {
            Mouse.SetCursor(Cursors.Cross);
        }
        else if (e.Effects.HasFlag(DragDropEffects.Move))
        {
            Mouse.SetCursor(Cursors.Pen);
        }
        else
        {
            Mouse.SetCursor(Cursors.No);
        }
        e.Handled = true;
    }
    
    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);
        
        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            var dataString = e.Data.GetData(DataFormats.StringFormat) as string;
            
            var converter = new BrushConverter();
            if (converter.IsValid(dataString))
            {
                var newFill = converter.ConvertFromString(dataString) as Brush;
                circleUI.Fill = newFill;
                
                e.Effects = e.KeyStates.HasFlag(DragDropKeyStates.ControlKey) 
                    ? DragDropEffects.Copy 
                    : DragDropEffects.Move;
            }
        }
        e.Handled = true;
    }
    
    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDragOver(e);
        
        e.Effects = DragDropEffects.None;
        
        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            var dataString = e.Data.GetData(DataFormats.StringFormat) as string;
            
            var converter = new BrushConverter();
            if (converter.IsValid(dataString))
            {
                e.Effects = e.KeyStates.HasFlag(DragDropKeyStates.ControlKey) 
                    ? DragDropEffects.Copy 
                    : DragDropEffects.Move;
            }
        }
        e.Handled = true;
    }
    
    protected override void OnDragEnter(DragEventArgs e)
    {
        base.OnDragEnter(e);
        
        _previousFill = circleUI.Fill;

        if (!e.Data.GetDataPresent(DataFormats.StringFormat)) return;
        
        var dataString = e.Data.GetData(DataFormats.StringFormat) as string;
            
        var converter = new BrushConverter();
        if (!converter.IsValid(dataString)) return;
        
        var newFill = converter.ConvertFromString(dataString) as Brush;
        circleUI.Fill = newFill;
    }
    
    protected override void OnDragLeave(DragEventArgs e)
    {
        base.OnDragLeave(e);
        
        circleUI.Fill = _previousFill;
    }
}