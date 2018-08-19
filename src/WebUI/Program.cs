using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebUI{
    public class Program{
        public static IWebHostBuilder CreateWebHostBuilder(string[] args){
            var builder = WebHost.CreateDefaultBuilder(args);
            builder.UseStartup<Startup>();
            return builder;
        }
        public static void Main(string[] args){
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}
