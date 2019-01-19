using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FurnitureStoreFront.Controllers
{
    public class CatalogueController : Controller
    {
        public Models.StoreFront.StoreFront StoreFront = new Models.StoreFront.StoreFront();

        // GET: Catalogue
        public ActionResult Index()
        {
            if(HttpContext.Request.RequestType == "POST")
            {
                int i = int.Parse(Request.Form["Room"]);

                switch (i)
                {
                    case 0:
                        StoreFront.Searchstring = "All";
                        break;
                    case 1:
                        StoreFront.Searchstring = "Bedroom";
                        break;
                    case 2:
                        StoreFront.Searchstring = "Livingroom";
                        break;
                    case 3:
                        StoreFront.Searchstring = "Kitchen";
                        break;
                    case 4:
                        StoreFront.Searchstring = "Outdoor";
                        break;
                    case 11:
                        StoreFront.Searchstring = "Desk Chairs";
                        break;
                    case 12:
                        StoreFront.Searchstring = "Writing Desks";
                        break;
                    case 13:
                        StoreFront.Searchstring = "Beds";
                        break;
                    case 21:
                        StoreFront.Searchstring = "Arm Chairs";
                        break;
                    case 22:
                        StoreFront.Searchstring = "Coffe Tables";
                        break;
                    case 23:
                        StoreFront.Searchstring = "Studio Couches";
                        break;
                    case 31:
                        StoreFront.Searchstring = "Kitchen Chair";
                        break;
                    case 32:
                        StoreFront.Searchstring = "Kitchen Table";
                        break;
                    case 41:
                        StoreFront.Searchstring = "Lawn Chair";
                        break;
                    case 42:
                        StoreFront.Searchstring = "Lawn Table";
                        break;
              
                }
            StoreFront.SearchInt = i;
            return View(StoreFront);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}