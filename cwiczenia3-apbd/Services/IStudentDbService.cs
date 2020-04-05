using cwiczenia3_apbd.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia3_apbd.Services
{
    interface IStudentDbService
    {
        public void EnrollStudent(EnrollStudentRequest request);
        public void PromoteStudents(int semester, string studies);
    }
}
