using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System;
using DisplayUpdateData.Models;
using System.Data.SqlClient;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DisplayUpdateData.Controllers
{

    public class DisplayUpdateController : Controller
    {
        private readonly IConfiguration _configuration;
        public DisplayUpdateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index(LoginModel loginModel)
        {
             try
             {
                var id = HttpContext.Session.GetString("id");
                if(id == "1")
                {
                    List<StoreModel> liststore = new List<StoreModel>();

                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("StoreMvc")))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("GetStore", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        var rd = cmd.ExecuteReader();
                        if (rd.HasRows)
                        {
                            while (rd.Read())
                            {
                                StoreModel storemodel = new StoreModel();
                                storemodel.id = Convert.ToInt32(rd["id"]);
                                storemodel.store_code = rd["store_code"].ToString();
                                storemodel.store_name = rd["store_name"].ToString();
                                storemodel.state = rd["state"].ToString();
                                storemodel.city = rd["city"].ToString();
                                storemodel.Pin = (rd["Pin"]).ToString();
                                storemodel.store_type = (rd["store_type"]).ToString();
                                storemodel.region = rd["region"].ToString();
                                liststore.Add(storemodel);
                            }
                        }
                        con.Close();
                    }
                    return View(liststore);
                }
                else
                {
                    return RedirectToAction("index", "Login");
                }
            }
            catch (Exception ex)
            {
                 throw;
            }
            
        }
        [HttpGet]
        public IActionResult GetEmployeeData(int id)
        {
            try
            {
                StoreModel storemodel = new StoreModel();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("StoreMvc")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("particularId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                 
                    var rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            storemodel.id = Convert.ToInt32(rd["id"]);
                            storemodel.store_code = rd["store_code"].ToString();
                            storemodel.store_name = rd["store_name"].ToString();
                            storemodel.state = rd["state"].ToString();
                            storemodel.city = rd["city"].ToString();
                            storemodel.Pin = (rd["Pin"]).ToString();
                            storemodel.store_type = (rd["store_type"]).ToString();
                            storemodel.region = rd["region"].ToString();
                        }
                    }
                    con.Close();
                }
                return View(storemodel);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public IActionResult Updateemp(int id)
        {
            try
            {
                StoreModel storemodel = new StoreModel();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("StoreMvc")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("particularId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            storemodel.id = Convert.ToInt32(rd["id"]);
                            storemodel.store_code = rd["store_code"].ToString();
                            storemodel.store_name = rd["store_name"].ToString();
                            storemodel.state = rd["state"].ToString();
                            storemodel.city = rd["city"].ToString();
                            storemodel.Pin = (rd["Pin"]).ToString();
                            storemodel.store_type = (rd["store_type"]).ToString();
                            storemodel.region = rd["region"].ToString();
                        }
                    
                    con.Close();
                }
                return View(storemodel);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public IActionResult Updateemp(int id, [Bind] StoreModel storemodel)
        {
            try
            {
               
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("StoreMvc")))
                {
                    SqlCommand cmd = new SqlCommand("Updatedata", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", storemodel.id);
                    cmd.Parameters.AddWithValue("@store_code", storemodel.store_code);
                    cmd.Parameters.AddWithValue("@store_name", storemodel.store_name);
                    cmd.Parameters.AddWithValue("@state", storemodel.state);
                    cmd.Parameters.AddWithValue("@city", storemodel.city);
                    cmd.Parameters.AddWithValue("@Pin", storemodel.Pin);
                    cmd.Parameters.AddWithValue("@store_type", storemodel.store_type);
                    cmd.Parameters.AddWithValue("@region", storemodel.region);

                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result != 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(storemodel);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
         
    }
}
