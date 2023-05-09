using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarWebsiteBackend.Exceptions.CarExceptions;
using Microsoft.Data.SqlClient;

namespace CarWebsiteBackend.Storage;

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
            await _context.SaveChangesAsync();
        }
        catch
        {
            throw new DuplicateCarException();
        } 
    }

    public async Task DeleteCar(string name)
    {
        var sql = "DELETE FROM Cars WHERE name = @Name";
        var parameters = new[] { new SqlParameter("@Name", name) };
        var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        if (rowsAffected <= 0)
        {
            throw new CarNotFoundException();
        }
    }

    public async Task SellCar(string name)
    {
        var sql = "DELETE FROM Cars WHERE name = @Name";
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

    public async Task<List<Car>> GetAllCars()
    {
        var cars = await _context.Cars.ToListAsync();
        if (cars == null || cars.Count == 0)
        {
            throw new CarNotFoundException();
        }
        return cars;
    }

    public async Task EditCar(Car car)
    {
        var sql = "UPDATE Cars SET make = @NewMake, model = @NewModel, year = @NewYear, color = @NewColor, price = @NewPrice, description = @NewDescription, mileage = @NewMileage, image_id_list = @NewImage, video_id = @NewVideo, fueltankcapacity = @Newfueltankcapacity, transmissiontype = @Newtransmissiontype, horsepower = @Newhorsepower , fuelconsumption = @Newfuelconsumption  WHERE name = @Name";
        var parameters = new[]
        {
        new SqlParameter("@Name", car.name),
        new SqlParameter("@NewMake", car.make),
        new SqlParameter("@NewModel", car.model),
        new SqlParameter("@NewYear", car.year),
        new SqlParameter("@NewColor", car.color),
        new SqlParameter("@NewPrice", car.price),
        new SqlParameter("@NewDescription", car.description),
        new SqlParameter("@NewMileage", car.mileage),
        new SqlParameter("@NewImage", car.image_id_list),
        new SqlParameter("@NewVideo", car.video_id),
        new SqlParameter("@Newfueltankcapacity", car.fueltankcapacity),
        new SqlParameter("@Newtransmissiontype", car.transmissiontype),
        new SqlParameter("@Newhorsepower", car.horsepower),
        new SqlParameter("@Newfuelconsumption", car.fuelconsumption)
        };
        var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        if(rowsAffected <= 0)
        {
            throw new CarNotFoundException();
        }
    }
}


