using cwiczenia3_apbd.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia3_apbd.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public int DeleteStud(int id);
    }
}
