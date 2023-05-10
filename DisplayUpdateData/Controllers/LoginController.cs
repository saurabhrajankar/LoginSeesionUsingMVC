using DisplayUpdateData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DisplayUpdateData.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public LoginController(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind]LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("StoreMvc")))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("Logins", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", loginModel.Email);
                    cmd.Parameters.AddWithValue("@Password", loginModel.Password);
                    //cmd.Parameters.AddWithValue("@Role", loginModel.Role);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        HttpContext.Session.SetString("id", reader["id"].ToString());
                        HttpContext.Session.SetString("Email", reader["Email"].ToString());
                        
                        con.Close();
                        //Convert.ToInt32(objReader["Id"]);
                        return RedirectToAction("index", "DisplayUpdate");
                        
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Login Failed";
                    }

                    reader.Close();
                }
            }
            return View(loginModel);
        }
    }
    
}
