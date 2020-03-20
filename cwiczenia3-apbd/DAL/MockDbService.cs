using cwiczenia3_apbd.models;
using System;
using System.Collections.Generic;

namespace cwiczenia3_apbd.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;


        static MockDbService()
        {
            _students = new List<Student>
                        {
                            new Student{IdStudent=1, FirstName="Jan", LastName  ="Kowalski" },
                            new Student{IdStudent=2, FirstName="Błążej", LastName  ="Lol" },
                            new Student{IdStudent=3, FirstName="Andrzej", LastName  ="Nowak" },
                        };


        }


        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        public int DeleteStud(int id)
        { 
               foreach(Student st in _students)
            {
                if(st.IdStudent == id)
                {
                    ((List<Student>)_students).Remove(st);
                    return 0;
                }
            }

            return -1;  // 0 udało się , -1 nie 
        }
    }
}
