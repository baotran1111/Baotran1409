using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toranasp.Context;
using Toranasp.Models;

namespace Toranasp.Controllers
{
    public class ProductController : Controller
    {
        WebBHEntities1 obj = new WebBHEntities1();
        // GET: Product
        public ActionResult Detail(int Id)
        {
            var objProduct = obj.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        public ActionResult ListProduct()
        {
            HomeModel home = new HomeModel();
            home.ListProduct = obj.Products.ToList();
            home.ListCategory = obj.Categories.ToList();
            home.ListSlider = obj.Sliders.ToList();
            return View(home);
        }
    }
}