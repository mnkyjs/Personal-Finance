using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Personal.Finance.WebApi.Controllers
{
    [AllowAnonymous]
    public class FallbackController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Path.Combine(Directory.GetCurrentDirectory()), "wwwroot", "index.html"),
                "text/HTML");
        }
    }
}