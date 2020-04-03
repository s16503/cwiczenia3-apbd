using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cwiczenia3_apbd.DAL;
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


        [HttpGet]
        public IActionResult test()
        {
            return Ok("balaallaahahhaa");
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request) //nowy student
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
            using (var com = new SqlCommand())
            {


                 com.Connection = con;
                com.CommandText = "select IdStudy from Studies where name=@name";
                con.Open();

                    var tran = con.BeginTransaction("SampleTransaction");
                    com.Transaction = tran;

                try
                {
                               
                    //1. Czy studia istnieja?
                    com.CommandText = "select IdStudy from Studies where name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);
                    
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
                            "VALUES(@Index, @Fname, @Lname,@BirthDate, "+newIdEnrollment+ " )";
                    dr.Close();
                    
                    com.ExecuteNonQuery();
                    //x.Dodanie studenta
                    //com.CommandText = "INSERT INTO Student(IndexNumber, FirstName) VALUES(@Index, @Fname)";
                    //com.Parameters.AddWithValue("index", request.IndexNumber);
                    //...


                    tran.Commit();

                }
                catch (SqlException exc)
                {
                   // tran.Rollback();

                    return Ok(exc.Message);
                    tran.Rollback();
                }
            }

            return Ok("KK");

















            //if(student.firstname.equals("")
            //    || student.lastname.equals("")
            //    || student.birthdate.equals("")
            //    || student.indexnumber.equals("")
            //    || student.studies.equals("")
            //  )
            //{
            //    return badrequest();
            //}

            //using (var con = new SqlConnection(ConString))
            //using (var com = new SqlCommand())
            //{
            //    com.Connection = con;
            //    com.CommandText = "SELECT  Count(*) as istn  FROM Enrollment " +
            //                      "JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy " +
            //                      "WHERE Studies.Name = '" + student.Studies + "';" ;

            //  //  com.Parameters.AddWithValue("index", index);
            //    con.Open();
            //    SqlDataReader dr = com.ExecuteReader();

            //    int count = 0;
            //    while (dr.Read())
            //    {
            //        count = (int)dr["istn"];         
            //    }

            //    if(count == 0)
            //        return BadRequest();

            //    int idWpisu = 0;
            //    com.CommandText = "SELECT IdEnrollment FROM Enrollment " +
            //                       "JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy " +
            //                       "WHERE Studies.Name = '" +student.Studies + "' AND Enrollment.Semester = 1;";

            //    dr = com.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        idWpisu = (int)dr["IdEnrollment"];
            //    }


            //}


            //return Ok("blahahalalalahahala!");


        }
    }
}