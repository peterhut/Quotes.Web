using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using WebUI;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IQuoteClient _client;
        private IConfiguration _config;

        public HomeController(IQuoteClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quote = await _client.GetRandomQuote();
            var model = new IndexModel()
            {
                Data = Words.GetWord(),
                Quote = quote,
                BackgroundColor = Environment.GetEnvironmentVariable("BACKGROUND_COLOR")
            };

            var envVersion = Environment.GetEnvironmentVariable("VERSION");
            if (envVersion != null)
            {
                model.Version = new Version(envVersion.ToString());
            }
            else
            {
                model.Version = new Version("1.0.0");
            }
            
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Index(IndexModel model)
        {
            if (!Words.IsWordValid(model.Data))
            {
                TempData["Message"] = "Not saved. Nice try.";
                return RedirectToAction("Index");
            }

            var storageAccount = CloudStorageAccount.Parse(_config["StorageConnectionString"]);

            // Create the queue client.
            var queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            var queue = queueClient.GetQueueReference(_config["QueueName"]);
            await queue.CreateIfNotExistsAsync();
            await queue.AddMessageAsync(new CloudQueueMessage(model.Data));

            TempData["Message"] = "Got it!, want to give us more data?";
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            var model = new AboutModel();

            try
            {
                model.Message = "Debugging Info.";
                model.BackgroundColor = Environment.GetEnvironmentVariable("BACKGROUND_COLOR");
                var path = Environment.GetEnvironmentVariable("DOCKER_REGISTRY_SERVER_URL");
                if (!string.IsNullOrEmpty(path))
                {
                    model.RegistryUrl = path.Replace("/", "");
                    model.RegistryIp = System.Net.Dns.GetHostAddresses(model.RegistryUrl)[0].ToString();
                }
                model.HostName = Environment.GetEnvironmentVariable("COMPUTERNAME") ??
                                                Environment.GetEnvironmentVariable("HOSTNAME");
                model.OsArchitecture = RuntimeInformation.OSArchitecture.ToString();
                model.OsDescription = RuntimeInformation.OSDescription;
                model.ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
                model.FrameworkDescription = RuntimeInformation.FrameworkDescription;
                model.AspNetCorePackageVersion = Environment.GetEnvironmentVariable("ASPNETCORE_PKG_VERSION");
                model.AspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                model.ImageBuildDate = Environment.GetEnvironmentVariable("IMAGE_BUILD_DATE");
                model.BaseImageVersion = Environment.GetEnvironmentVariable("BASE_IMAGE_VERSION");
            }
            catch (System.Exception ex)
            {
                model.Message = ex.ToString();
            }
            StringBuilder envVars = new StringBuilder();
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                envVars.Append(string.Format("<strong>{0}</strong>:{1}<br \\>", de.Key, de.Value));
            }

            model.EnvironmentVariables = envVars.ToString();
            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> ScaleOut(int? n = 10)
        {
            if (n.HasValue)
            {
                var tasks = Enumerable.Range(0, n.Value).Select(i => _client.GetRandomQuote());
                var quotes = await Task.WhenAll(tasks);

                var model = new ScaleOutViewModel()
                {
                    N = n.Value,
                    AllQuotes = quotes
                };
                return View(model);
            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        public IActionResult ScaleOut(ScaleOutViewModel model)
        {
            return RedirectToAction("ScaleOut", new { n = model.N });
        }

        private static BigInteger CalcFactorial(BigInteger i)
        {
            if (i <= 1)
                return 1;
            return i * CalcFactorial(i - 1);
        }
    }
}
