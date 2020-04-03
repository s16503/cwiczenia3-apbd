using cwiczenia3_apbd.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia3_apbd.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {

        public SqlServerStudentDbService(/*.. */ )
        {

        }

        public void EnrollStudent(EnrollStudentRequest request)
        {

           
            //DTOs - Data Transfer Objects
            //Request models
            //==mapowanie==
            //Modele biznesowe/encje (entity)
            //==mapowanie==
            //Response models

            var st = new Student();
            st.FirstName = request.FirstName;
            //...
            //...
            //Micro ORM object-relational mapping
            //problemami - impedance mismatch

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    //1. Czy studia istnieja?
                    com.Parameters.AddWithValue("name", request.Studies);
                    com.CommandText = "select IdStudies from studies where name=@name";
                   

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                       // return BadRequest("Studia nie istnieja");
                        //...
                    }
                    int idstudies = (int)dr["IdStudies"];

                    //x. Dodanie studenta
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName) VALUES(@Index, @Fname)";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    //...
                    com.ExecuteNonQuery();

                    tran.Commit();

                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
            }

        }

        public void PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }


    }
}
