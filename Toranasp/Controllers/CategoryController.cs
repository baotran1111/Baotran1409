using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toranasp.Context;

namespace Toranasp.Controllers
{
    public class CategoryController : Controller
    {
        WebBHEntities1 obj = new WebBHEntities1();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory = obj.Categories.ToList();
            return View(lstCategory);
        }
        public ActionResult ListCategory()
        {
            var listProduct = obj.Categories.ToList();
            return View(listProduct);
        }
        public ActionResult ProductCategory(int Id)
        {

            var listProduct = obj.Products.Where(n => n.CategoryId == Id).ToList();
            return View(listProduct);
        }
    }
}