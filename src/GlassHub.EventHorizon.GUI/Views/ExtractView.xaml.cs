using System.Windows;
using System.Windows.Controls;
using GlassHub.EventHorizon.GUI.ViewModels;

namespace GlassHub.EventHorizon.GUI.Views;

public partial class ExtractView : UserControl
{
    public ExtractView()
    {
        InitializeComponent();
    }

    private void DropZoneExtract_DragOver(object sender, DragEventArgs e)
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

    private void DropZoneExtract_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop) && DataContext is ExtractViewModel vm)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                vm.SetSourceArchive(files[0]);
            }
        }
    }
}
