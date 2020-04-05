using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cwiczenia3_apbd.DAL;
using cwiczenia3_apbd.DTOs.Requests;
using cwiczenia3_apbd.models;
using cwiczenia3_apbd.Services;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia3_apbd.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        // private const string ConString = "Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;";
        // private readonly IDbService _dbService;

        private IStudentDbService _service;

        //public EnrollmentsController(IStudentDbService service)
        //{
        //    _service = service;
        //}

       
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request) //nowy student
        {

            var enrollment = new Enrollment();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
            using (var com = new SqlCommand())
            {


                 com.Connection = con;
                com.CommandText = "select IdStudy from Studies where name=@name";
                com.Parameters.AddWithValue("name", request.Studies);
                con.Open();

                    var tran = con.BeginTransaction("SampleTransaction");
                    com.Transaction = tran;


                
                try
                {
                               
                    //1. Czy studia istnieja?
                    //com.CommandText = "select IdStudy from Studies where name=@name";
                    
                    
                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Studia nie istnieja");
                        //...
                    }
                    int idstudies = (int)dr["IdStudy"];

                   // return Ok(idstudies);
                    //com.Parameters.AddWithValue("idStud", idstudies);

                     com.CommandText = "SELECT IdEnrollment FROM Enrollment " +

                                           "WHERE IdStudy ="+idstudies+" AND Enrollment.Semester = 1 ORDER BY StartDate;";
                    

                    com.Parameters.AddWithValue("Index", request.IndexNumber);
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);

                    dr.Close();
                    dr = com.ExecuteReader();
                
                    int newIdEnrollment;
                    if (dr.Read())
                    {
                        newIdEnrollment = (int)dr["IdEnrollment"];

                       // return Ok(newIdEnrollment);
                        
                       // com.Parameters.AddWithValue("IdEnroll", dr["IdEnrollment"].ToString());

                      //  com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName,BirthDate, IdEnrollment) VALUES(@Index, @Fname, @Lname,@BirthDate, @IdEnroll )";
                      //  com.ExecuteNonQuery();
                    }
                    else
                    {
                        com.CommandText = "SELECT max(IdEnrollment) as id FROM Enrollment;";
                        dr.Close();
                        dr = com.ExecuteReader();
                        dr.Read();

                        newIdEnrollment = ((int)dr["id"])+1;

                       // return Ok(newIdEnrollment);
                        com.CommandText = "INSERT INTO Enrollment(IdEnrollment,Semester,  IdStudy, StartDate) " +
                                          "VALUES("+newIdEnrollment + ", 1," + idstudies + ",'"+ DateTime.Now +"');";
                       
                        dr.Close();
                        com.ExecuteNonQuery();
                                 //   tran.Commit();
                    }
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName,BirthDate, IdEnrollment) " +
                            "VALUES(@Index, @Fname, @Lname, CONVERT(DATETIME,@BirthDate,104), "+newIdEnrollment+ " ) ";
                    dr.Close();
                    
                    com.ExecuteNonQuery();
                    //x.Dodanie studenta
                    //com.CommandText = "INSERT INTO Student(IndexNumber, FirstName) VALUES(@Index, @Fname)";
                    //com.Parameters.AddWithValue("index", request.IndexNumber);
                    //...


                    tran.Commit();


                   
                    com.CommandText = "SELECT * FROM Enrollment WHERE IdEnrollment = " + newIdEnrollment + " ;";
                    dr.Close();
                    dr = com.ExecuteReader();
                    dr.Read();

                    enrollment.IdEnrollment = newIdEnrollment;
                    enrollment.Semester = (int)dr["Semester"];
                    enrollment.StartDate = dr["StartDate"].ToString();
                    enrollment.Study = request.Studies;
                    



                }
                catch (SqlException exc)
                {
                   // tran.Rollback();

                    return Ok(exc.Message);
                    tran.Rollback();
                }
            }

            return Ok(enrollment);


        }

        [HttpPost("{promotions}")]
         public IActionResult PromoteStudents(PromotionStudentRequest request, string promotions)
        {
            if (promotions.Equals("promotions"))
            {
                
                int idStud;

                using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
                using (var com = new SqlCommand())
                {


                    com.Connection = con;
                    com.CommandText = "SELECT Studies.IdStudy FROm Studies WHERE Studies.Name = '"+ request.Studies+"';";

                    
                    con.Open();
                    var dr = com.ExecuteReader();

                    
                    if (dr.Read())
                    {
                        idStud = (int)dr["IdStudy"];
                    }
                    else
                        return NotFound("Not found >> studia");


                  

                    //  var tran = con.BeginTransaction("SampleTransaction");
                  //  com.Transaction = tran;

                    com.CommandText = "select IdEnrollment from Enrollment where IdStudy=" + idStud + " AND Semester= " + request.Semester + ";";
                    // con.Open();
                    //  dr = com.ExecuteReader();
                    dr.Close();
                   dr = com.ExecuteReader();
                    if (!dr.Read())
                        return NotFound("Not found >> enrollment");



                    con.Close();
                    dr.Close();
                }


                using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
                using (var com = new SqlCommand("Promotions",con))
                {
                    com.Connection = con;
                    con.Open();

                    try
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add("@Studies", SqlDbType.VarChar).Value = request.Studies;
                        com.Parameters.Add("@Semester", SqlDbType.Int).Value = request.Semester;
                        com.ExecuteNonQuery();

                        con.Close();
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest("sql error > " + ex.Message);
                    }
                }



                using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
                using (var com = new SqlCommand())
                {


                    com.Connection = con;
                    com.CommandText = "SELECT * FROM Enrollment WHERE Semester = " + (request.Semester + 1) + " AND IdStudy = " + idStud + " ORDER BY IdEnrollment;";


                    con.Open();
                    var dr = com.ExecuteReader();

                    var enrollment = new Enrollment();

                    if (dr.Read())
                    {
                        enrollment.IdEnrollment = (int)dr["IdEnrollment"];
                        enrollment.Semester = (int)dr["Semester"];
                        enrollment.Study = request.Studies;
                        enrollment.StartDate = dr["StartDate"].ToString();
                    }
                    else
                        return NotFound("not found new enrollment!");



                    return Ok(enrollment);

                }


                }
            else
                return BadRequest("Bad request!");

        }
    }
}