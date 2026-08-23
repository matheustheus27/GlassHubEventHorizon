using System.Windows;
using System.Windows.Controls;
using GlassHub.EventHorizon.GUI.ViewModels;

namespace GlassHub.EventHorizon.GUI.Views;

public partial class CompressView : UserControl
{
    public CompressView()
    {
        InitializeComponent();
    }

    private void DropZone_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effects = DragDropEffects.Copy;
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    private void DropZone_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop) && DataContext is CompressViewModel vm)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            vm.AddPaths(files);
        }
    }
}
