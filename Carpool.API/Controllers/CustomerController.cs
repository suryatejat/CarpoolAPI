using AutoMapper;
using Carpool.API.DTO.CustomerDTO;
using Carpool.API.Models;
using Carpool.API.Services;
using Carpool.API.Services.Interfaces;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Carpool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerServices CustomerServices;
        private IMapper mapper;
        private IConfiguration configuration;
        private IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "murVwoNymAqjv27FwgPgzjz7mPINOv2dk1r8vjtF",
            BasePath = "https://carpooltask-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        private IFirebaseClient client;

        public CustomerController(ICustomerServices customerServices, IMapper Mapper, IConfiguration Configuration)
        {
            CustomerServices = customerServices;
            mapper = Mapper;
            configuration = Configuration;
            client = new FirebaseClient(config);
        }

        

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                /*var Customers = CustomerServices.AllCustomers();
                return Ok(Customers);*/
                var customers = client.Get("People");
                dynamic result = JsonConvert.DeserializeObject(customers.Body);
                var list = new List<Customer>();
                foreach(var item in result)
                {
                    list.Add(JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString()));
                }
                return Ok(list);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        } 

        [HttpPost]
        public IActionResult AddNewCustomer(NewCustomerDTO customerDTO)
        {
            try
            {
                var newCustomer = mapper.Map<Customer>(customerDTO);
                newCustomer.CustomerId = Guid.NewGuid().ToString();
                client.Push("People", newCustomer);
                return Ok(newCustomer);
                /*CustomerServices.AddCustomer(newCustomer);
                return new Response { Status = "Success", Message = "Signed Up Successfully" };*/
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
                /*return new Response { Status = "Failed", Message = e.Message };*/
            }
        }

        [HttpGet("/Authenticate")]
        public Response Authenticate([FromQuery]NewCustomerDTO customerDTO)
        {
            try
            {
                var customer = mapper.Map<Customer>(customerDTO);
                CustomerServices.AuthenticateCustomer(customer);
                return new Response { Status = "Success", Message = "Authenticated Successfully" };
            }
            catch(Exception e)
            {
                return new Response { Status = "Failed", Message = e.Message};
            }
        }

        [HttpDelete("{value}")]
        public IActionResult RemoveCustomer(string value)
        {
            try
            {
                var removedCustomer = CustomerServices.DeleteCustomer(value);
                return Ok(removedCustomer);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}