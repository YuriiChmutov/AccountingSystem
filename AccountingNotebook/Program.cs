using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Net;

namespace AccountingNotebook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    // todo: delete IIS
                    // todo: format + eng
                    // todo: check which options are really required
                    // задает максимально количество оновременно открытых соединений
                    options.Limits.MaxConcurrentConnections = 100;
                    //устанавливает максимальный размер для запроса в байтах
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    //задает минимальную скорость передачи данных в запросе в байтах в секунду
                    options.Limits.MinRequestBodyDataRate =        
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    //задает минимальную скорость передачи данных в исходящем потоке в байтах в секунду
                    options.Limits.MinResponseDataRate =           
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Loopback, 5000);
                })
                .Build();
    }
}
