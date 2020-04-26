using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using AccountingNotebook.Service.AccountService;
using AccountingNotebook.Service.TransactionService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingNotebook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters();            
            services.AddSingleton<ITransactionService, TransactionsService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ITransactionHistoryService<Transaction>, TransactionsHistory>();
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver
            //                            as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});          
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseMvc();           
        }
    }
}
