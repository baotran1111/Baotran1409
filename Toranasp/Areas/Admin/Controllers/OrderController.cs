using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toranasp.Context;

namespace Toranasp.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        WebBHEntities1 obj = new WebBHEntities1();
        // GET: Admin/Orders
        public ActionResult Index()
        {
            var listOrder = obj.Orders.ToList();
            return View(listOrder);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var order = obj.Orders.Where(n => n.Id == id).FirstOrDefault();
            return View(order);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var order = obj.Orders.SingleOrDefault(n => n.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }



        [HttpGet]
        public ActionResult Delete(int Id)
        {

            var objorder = obj.Orders.Where(n => n.Id == Id).FirstOrDefault();

            return View(objorder);
        }
        [HttpPost]
        public ActionResult Delete(Order objor)
        {

            var objorder = obj.Orders.Where(n => n.Id == objor.Id).FirstOrDefault();
            obj.Orders.Remove(objorder);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}