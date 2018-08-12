using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class ScaleOutViewModel
    {
        [FromForm]
        public long N { get; set; }

        public Quote[] AllQuotes { get; set; }
    }
}
