using System.Web;

namespace Task.Models
{
    public class SigninViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    //public class Img
   // {
       // public HttpPostedFileBase File { get; set; }
   // }

    public class SignupViewModel
    {
        //[Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        //public Img Image { get; set; }
        public HttpPostedFileBase Image { get; set; }
    } 

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
    }
}


