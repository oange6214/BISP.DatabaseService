using BISP.Infra.Entity.Entities;
using BISP.Service.IRepository;
using BISP.ServiceInterface;

namespace BISP.Service;

public class FileListService : IFileListService
{
    private readonly IRepository<OfileInfo> _fileInfoRepository;


    public FileListService(IRepository<OfileInfo> fileInfoRepository)
    {
        _fileInfoRepository = fileInfoRepository;
    }

    public async Task<IEnumerable<OfileInfo>> GetFileInfosAsync()
    {
        return await _fileInfoRepository.GetAllAsync();
    }

    public async Task<OfileInfo> GetFileInfoByGuidAsync(Guid Guid)
    {
        return await _fileInfoRepository.GetByIdAsync(Guid);
    }

    public async Task AddFileInfoAsync(OfileInfo ofileInfo)
    {
        await _fileInfoRepository.InsertAsync(ofileInfo);
    }

    public async Task AddFileInfosAsync(IEnumerable<OfileInfo> fileInfos)
    {
        await _fileInfoRepository.InsertRangeAsync(fileInfos);
    }

    public async Task UpdateFileInfosAsync(OfileInfo fileInfo)
    {
        await _fileInfoRepository.Update(fileInfo);
    }

    public async Task DeleteFileInfo(Guid guid)
    {
        await _fileInfoRepository.Delete(guid);
    }
}