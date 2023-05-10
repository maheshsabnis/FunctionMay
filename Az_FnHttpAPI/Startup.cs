using Az_FnHttpAPI.Models;
using Az_FnHttpAPI.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// Load the Startup class, in the host and start processing it
[assembly:FunctionsStartup(typeof(Az_FnHttpAPI.Startup))]
namespace Az_FnHttpAPI
{
    /// <summary>
    /// FunctionsStartup: The class responseble for Providing
    /// Dependency Container for all objects required for Function to execute
    /// This will be the the First Class that will be invoked by The Hosting Environment
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// IFunctionsHostBuilder: The contract that is uysed by the Hosting Env.
        /// to register required objects (e.g. DataAccess, Utility Objects, Domain Objects, etc) 
        /// in DI Container
        /// This defines a 'Services' property of the type IServiceCollection that represents all
        /// objects registerd in DI Container
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            /// Registering Data Access Class in DI Container
            builder.Services.AddDbContext<CompanyContext>(options => 
            {
                options.UseSqlServer("Data Source=.;Initial Catalog=Company;Integrated Security=SSPI");
            });

            /// register the DepartmentService class in DI Container
            ///                        Implementation Type, Service Type 
            builder.Services.AddScoped<IServices<Department,int>, DepartmentService>();

        }
    }
}
