using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;


namespace WebApi.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        static HttpClient client = new HttpClient();
        Customer[] customers = new Customer[]
        {
            new Customer{ Id=1, Firstname="Peter", Lastname = "Ivanov"},
            new Customer{ Id=2, Firstname="Svetlana", Lastname = "Petrova"},
            new Customer{ Id=3, Firstname="Denis", Lastname = "Morozov"},
            new Customer{ Id=4, Firstname="Igor", Lastname = "Volkov"}
        };

        [HttpGet("{id:long}")]
        public IActionResult GetCustomerAsync([FromRoute] long id)
        {
            var customer = customers.FirstOrDefault((c) => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);          
        }

        [HttpPost("")]
        public IActionResult CreateCustomerAsync([FromBody] Customer customer)
        {                      
                var newCustomer = new Customer { Id = customer.Id, Firstname = customer.Firstname, Lastname = customer.Lastname };
                var hasCustomer = customers.FirstOrDefault((c) => c.Id == customer.Id);

                if (hasCustomer == null)
                {
                    customers = customers.Append<Customer>(newCustomer).ToArray();
                    var customer_new = customers.FirstOrDefault((c) => c.Id == customer.Id);
                    return Ok(customer_new);

                } else
                {
                    return StatusCode((int)HttpStatusCode.Conflict);
                }              
        }      
    }
}