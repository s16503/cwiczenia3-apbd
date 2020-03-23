using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using cwiczenia3_apbd.DAL;
using cwiczenia3_apbd.models;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia3_apbd.Controllers
{

    [ApiController]     //definiuje zestaw standardowych cech dla api ,, walidować modele
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;


        public StudentsController(IDbService db)
        {
            _dbService = db;
        }
        //2 q
        [HttpGet]       // odpowiada na żądanie GET
        public IActionResult GetStudents(string orderBy) //action methos
        {

            List<Student> resList = new List<Student>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16503;Integrated Security=True;"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM Student";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while(dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.Index = dr["IndexNumber"].ToString();
                    st.IdStudent = (int)dr["IdEnrollment"];

                    resList.Add(st);
                }

            }

                return Ok(resList);
        }




        ////1 spososób przekazywania danych
        //[HttpGet("{id}")]
        //public IActionResult GetStudent(int id) //zwraca rezultat z metody action. ...
        //{

        //    if (id == 1)
        //        return Ok("Jan");
        //    else if(id ==2)
        //        return Ok("Andrzej");

        //    return NotFound("not found");
        //}


        //3 przekazanie dancyh w cile żądania POST
        [HttpPost]
        public IActionResult CreateStudent(Student student) //nowy student
        {
            student.Index = $"s{new Random().Next(1, 20000)}";

            ((List<Student>)_dbService.GetStudents()).Add(student);

            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudnet(int id)  //aktualizacja
        {
            var list = _dbService.GetStudents();


            foreach (Student st in list)
            {
                if (st.IdStudent == id)
                {
                    st.Index = "s6666";
                    return Ok("Aktualizacja dokończona");
                }
            }
            // Console.WriteLine(st.FirstName);

            return NotFound("id not found");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)      //usuwanie
        { 
            foreach (Student st in _dbService.GetStudents())
            {
                if (st.IdStudent == id)
                {
                    ((List<Student>)_dbService.GetStudents()).Remove(st);
                    return Ok("Usuwanie ukończone");
                }
            }
            return NotFound("nie znaleziono studenta o id: " + id); 
        }
        
    }
}