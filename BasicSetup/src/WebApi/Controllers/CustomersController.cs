using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using WebApi.Models;
using Microsoft.AspNet.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : Controller
    {
       
        // GET: api/values
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            var c1 = new Customer { FirstName = "paul", LastName = "van bladel" };
            var c2 = new Customer { FirstName = "isabelle", LastName = "Robesyn" };

            return new List<Customer> { c1, c2 };


        }
    }



}
