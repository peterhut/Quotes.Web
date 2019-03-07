using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class AboutModel
    {
        public string Message { get; set; }
        public string RegistryIp { get; set; }
        public string HostName { get; set; }
        public string OsArchitecture { get; set; }
        public string OsDescription { get; set; }
        public string ProcessArchitecture { get; set; }
        public string FrameworkDescription { get; set; }
        public string AspNetCorePackageVersion { get; set; }
        public string AspNetCoreEnvironment { get; set; }
        public string EnvironmentVariables { get; set; }
        public string RequestHeaders { get; set; }
        public string ImageBuildDate { get; set; }
        public string BaseImageVersion { get; set; }
        public string RegistryUrl { get; set; }
        public string BackgroundColor { get; set; }

    }
}
