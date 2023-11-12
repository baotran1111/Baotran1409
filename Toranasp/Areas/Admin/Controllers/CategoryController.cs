using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toranasp.Context;
using static Toranasp.Common;

namespace Toranasp.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        WebBHEntities1 obj = new WebBHEntities1();
        // GET: Admin/Categories
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstCategory = new List<Category>();
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
                lstCategory = obj.Categories.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstCategory = obj.Categories.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        [ValidateInput(false)]

        [HttpPost]
        public ActionResult Create(Category objCategory)

        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objCategory.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                        //ten hinh
                        string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                        //jpg
                        fileName = fileName + extension;
                        //ten hinh.jpg
                        objCategory.Avatar = fileName;
                        objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category"), fileName));
                    }
                    objCategory.CreatedOnUtc = DateTime.Now;
                    obj.Categories.Add(objCategory);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(objCategory);

                }

            }

            return View(objCategory);
        }




        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = obj.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = obj.Categories.Where(n => n.Id == id).FirstOrDefault();

            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objPro)
        {
            var objCategory = obj.Categories.Where(n => n.Id == objPro.Id).FirstOrDefault();
            obj.Categories.Remove(objCategory);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var objCategory = obj.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category objCategory, FormCollection form)
        {

            if (objCategory.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                //tenhinh
                string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objCategory.Avatar = fileName;
                objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category"), fileName));

            }
            else
            {
                objCategory.Avatar = form["oldimage"];
                obj.Entry(objCategory).State = EntityState.Modified;
                obj.SaveChanges();
                return RedirectToAction("Index");
            }
            obj.Entry(objCategory).State = EntityState.Modified;
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
            List<CategoryType> lstCategoryType = new List<CategoryType>();
            CategoryType objCategoryType = new CategoryType();
            objCategoryType.Id = 01;
            objCategoryType.Name = "Giảm giá sốc";
            lstCategoryType.Add(objCategoryType);

            objCategoryType = new CategoryType();
            objCategoryType.Id = 02;
            objCategoryType.Name = "Đề xuất";
            lstCategoryType.Add(objCategoryType);

            DataTable dtCategoryType = converter.ToDataTable(lstCategoryType);
            //convert sang select list dang value, text
            ViewBag.CategoryType = objCommon.ToSelectList(dtCategoryType, "Id", "Name");
        }


    }
}