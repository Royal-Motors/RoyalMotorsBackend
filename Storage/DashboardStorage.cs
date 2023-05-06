using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CarWebsiteBackend.Storage
{
    public class DashboardStorage : IDashboardInterface
    {
        private readonly DataContext _context;
        public DashboardStorage(DataContext context)
        {
            _context = context;
        }

        public Task<int> GetTotalCarsSold()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCarsSoldByDay(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCarsSoldByMonth(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCarsSoldByYear(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalSales()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalSalesByDay(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalSalesByMonth(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalSalesByYear(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalTestDriveByDay(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalTestDriveByMonth(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalTestDriveByYear(int unix_time)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalTestDriveRequsted()
        {
            throw new NotImplementedException();
        }
    }
}


