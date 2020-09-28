using Reversi.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi
{
    public static class ServiceProviderExtensions
    {
        public static void AddGameService(this IServiceCollection services)
        {
            services.AddSingleton<GameService>();
        }
    }
}
