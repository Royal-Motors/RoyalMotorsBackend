using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarWebsiteBackend.Exceptions.CarExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace CarWebsiteBackend.Storage
{
    public class CarStorage : CarInterface
    {
        private readonly DataContext _context;
        public CarStorage(DataContext context)
        {
            _context = context;
        }

        public async Task AddCar(Car car)
        {
               try
            {
                _context.Add(car);
                var saved = await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DuplicateCarException();
            } 
        }

        public async Task DeleteCar(string name)
        {
            var sql = "DELETE FROM Cars WHERE model = @Name";
            var parameters = new[] { new SqlParameter("@Name", name) };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
            {
                throw new CarNotFoundException();
            }
        }

        public async Task<Car> GetCar(string name)
        {
            var Car = await _context.Cars.Where(p => p.name == name).FirstOrDefaultAsync();
            if (Car == null)
            {
                throw new CarNotFoundException();
            }
            return Car;
        }

        public async Task EditCar(Car car)
        {
            var sql = "UPDATE Cars SET name = @NewName, make = @NewMake, model = @NewModel, year = @NewYear, color = @NewColor, price = @NewPrice, description = @NewDescription, mileage = @NewMileage, image_id_list = @NewImage, video_id = @NewVideo WHERE email = @Email";
            var parameters = new[]
            {
            new SqlParameter("@NewName", car.name),
            new SqlParameter("@NewMake", car.make),
            new SqlParameter("@NewModel", car.model),
            new SqlParameter("@NewYear", car.year),
            new SqlParameter("@NewColor", car.color),
            new SqlParameter("@NewPrice", car.price),
            new SqlParameter("@NewDescription", car.description),
            new SqlParameter("@NewMileage", car.mileage),
            new SqlParameter("@NewImage", car.image_id_list),
            new SqlParameter("@NewVideo", car.video_id)
            };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if(rowsAffected <= 0)
            {
                throw new CarNotFoundException();
            }
        }
    }
}