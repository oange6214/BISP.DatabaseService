namespace BISP.ServiceInterface;

public interface IFileSystemExecutorService
{
    void Start();
    void Stop();

    void Dispose();
}
