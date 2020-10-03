using Reversi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Reversi
{
    public static class ServiceProviderExtensions
    {
        public static void AddPlayerService(this IServiceCollection services)
        {
            services.AddSingleton<PlayerService>();
        }

        public static void AddBoardService(this IServiceCollection services)
        {
            services.AddSingleton<BoardService>();
        }
    }
}
