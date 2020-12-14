using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Personal.Finance.WebApi.Controllers
{
    public class FallbackController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Path.Combine(Directory.GetCurrentDirectory()), "Client/Angular", "index.html"),
                "text/HTML");
        }
    }
}
