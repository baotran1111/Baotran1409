using Toranasp.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toranasp;
using static Toranasp.Common;

namespace Toranasp.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        WebBHEntities1 obj = new WebBHEntities1();
        // GET: Admin/Brand
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstBrand = obj.Brands.Where(n => n.Name.Contains(SearchString)).ToList();

                //lstBrand = obj.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = obj.Brands.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }
        [ValidateInput(false)]

        [HttpPost]
        public ActionResult Create(Brand objBrand)

        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                        //ten hinh
                        string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                        //jpg
                        fileName = fileName + extension;
                        //ten hinh.jpg
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
                    }
                    objBrand.CreatedOnUtc = DateTime.Now;
                    obj.Brands.Add(objBrand);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(objBrand);

                }

            }

            return View(objBrand);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBrand = obj.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = obj.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objPro)
        {
            var objBrand = obj.Brands.Where(n => n.Id == objPro.Id).FirstOrDefault();
            obj.Brands.Remove(objBrand);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var objBrand = obj.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand objBrand, FormCollection form)
        {

            if (objBrand.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                //tenhinh
                string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objBrand.Avatar = fileName;
                objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));

            }
            else
            {
                objBrand.Avatar = form["oldimage"];
                obj.Entry(objBrand).State = EntityState.Modified;
                obj.SaveChanges();
                return RedirectToAction("Index");
            }
            obj.Entry(objBrand).State = EntityState.Modified;
            obj.SaveChanges();
            return RedirectToAction("Index");
        }
        void LoadData()
        {
            Common objCommon = new Common();
            // lấy dữ liệu dưới db
            var lstCat = obj.Categories.ToList();
            //convert  sang select list dạng value,text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");
            // lấy dữ diệu thương hiệu dưới db
            var lstBrand = obj.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            //convert sang select list dang value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loai san pham
            List<BrandType> lstBrandType = new List<BrandType>();
            BrandType objBrandType = new BrandType();
            objBrandType.Id = 01;
            objBrandType.Name = "Giảm giá sốc";
            lstBrandType.Add(objBrandType);

            objBrandType = new BrandType();
            objBrandType.Id = 02;
            objBrandType.Name = "Đề xuất";
            lstBrandType.Add(objBrandType);

            DataTable dtBrandType = converter.ToDataTable(lstBrandType);
            //convert sang select list dang value, text
            ViewBag.BrandType = objCommon.ToSelectList(dtBrandType, "Id", "Name");
        }
    }
}