using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MBLabs.Models;
using EuVou.Data;
using Microsoft.EntityFrameworkCore;

namespace MBLabs.Controllers
{
    public class HomeController : Controller
    {
        private readonly EuVouContext _context;
        public HomeController(EuVouContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Event.ToListAsync());
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
