using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using System;


namespace CarWebsiteBackend.Storage

{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }
    }
    public class DashboardStorage : IDashboardInterface
    {
        private readonly DataContext _context;
        public DashboardStorage(DataContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalCarsSold()
        {
            return await _context.Sales.CountAsync();
        }

        public async Task<int> GetTotalCarsSoldByDay(int unix_time)
        {
            var startOfDay = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfDay = startOfDay.AddDays(1);
            return await _context.Sales
                .Where(s => s.Time >= startOfDay.ToUnixTimeSeconds() && s.Time < endOfDay.ToUnixTimeSeconds())
                .CountAsync();
        }

        public async Task<int> GetTotalCarsSoldByMonth(int unix_time)
        {
            var startOfMonth = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfMonth = startOfMonth.AddMonths(1);
            return await _context.Sales
                .Where(s => s.Time >= startOfMonth.ToUnixTimeSeconds() && s.Time < endOfMonth.ToUnixTimeSeconds())
                .CountAsync();
        }

        public async Task<int> GetTotalCarsSoldByYear(int unix_time)
        {
            var startOfYear = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfYear = startOfYear.AddYears(1);
            return await _context.Sales
                .Where(s => s.Time >= startOfYear.ToUnixTimeSeconds() && s.Time < endOfYear.ToUnixTimeSeconds())
                .CountAsync();
        }

        public async Task<int> GetTotalCustomers()
        {
            return await _context.Sales.Select(s => s.Email).Distinct().CountAsync();
        }

        public async Task<int> GetTotalSales()
        {
            return await _context.Sales.SumAsync(s => s.Price);
        }

        public async Task<int> GetTotalSalesByDay(int unix_time)
        {
            var startOfDay = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfDay = startOfDay.AddDays(1);
            return await _context.Sales
                .Where(s => s.Time >= startOfDay.ToUnixTimeSeconds() && s.Time < endOfDay.ToUnixTimeSeconds())
                .SumAsync(s => s.Price);
        }

        public async Task<int> GetTotalSalesByMonth(int unix_time)
        {
            var startOfMonth = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfMonth = startOfMonth.AddMonths(1);
            return await _context.Sales
                .Where(s => s.Time >= startOfMonth.ToUnixTimeSeconds() && s.Time < endOfMonth.ToUnixTimeSeconds())
                .SumAsync(s => s.Price);
        }

        public async Task<int> GetTotalSalesByYear(int unix_time)
        {
            var startOfYear = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfYear = startOfYear.AddYears(1);
            return await _context.Sales
                .Where(s => s.Time >= startOfYear.ToUnixTimeSeconds() && s.Time < endOfYear.ToUnixTimeSeconds())
                .SumAsync(s => s.Price);
        }

        public async Task<int> GetTotalTestDriveByDay(int unix_time)
        {
            var startOfDay = DateTimeOffset.FromUnixTimeSeconds(unix_time).Date;
            var endOfDay = startOfDay.AddDays(1);
            return await _context.TestDrives
                .Where(s => s.Time >= startOfDay.ToUnixTimeSeconds() && s.Time < endOfDay.ToUnixTimeSeconds())
                .CountAsync();
        }

        public async Task<int> GetTotalTestDriveByMonth(int unix_time)
        {
            var startOfMonth = new DateTime(unix_time, 1, 1);
            var endOfMonth = startOfMonth.AddMonths(1);
            return await _context.TestDrives
                .Where(s => s.Time >= startOfMonth.ToUnixTimeSeconds() && s.Time < endOfMonth.ToUnixTimeSeconds())
                .CountAsync();
        }

        public async Task<int> GetTotalTestDriveByYear(int unix_time)
        {
            var startOfYear = new DateTime(unix_time, 1, 1);
            var endOfYear = startOfYear.AddYears(1);
            return await _context.TestDrives
                .Where(s => s.Time >= startOfYear.ToUnixTimeSeconds() && s.Time < endOfYear.ToUnixTimeSeconds())
                .CountAsync();
        }

        public async Task<int> GetTotalTestDriveRequsted()
        {
            return await _context.TestDrives.CountAsync();
        }
    }
}


