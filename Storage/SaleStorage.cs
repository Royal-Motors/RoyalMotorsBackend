using CarWebsiteBackend.Data;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        var startOfDay = unix_time;
        var endOfDay = unix_time + 86400;
        return await _context.Sales
            .Where(s => s.Time >= startOfDay && s.Time < endOfDay)
            .CountAsync();
    }

    public async Task<int> GetTotalCarsSoldByMonth(int unix_time)
    {
        var startOfMonth = unix_time;
        var endOfMonth = unix_time + 2.628e+6;
        return await _context.Sales
            .Where(s => s.Time >= startOfMonth && s.Time < endOfMonth)
            .CountAsync();
    }

    public async Task<int> GetTotalCarsSoldByYear(int unix_time)
    {
        var startOfYear = unix_time;
        var endOfYear = unix_time + 3.154e+7;
        return await _context.Sales
            .Where(s => s.Time >= startOfYear && s.Time < endOfYear)
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
        var startOfDay = unix_time;
        var endOfDay = unix_time + 86400;
        return await _context.Sales
            .Where(s => s.Time >= startOfDay && s.Time < endOfDay)
            .SumAsync(s => s.Price);
    }

    public async Task<int> GetTotalSalesByMonth(int unix_time)
    {
        var startOfMonth = unix_time;
        var endOfMonth = unix_time + 2.628e+6;
        return await _context.Sales
            .Where(s => s.Time >= startOfMonth && s.Time < endOfMonth)
            .SumAsync(s => s.Price);
    }

    public async Task<int> GetTotalSalesByYear(int unix_time)
    {
        var startOfYear = unix_time;
        var endOfYear = unix_time + 3.154e+7;
        return await _context.Sales
            .Where(s => s.Time >= startOfYear && s.Time < endOfYear)
            .SumAsync(s => s.Price);
    }

    public async Task<int> GetTotalTestDriveByDay(int unix_time)
    {
        var startOfDay = unix_time;
        var endOfDay = unix_time + 86400;
        return await _context.TestDrives
            .Where(s => s.Time >= startOfDay && s.Time < endOfDay)
            .CountAsync();
    }

    public async Task<int> GetTotalTestDriveByMonth(int unix_time)
    {
        var startOfMonth = unix_time;
        var endOfMonth = unix_time + 2.628e+6;
        return await _context.TestDrives
            .Where(s => s.Time >= startOfMonth && s.Time < endOfMonth)
            .CountAsync();
    }

    public async Task<int> GetTotalTestDriveByYear(int unix_time)
    {
        var startOfYear = unix_time;
        var endOfYear = unix_time + 3.154e+7;
        return await _context.TestDrives
            .Where(s => s.Time >= startOfYear && s.Time < endOfYear)
            .CountAsync();
    }

    public async Task<int> GetTotalTestDriveRequsted()
    {
        return await _context.TestDrives.CountAsync();
    }
}