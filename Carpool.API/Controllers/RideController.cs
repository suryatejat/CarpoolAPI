using AutoMapper;
using Carpool.API.DTO.RideDTO;
using Carpool.API.Models;
using Carpool.API.Services;
using Carpool.API.Services.Interfaces;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private IMapper mapper;
        private IConfiguration configuration;
        private IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "murVwoNymAqjv27FwgPgzjz7mPINOv2dk1r8vjtF",
            BasePath = "https://carpooltask-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        private IFirebaseClient client;

        public RideController(IMapper Mapper, IConfiguration Configuration)
        {
            mapper = Mapper;
            configuration = Configuration;
            client = new FirebaseClient(config);
        }

        [HttpGet]
        public IActionResult AllRides()
        {
            try
            {
                var rides = client.Get("Rides");
                dynamic result = JsonConvert.DeserializeObject(rides.Body);
                var list = new List<Ride>();
                foreach (var item in result)
                {
                    list.Add(JsonConvert.DeserializeObject<Ride>(((JProperty)item).Value.ToString()));
                }
                return Ok(list);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{rideId}")]
        public Ride GetARide(string rideId)
        {
            try
            {
                var rides = client.Get("Rides/"+rideId);
                dynamic result = JsonConvert.DeserializeObject(rides.Body);
                var list = new List<Ride>();
                foreach (var item in result)
                {
                    list.Add(JsonConvert.DeserializeObject<Ride>(((JProperty)item).Value.ToString()));
                }
                return list.SingleOrDefault(x => x.RideId == rideId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public Response AddNewRide(GetRideDTO rideDTO)
        {
            try
            {
                var newRide = mapper.Map<Ride>(rideDTO);
                newRide.RideId = Guid.NewGuid().ToString();
                client.Set("Rides/" + newRide.RideId, newRide);
                return new Response { Status = "Success", Message = "Added the new Ride" };
            }
            catch (Exception e)
            {
                return new Response { Status = "Failed", Message = e.Message };
            }
        }

        [HttpDelete]
        public IActionResult RemoveRide(string rideId)
        {
            try
            {
                client.Delete("Rides/" + rideId);
                return Ok("Deleted the Ride");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/AvailableRides")]
        public IActionResult AvailableRides(string source,string destination)
        {
            try
            {
                var rides = client.Get("Rides");
                dynamic result = JsonConvert.DeserializeObject(rides.Body);
                var list = new List<Ride>();
                foreach (var item in result)
                {
                    list.Add(JsonConvert.DeserializeObject<Ride>(((JProperty)item).Value.ToString()));
                }
                var availableRides = new List<Ride>();
                foreach(var item in list)
                {
                    if(item.Source==source && item.Destination == destination)
                    {
                        availableRides.Add(item);
                    }
                }
                return Ok(availableRides);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}