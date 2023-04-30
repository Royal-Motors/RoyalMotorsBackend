using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface CarInterface
{
    Task AddCar(Car car);            // Used for adding a car, returns error if car already exists
    Task DeleteCar(string name);
    Task SellCar(string name);
    Task EditCar(Car car);           // Forces upsert, used for Edit car
    Task<Car> GetCar(string name);
    Task<List<Car>> GetAllCars();
}
