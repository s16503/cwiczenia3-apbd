using cwiczenia3_apbd.Controllers;
using cwiczenia3_apbd.models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia3_apbd.Services
{
    public class SqlServerDbService : IStudentDbService
    {


        public SqlServerDbService(/*.. */ )
        {

        }

        [HttpPost]
        void IStudentDbService.EnrollStudent(EnrollStudentRequest request) //nowy student
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

                                          "WHERE IdStudy =" + idstudies + " AND Enrollment.Semester = 1 ORDER BY StartDate;";


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

                        newIdEnrollment = ((int)dr["id"]) + 1;

                        // return Ok(newIdEnrollment);
                        com.CommandText = "INSERT INTO Enrollment(IdEnrollment,Semester,  IdStudy, StartDate) " +
                                          "VALUES(" + newIdEnrollment + ", 1," + idstudies + ",'" + DateTime.Now + "');";

                        dr.Close();
                        com.ExecuteNonQuery();
                        //   tran.Commit();
                    }
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName,BirthDate, IdEnrollment) " +
                            "VALUES(@Index, @Fname, @Lname, CONVERT(DATETIME,@BirthDate,104), " + newIdEnrollment + " ) ";
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

        public void PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }

       
    }
}
