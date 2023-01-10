using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ServerlessApp;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ServerlessApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {


            //TaskHelper.ConfigureTestTaskServices(builder.Services);

            //builder.Services.AddHttpClient();

            /* TODO: Add AutoMapper to Dependency Injection */
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddSingleton<ITaskHelper, TaskHelper>();

            
        }
    }
}
