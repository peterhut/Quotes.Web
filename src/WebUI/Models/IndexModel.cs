using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class IndexModel
    {
        public Quote Quote { get; set; }
        public string BackgroundColor { get; set; }
        public Version Version { get; set; }

        [FromForm]
        public string Data { get; set; }

        [TempData]
        public string Message { get; set; }
    }
}
