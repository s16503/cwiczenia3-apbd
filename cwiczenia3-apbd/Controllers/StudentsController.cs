using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cwiczenia3_apbd.models;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia3_apbd.Controllers
{

    [ApiController]     //definiuje zestaw standardowych cech dla api ,, walidować modele
    [Route("api/students")] 
    public class mojKontroller1Controller : ControllerBase
    {

        //2 q
        [HttpGet]       // odpowiada na żądanie GET
        public string GetStudents(string orderBy) //action methos
        {
            //var str = string.Format("Ala ma kota{0}",orderBy);
            // to samo co
            return $"Jan, Anna, Katarzyna sortowanie ={orderBy}";
        }




        //1 spososób przekazywania danych
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id) //zwraca rezultat z metody action. ...
        {
            
            if (id == 1)
                return Ok("Jan");
            else if(id ==2)
                return Ok("Andrzej");

            return NotFound("not found");
        }


        //3 przekazanie dancyh w cile żądania POST
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {

            student.Index = $"s{new Random().Next(1, 2000)}";
            return Ok(student);
        }


    }
}