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
        public static void AddBoardService(this IServiceCollection services)
        {
            services.AddSingleton<BoardService>();
        }

        public static void AddPlayerService(this IServiceCollection services)
        {
            services.AddSingleton<PlayerService>();
        }
    }
}
