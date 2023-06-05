using BISP.UI.FileSystemByExample.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows.Controls;

namespace BISP.UI.FileSystemByExample.ViewModel;

public class MainViewModel
{
    #region Properties

    public ContentControl CurrentViewModel { get; set; }

    public IHost Host { get; }

    #endregion

    public MainViewModel(IHost host)
    {
        Host = host;

        CurrentViewModel = host.Services.GetRequiredService<FileListView>();
    }
}
