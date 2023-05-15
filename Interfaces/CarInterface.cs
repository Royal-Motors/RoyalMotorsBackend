using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface CarInterface
{
    Task AddCar(Car car);
    Task DeleteCar(string name);
    Task EditCar(Car car);
    Task<Car> GetCar(string name);
    Task<List<Car>> GetAllCars();
}
