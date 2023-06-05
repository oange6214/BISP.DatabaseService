using BISP.Client.WPF.Services;
using BISP.Client.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BISP.Client.WPF;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModel(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {

        });

        return hostBuilder;
    }
}
