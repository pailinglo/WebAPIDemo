using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeService.Controllers
{
    //comment this for testing purpose.
    //[Authorize]
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using(EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        [Route("api/employee/{id}",Name ="GetEmployeeById")]
        public Employee Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.ID == id);
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    //there are two ways to produce the location. use "Route name" is better.
                    //since in the other way, you are not sure if there is a "/" at the end of request url. and can cause problem.
                    //message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    message.Headers.Location = new Uri(Url.Link("GetEmployeeById", new { id = employee.ID }));

                    return message;
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
