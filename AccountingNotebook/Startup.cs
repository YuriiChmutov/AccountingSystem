using AccountingNotebook.Abstractions;
using AccountingNotebook.Models;
using AccountingNotebook.Service.AccountService;
using AccountingNotebook.Service.TransactionHistoryService;
using AccountingNotebook.Service.TransactionService;
using Microsoft.AspNetCore.Builder;
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
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddJsonFormatters();

            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<IAccountService, InMemoryAccountService>();
            services.AddSingleton<ITransactionHistoryService<Transaction>, TransactionsHistoryService>();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
