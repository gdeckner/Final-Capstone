using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;

namespace WebApplication.Web.Controllers
{
    public class DemoController : Controller
    {
        private readonly IDEMO_DAL demoDal;

        public DemoController(IDEMO_DAL demoDal)
        {
            this.demoDal = demoDal;
        }
   
        public IActionResult Index()
        {
            demoDal.DemoReset();
            return RedirectToAction("index","Home");
        }
    }
}