using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class StudentsV1Controller : ApiController
    {
        List<StudentV1> students = new List<StudentV1>()
        {
            new StudentV1() { Id = 1, Name = "Tom"},
            new StudentV1() { Id = 2, Name = "Sam"},
            new StudentV1() { Id = 3, Name = "John"},
        };

        public IEnumerable<StudentV1> Get()
        {
            return students;
        }

        public StudentV1 Get(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }
    }
}