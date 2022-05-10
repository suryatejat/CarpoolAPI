using AutoMapper;
using Carpool.API.DTO.RideDTO;
using Carpool.API.Models;
using Carpool.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Carpool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private IRideServices RideServices;
        private IMapper mapper;
        private IConfiguration configuration;

        public RideController(IRideServices rideServices, IMapper Mapper, IConfiguration Configuration)
        {
            RideServices = rideServices;
            mapper = Mapper;
            configuration = Configuration;
        }

        [HttpGet]
        public IActionResult AllRides()
        {
            try
            {
                var rides = RideServices.GetAllRides();
                return Ok(rides);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{rideId}")]
        public IActionResult GetARide(string rideId)
        {
            try
            {
                var ride = RideServices.FindRide(rideId);
                if (ride == null)
                    throw new Exception("Invalid Ride Id");
                return Ok(ride);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddNewRide(GetRideDTO rideDTO)
        {
            try
            {
                var newRide = mapper.Map<Ride>(rideDTO);
                newRide.RideId = Guid.NewGuid().ToString();
                RideServices.NewRide(newRide);
                return Ok(newRide);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult RemoveRide(string rideId)
        {
            try
            {
                var ride = RideServices.RemoveRide(rideId);
                return Ok(ride);
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
                var availableRides = RideServices.AvailableRides(source,destination);
                return Ok(availableRides);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}