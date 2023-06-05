using BISP.Client.WPF.HostBuilders;
using BISP.Client.WPF.Services;
using BISP.Client.WPF.Store;
using BISP.Client.WPF.ViewModel;
using BISP.Service;
using BISP.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace BISP.Client.WPF;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .AddViewModel()
            .AddDatabase()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<RecipeListViewModel>();
                services.AddSingleton<Func<RecipeListViewModel>>((s) => () => s.GetRequiredService<RecipeListViewModel>());
                services.AddSingleton<NavigationService<RecipeListViewModel>>();

                services.AddSingleton<MainViewModel>();

                services.AddSingleton<IRecipeListService, RecipeListService>();

                services.AddSingleton<NavigationStore>();

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

        NavigationService<RecipeListViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<RecipeListViewModel>>();
        navigationService.Navigation();

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
