using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebMovie.Models;

namespace WebMovie.ViewModel
{
    public class loginVn
    {
      
            MovieDataDataContext db = new MovieDataDataContext();
            [Required(ErrorMessage = "Username can't be blank")]
            public string tentk { get; set; }
            [Required(ErrorMessage = "Password can't be plank")]
            public string mk { get; set; }
        }
  }