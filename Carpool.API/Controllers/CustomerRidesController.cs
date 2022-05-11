using AutoMapper;
using Carpool.API.Models;
using Carpool.API.Services;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Carpool.API.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class CustomerRidesController : ControllerBase
    {
        private IMapper mapper;
        private IConfiguration configuration;
        private IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "murVwoNymAqjv27FwgPgzjz7mPINOv2dk1r8vjtF",
            BasePath = "https://carpooltask-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        private IFirebaseClient client;
        public CustomerRidesController(IMapper _mapper, IConfiguration _configuration)
        {
            mapper = _mapper;
            configuration = _configuration;
            client = new FirebaseClient(config);
        }

        [HttpGet]
        public IActionResult GetRides(string customerId)
        {
            try
            {
                var rides = client.Get("CustomerRides");
                dynamic result = JsonConvert.DeserializeObject(rides.Body);
                var list = new List<CustomerRides>();
                foreach (var item in result)
                {
                    CustomerRides temp = JsonConvert.DeserializeObject<CustomerRides>(((JProperty)item).Value.ToString());
                    if(temp.CustomerId == customerId)
                    {
                        list.Add(temp);
                    }
                }
                return Ok(list);
            }
            catch(Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public IActionResult NewCustomerRide(CustomerRides customerRide)
        {
            try
            {
                client.Push("CustomerRides", customerRide);
                return Ok(customerRide);
            }
            catch(Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}