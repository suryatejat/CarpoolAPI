using Carpool.API.Models;
using Carpool.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpool.API.Services
{
    public class RideServices : IRideServices
    {
        private CarpoolDbContext _DbContext;
        private IConfiguration configuration;
        public RideServices(CarpoolDbContext DbContext, IConfiguration configuration)
        {
            _DbContext = DbContext;
            this.configuration = configuration;
        }
        public List<Ride> GetAllRides()
        {
            try
            {
                return _DbContext.Rides.ToList();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Ride FindRide(string id)
        {
            try
            {
                var ride = _DbContext.Rides.SingleOrDefault(x => x.RideId == id);
                return ride;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Ride NewRide(Ride ride)
        {
            try
            {
                _DbContext.Rides.Add(ride);
                _DbContext.SaveChanges();
                return ride;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Ride RemoveRide(string id)
        {
            try
            {
                var ride = FindRide(id);
                if (ride == null)
                    throw new Exception("Invalid Ride Id");
                _DbContext.Rides.Remove(ride);
                _DbContext.SaveChanges();
                return ride;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Ride> AvailableRides(string source,string destination)
        {
            try
            {
                var rides = _DbContext.Rides.Where(x => x.Source == source && x.Destination == destination).ToList();
                return rides;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}