using BISP.Infra.Entity.Entities;

namespace BISP.ServiceInterface;

public interface IFileListService
{
    Task<IEnumerable<OfileInfo>> GetFileInfosAsync();

    Task<OfileInfo> GetFileInfoByGuidAsync(Guid Guid);

    Task AddFileInfoAsync(OfileInfo ofileInfo);

    Task AddFileInfosAsync(IEnumerable<OfileInfo> ofileInfo);
    Task UpdateFileInfosAsync(OfileInfo ofileInfo);

    Task DeleteFileInfo(Guid guid);

}