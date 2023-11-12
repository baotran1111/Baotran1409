﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toranasp.Models
{
    public partial class UserMasterData
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ")]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Tên Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
    }
}