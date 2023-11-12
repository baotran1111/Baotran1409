using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toranasp.Context;

namespace Toranasp.Models
{
    public class HomeModel
    {
        public List<Product> ListProduct { set; get; }
        public List<Category> ListCategory { set; get; }
        public List<Slider> ListSlider { set; get; }
    }
}