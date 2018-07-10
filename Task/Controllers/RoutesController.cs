using System;
//using System.Globalization;
//using System.Threading.Tasks;
//using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Task.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Task.Controllers
{
    [Authorize]
    public class RoutesController : Controller
    {
        //
        // Get: /Routes/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //
        // Get: /Routes/Profile
        [AllowAnonymous]
        public ActionResult Profile()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Signin", "Routes");
            }
            else
            {
                return View();
            }
        }
      
        //
        // GET: /Routes/Signup
        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }

        //
        // POST: /Routes/Signup
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Signup(SignupViewModel user)
        {
            if (user.Password == user.ConfirmPassword)
            {
                string majerConnection2 = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection sqlConnection2 = new SqlConnection(majerConnection2);

                string sqlStatment2 = "Insert into [dbo].[users]([Email] , [Password]) values (@Email , @Password)";
                SqlCommand sqlComm2 = new SqlCommand(sqlStatment2, sqlConnection2);

                string sqlStatment3 = "select Email from [dbo].[users] where Email=@Email";
                SqlCommand sqlComm3 = new SqlCommand(sqlStatment3, sqlConnection2);

                sqlConnection2.Open();
            
                //SqlDataReader readData = sqlComm2.ExecuteNonQuery();

                sqlComm3.Parameters.AddWithValue("@Email", user.Email);



                //var repeated = sqlComm3.ExecuteScalar();
                SqlDataReader repeated = sqlComm3.ExecuteReader();

                if (repeated.Read())//repeated != null)
                {
                    sqlConnection2.Close();
                    return View();
                }
                else
                {
                    //Console.WriteLine("gggg");
                    sqlConnection2.Close();

                    sqlConnection2.Open();
                    sqlComm2.Parameters.AddWithValue("@Email", user.Email);
                    // Password Encryption
                    byte[] encode = System.Text.Encoding.UTF8.GetBytes(user.Password);
                    var afterEncoded = Convert.ToBase64String(encode);

                    sqlComm2.Parameters.AddWithValue("@Password", afterEncoded);
               
                    if (user.Image != null && user.Image.ContentLength > 0)
                    {
                        var ImgName = Path.GetFileName(user.Image.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Img"), ImgName);
                        user.Image.SaveAs(path);

                        sqlComm2.Parameters.AddWithValue("@Image", path);

                        int createData = sqlComm2.ExecuteNonQuery();
                        sqlConnection2.Close();
                
                        if (createData >= 1)
                        {
                            Session["Email"] = user.Email;
                            return RedirectToAction("Profile", "Routes");
                            //return Content(afterEncoded);
                            //return Content(path);
                        }
                        else
                        {
                            return View();
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            } 
            else
            {
                return View();
            }
        }

        //
        // GET: /Routes/Signin
        [AllowAnonymous]
        public ActionResult Signin()
        {
            return View();
        }

        //
        // POST: /Routes/Signin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Signin(SigninViewModel user) //user = req.body
        {
            string majerConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(majerConnection);

            string sqlStatment = "select Email , Password from [dbo].[users] where Email=@Email and Password=@Password";
            SqlCommand sqlComm = new SqlCommand(sqlStatment, sqlConnection);

            sqlConnection.Open();
            sqlComm.Parameters.AddWithValue("@Email", user.Email);

            //Password Decryption
            //byte[] decode = Convert.FromBase64String(user.Password);
            //var afterDecode = System.Text.Encoding.UTF8.GetString(decode);

            // Password Encryption
            byte[] encodee = System.Text.Encoding.UTF8.GetBytes(user.Password);
            var afterEncodedAgain = Convert.ToBase64String(encodee);

            //sqlComm.Parameters.AddWithValue("@Password", afterDecode);
            //sqlComm.Parameters.AddWithValue("@Password", user.Password);
            sqlComm.Parameters.AddWithValue("@Password", afterEncodedAgain);
            SqlDataReader readData = sqlComm.ExecuteReader();

            if (readData.Read())
            {
                Session["Email"] = user.Email;
                return RedirectToAction("Profile", "Routes");
                //return Content(afterDecode);
                //return Content(user.Password);
            }
            else
            {
                return View();
                //return Content(user.Password);
            }
            sqlConnection.Close();
        }

        //
        // Get: /Routes/Signout
        [AllowAnonymous]
        public ActionResult Signout()
        {
            Session["Email"] = null;
            return RedirectToAction("Signup", "Routes");
        }

        //  [HttpPost]
        // [ValidateAntiForgeryToken]
    }
}