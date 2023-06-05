using BISP.Base;
using BISP.Infra.Entity.Entities;
using BISP.Service;
using BISP.Service.IRepository;
using BISP.ServiceInterface;
using BISP.FileSystemInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BISP.UI.FileSystemByExample.ViewModel;

public class FileListViewModel : ViewModelBase
{
    #region Fields

    private readonly IHost _host;
    private readonly IFileListService _fileListService;
    private readonly FileSystemExecutorService _file;
    private ObservableCollection<OfileInfo> _ofileInfos;

    #endregion


    #region Ctors

    public FileListViewModel(IHost host, IFileListService fileListService)
    {
        _host = host;
        _fileListService = fileListService;
        _ofileInfos = new();

        var watcher = _host.Services.GetRequiredService<IFileSystemExecutor>();
        var db = _host.Services.GetRequiredService<IRepository<OfileInfo>>();

        _file = new(watcher, db);
        _file.Start();

        GetFiles().ConfigureAwait(false);
    }

    #endregion


    #region Properties

    public ObservableCollection<OfileInfo> OfileInfos
    {
        get => _ofileInfos;
        set
        {
            _ofileInfos = value;
            OnPropertyChanged();
        }
    }

    #endregion


    #region Private Methods

    private async Task GetFiles()
    {
        try
        {
            OfileInfos = new ObservableCollection<OfileInfo>(await _fileListService.GetFileInfosAsync());
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

    #endregion


}

