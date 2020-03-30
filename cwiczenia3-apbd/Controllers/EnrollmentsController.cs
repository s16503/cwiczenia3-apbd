using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cwiczenia3_apbd.DAL;
using cwiczenia3_apbd.models;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia3_apbd.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;";
        private readonly IDbService _dbService;


        [HttpPost]
        public IActionResult AddStudent(Student student) //nowy student
        {
            if(student.FirstName.Equals("")
                || student.LastName.Equals("")
                || student.BirthDate.Equals("")
                || student.IndexNumber.Equals("")
                || student.Studies.Equals("")
              )
            {
                return BadRequest();
            }

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT  Count(*) as istn  FROM Enrollment " +
                                  "JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy " +
                                  "WHERE Studies.Name = '" + student.Studies + "';" ;

              //  com.Parameters.AddWithValue("index", index);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                int count = 0;
                while (dr.Read())
                {
                    count = (int)dr["istn"];         
                }

                if(count == 0)
                    return BadRequest();

            }


            return Ok("blahahalalalahahala!");


        }
    }
}