using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Text.RegularExpressions;
using System.IO;

namespace Mater.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{

			return View();
		}

	}
}
