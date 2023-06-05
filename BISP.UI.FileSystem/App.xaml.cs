using BISP.Infra.EfCore;
using BISP.Infra.Entity.Data;
using BISP.Infra.Entity.Entities;
using BISP.Service;
using BISP.Service.IRepository;
using BISP.ServiceInterface;
using BISP.UI.FileSystemByExample.View;
using BISP.UI.FileSystemByExample.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace BISP.UI.FileSystemByExample;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                string defaultDb = hostContext.Configuration.GetSection("ConnectionDB")["Default"];
                string connectionString = hostContext.Configuration.GetConnectionString(defaultDb);

                // -- EfCore
                var options = new DbContextOptionsBuilder<BispContext>()
                .UseNpgsql(connectionString)
                .Options;

                services.AddSingleton<IRepository<OfileInfo>>(s => new EfRepository<OfileInfo>(new BispContext(options)));

                services.AddFileSystem(@"D:\\Demo", "*.*");
                services.AddScoped<IFileSystemExecutorService, FileSystemExecutorService>();
                services.AddScoped<IFileListService, FileListService>();

                services.AddSingleton<FileListViewModel>();
                services.AddSingleton(s => new FileListView()
                {
                    DataContext = s.GetRequiredService<FileListViewModel>()
                });

                services.AddSingleton<MainViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.Dispose();

        base.OnExit(e);
    }

}
