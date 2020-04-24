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
            // todo: why transient? :)
            services.AddTransient<ITransactionService, TransactionsService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransactionHistoryService<Transaction>, TransactionsHistory>();
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver
            //                            as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // todo: remove or explain why we need them
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();
            app.UseMvc();           
        }
    }
}
