using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia3_apbd.Controllers
{

    [ApiController]     //definiuje zestaw standardowych cech dla api ,, walidować modele
    [Route("api/students")]
    public class mojKontroller1Controller : ControllerBase
    {
        [HttpGet]       // odpowiada na żądanie GET
        public string GetStudent() //action methos
        {
            return "Jan, Anna, Katarzyna";
        }
    }
}