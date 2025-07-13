using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uweb4Media.Application.Services
{
    public static class ServiceRegistiration
    {
        public static void AddApplicationService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cg => cg.RegisterServicesFromAssembly(typeof(ServiceRegistiration).Assembly));
        }
    }
}
