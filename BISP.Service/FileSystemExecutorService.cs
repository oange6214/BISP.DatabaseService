
using BISP.Infra.Entity.Entities;
using BISP.Service.IRepository;
using BISP.ServiceInterface;
using System.Reactive.Linq;
using BISP.FileSystemInterface;

namespace BISP.Service;

public class FileSystemExecutorService : IFileSystemExecutorService
{
    IFileSystemExecutor _fileSystemWatcher;
    IRepository<OfileInfo> _repository;

    public FileSystemExecutorService(IFileSystemExecutor fileSystemWatcher, IRepository<OfileInfo> repository)
    {
        _fileSystemWatcher = fileSystemWatcher;
        _repository = repository;

        Initial();
    }

    public void Start() => _fileSystemWatcher.Start();

    public void Stop() => _fileSystemWatcher.Stop();
    
    public void Dispose() => _fileSystemWatcher.Dispose();

    private void Initial()
    {
        _fileSystemWatcher.Created.Subscribe(e =>
        {
            Console.WriteLine($"File created: {e.FullPath}");

            FileInfo oFileInfo = new(e.FullPath);

            OfileInfo newOFileInfo = new OfileInfo
            {
                Guid = Guid.NewGuid(),
                Name = oFileInfo.Name,
                LastAccessTime = oFileInfo.LastAccessTime,
                LastAccessTimeUtc = oFileInfo.LastAccessTimeUtc,
                LastWriteTime = oFileInfo.LastWriteTime,
                LastWriteTimeUtc = oFileInfo.LastWriteTimeUtc,
                LinkTarget = oFileInfo.LinkTarget,
                FullName = oFileInfo.FullName,
                DirectoryName = oFileInfo.DirectoryName,
                Exists = oFileInfo.Exists,
                Extension = oFileInfo.Extension,
                CreateTime = oFileInfo.CreationTime,
                CreateTimeUtc = oFileInfo.CreationTimeUtc,
            };

            _repository.InsertAsync(newOFileInfo).GetAwaiter().GetResult();
        });

        _fileSystemWatcher.Changed.Subscribe(e =>
        {
            Console.WriteLine($"File changed: {e.FullPath}");
        });

        _fileSystemWatcher.Deleted.Subscribe(e => Console.WriteLine($"File deleted: {e.FullPath}"));
        _fileSystemWatcher.Renamed.Subscribe(e => Console.WriteLine($"File renamed: {e.OldFullPath} renamed to {e.FullPath}"));
        _fileSystemWatcher.Error.Subscribe(e => Console.WriteLine($"Error: {e.GetException().Message}"));
    }

}
