using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Interfaces;

public interface ISaleStore
{
    Task AddSale(Sale sale);

    Task<int> GetTotalSalesByDay(int unix_time);
    Task<int> GetTotalSalesByMonth(int unix_time);
    Task<int> GetTotalSalesByYear(int unix_time);

    Task<int> GetTotalCarsSoldByDay(int unix_time);
    Task<int> GetTotalCarsSoldByMonth(int unix_time);
    Task<int> GetTotalCarsSoldByYear(int unix_time);

    Task<int> GetTotalTestDriveByDay(int unix_time);
    Task<int> GetTotalTestDriveByMonth(int unix_time);
    Task<int> GetTotalTestDriveByYear(int unix_time);

    Task<int> GetTotalSales();
    Task<int> GetTotalCarsSold();
    Task<int> GetTotalTestDriveRequsted();
    Task<int> GetTotalCustomers();   
}