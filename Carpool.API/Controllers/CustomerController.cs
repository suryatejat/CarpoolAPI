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
using System.Linq;

namespace Carpool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IMapper mapper;
        private IConfiguration configuration;
        private HashingService hash = new HashingService();
        private IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "murVwoNymAqjv27FwgPgzjz7mPINOv2dk1r8vjtF",
            BasePath = "https://carpooltask-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        private IFirebaseClient client;

        public CustomerController(IMapper Mapper, IConfiguration Configuration)
        {
            mapper = Mapper;
            configuration = Configuration;
            client = new FirebaseClient(config);
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var customers = client.Get("Customers");
                dynamic result = JsonConvert.DeserializeObject(customers.Body);
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        } 

        [HttpGet("/Finder")]
        public Customer GetACustomer(string value)
        {
            try
            {
                var customers = client.Get("Customers");
                dynamic result = JsonConvert.DeserializeObject(customers.Body);
                var list = new List<Customer>();
                foreach (var item in result)
                {
                    list.Add(JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString()));
                }
                return list.SingleOrDefault(x => x.EmailId == value || x.CustomerId == value);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public Response AddNewCustomer(NewCustomerDTO customerDTO)
        {
            try
            {
                var newCustomer = mapper.Map<Customer>(customerDTO);
                var emailHash = hash.GetHash(newCustomer.EmailId);
                newCustomer.CustomerId = emailHash;
                client.Set("Customers/" + emailHash, newCustomer);
                return new Response { Status = "Success", Message = "Signed Up Successfully" };
            }
            catch(Exception e)
            {
                return new Response { Status = "Failed", Message = e.Message };
            }
        }

        [HttpGet("/Authenticate")]
        public Response Authenticate([FromQuery]NewCustomerDTO customerDTO)
        {
            try
            {
                var customer = mapper.Map<Customer>(customerDTO);
                var auth = GetACustomer(customerDTO.EmailId);
                if(auth!=null && auth.Password == customer.Password)
                {
                    return new Response { Status = "Success", Message = "Logged in Successfully" };
                }
                return new Response { Status = "Failed", Message = "Invalid Email / Password" };
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
                var emailHash = hash.GetHash(value);
                client.Delete("Customers/" + emailHash);
                return Ok("Deleted the Customer");
            }
            catch
            {
                return BadRequest("Invalid Email Id");
            }
        }
    }
}