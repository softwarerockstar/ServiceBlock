using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class MenuLinksController : Controller
    {
        // GET: MenuLinks
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}