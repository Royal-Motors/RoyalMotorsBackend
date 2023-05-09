using CarWebsiteBackend.Data;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarWebsiteBackend.Extensions;
using CarWebsiteBackend.DTOs;

namespace CarWebsiteBackend.Storage;

public class SaleStorage : ISaleStore
{
    private readonly DataContext _context;
    public SaleStorage(DataContext context)
    {
        _context = context;
    }

    public async Task AddSale(Sale sale)
    {
        _context.Add(sale);
        await _context.SaveChangesAsync();
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
        var time1 = startOfDay.ToUnixTimeSeconds();
        var time2 = endOfDay.ToUnixTimeSeconds();

        return await _context.Sales
            .Where(s => s.Time >= time1 && s.Time < time2)
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