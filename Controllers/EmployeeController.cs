using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace WebAPIDemo.Controllers
{
    //[EnableCors("*","*","*")]
    public class EmployeeController : ApiController
    {
        // GET api/<controller>
        //[DisableCors]
        [Authorize]
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }

        }

        // GET api/<controller>/5
        //[HttpGet]
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault<Employee>(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "the employee with " + id + " can not be found");
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using(EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges(); //in the mean time, the employee object will be updated with the newly created employee's id in database.
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());

                    return message;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromUri] Employee employee)
        {
            try
            {
                using(EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entity = employeeDBEntities.Employees.FirstOrDefault(e => e.ID == id);
                    if(entity != null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        employeeDBEntities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id + " is not found");
                    }
                }



            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using(EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entity = employeeDBEntities.Employees.FirstOrDefault(e => e.ID == id);
                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id=" + id + " is not found");
                    }
                    else
                    {
                        employeeDBEntities.Employees.Remove(entity);
                        employeeDBEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}