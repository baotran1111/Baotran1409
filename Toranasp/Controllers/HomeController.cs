using Toranasp.Context;
using Toranasp.Models;
using System.Linq;
using System.Web.Mvc;
using ActionResult = System.Web.Mvc.ActionResult;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;


namespace Toranasp.Controllers
{
    public class HomeController : System.Web.Mvc.Controller
    {
        WebBHEntities1 obj = new WebBHEntities1();
        private List<Product> filteredProducts;

        public ActionResult Index()
        {

            HomeModel home = new HomeModel();
            home.ListProduct = obj.Products.ToList();
            home.ListCategory = obj.Categories.ToList();
            home.ListSlider = obj.Sliders.ToList();
            return View(home);
        }
        public ActionResult Search(string searchString)
        {
            HomeModel home = new HomeModel();
            home.ListProduct = obj.Products.Where(p => p.Slug.Contains(searchString)).ToList();
            home.ListCategory = obj.Categories.ToList();
            home.ListSlider = obj.Sliders.ToList();
            return View(home);
        }

        public ActionResult FilterByPrice(string priceRange)
        {

            HomeModel home = new HomeModel();
            home.ListProduct = obj.Products.ToList();
            home.ListCategory = obj.Categories.ToList();
            home.ListSlider = obj.Sliders.ToList();
            // Lọc sản phẩm theo giá
            switch (priceRange)
            {
                case "0-1000":
                    home.ListProduct = home.ListProduct.Where(p => p.Price >= 0 && p.Price < 1000).ToList();
                    break;
                case "1000-2000":
                    home.ListProduct = home.ListProduct.Where(p => p.Price >= 1000 && p.Price < 2000).ToList();
                    break;
                case "2000+":
                    home.ListProduct = home.ListProduct.Where(p => p.Price >= 2000).ToList();
                    break;
                default:
                    // Không có khoảng giá nào được chọn, không cần lọc sản phẩm
                    break;
            }

            // Truyền danh sách sản phẩm đã lọc vào view
            return View("Index", home);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = obj.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(_user);
                }
                else
                {
                    _user.Password = GetMD5(_user.Password);
                    obj.Configuration.ValidateOnSaveEnabled = false;
                    obj.Users.Add(_user);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.error = "Có lỗi xảy ra trong quá trình đăng ký";
            return View(_user);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var userEmail = User.Identity.Name;
            ViewBag.UserEmail = userEmail;

            return View();
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_pssword = GetMD5(password);
                var data = obj.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_pssword)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FirstName"] = data.FirstOrDefault().FirstName;
                    Session["LastName"] = data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            StringBuilder byte2String = new StringBuilder();
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String.Append(targetData[i].ToString("x2"));
            }
            return byte2String.ToString();
        }
    }
}