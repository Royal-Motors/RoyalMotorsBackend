using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

using CarWebsiteBackend.Exceptions.ProfileExceptions;
namespace CarWebsiteBackend.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ISaleStore saleStore;
        private readonly CarInterface carStore;
        private readonly ITestDriveInterface testdriveInterface;
        private readonly IAccountInterface accountStore;


        public DashboardController(ISaleStore saleStore, CarInterface carStore, ITestDriveInterface testdriveInterface, IAccountInterface accountStore)
        {
            this.saleStore = saleStore;
            this.carStore = carStore;
            this.testdriveInterface = testdriveInterface;
            this.accountStore = accountStore;
        }

        [HttpPost]
        public async Task<ActionResult> AddSale(Sale sale)
        {
            try
            {
                await accountStore.GetAccount(sale.Email);
                await carStore.GetCar(sale.CarName);
                await saleStore.AddSale(sale);
                return CreatedAtAction(nameof(AddSale), new { }, sale);
            }
            catch (Exception ex)
            {
                if(ex is ProfileNotFoundException)
                {
                    return NotFound($"Account with email {sale.Email} not found");
                }
                else if(ex is CarNotFoundException)
                {
                    return NotFound($"Car {sale.CarName} not found");
                }
                throw;
            }
        }

        [HttpGet("sales/day/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalSalesByDay(int unixTime)
        {
            try
            {
                int totalSalesByDay = await saleStore.GetTotalSalesByDay(unixTime);
                return Ok(totalSalesByDay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("sales/month/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalSalesByMonth(int unixTime)
        {
            try
            {
                int totalSalesByMonth = await saleStore.GetTotalSalesByMonth(unixTime);
                return Ok(totalSalesByMonth);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("sales/year/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalSalesByYear(int unixTime)
        {
            try
            {
                int totalSalesByYear = await saleStore.GetTotalSalesByYear(unixTime);
                return Ok(totalSalesByYear);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("cars/day/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalCarsSoldByDay(int unixTime)
        {
            try
            {
                int totalCarsSoldByDay = await saleStore.GetTotalCarsSoldByDay(unixTime);
                return Ok(totalCarsSoldByDay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("cars/month/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalCarsSoldByMonth(int unixTime)
        {
            try
            {
                int totalCarsSoldByMonth = await saleStore.GetTotalCarsSoldByMonth(unixTime);
                return Ok(totalCarsSoldByMonth);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("cars/year/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalCarsSoldByYear(int unixTime)
        {
            try
            {
                int totalCarsSoldByYear = await saleStore.GetTotalCarsSoldByYear(unixTime);
                return Ok(totalCarsSoldByYear);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("testdrive/day/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalTestDriveByDay(int unixTime)
        {
            try
            {
                int totalTestDrivesByDay = await saleStore.GetTotalTestDriveByDay(unixTime);
                return Ok(totalTestDrivesByDay);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("testdrive/month/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalTestDriveByMonth(int unixTime)
        {
            try
            {
                int totalTestDrivesByMonth = await saleStore.GetTotalTestDriveByMonth(unixTime);
                return Ok(totalTestDrivesByMonth);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("testdrive/year/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalTestDriveByYear(int unixTime)
        {
            try
            {
                int totalTestDrivesByYear = await saleStore.GetTotalTestDriveByYear(unixTime);
                return Ok(totalTestDrivesByYear);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("totalsales")]
        public async Task<IActionResult> GetTotalSales()
        {
            try
            {
                int totalSales = await saleStore.GetTotalSales();
                return Ok(totalSales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("totalcarssold")]
        public async Task<IActionResult> GetTotalCarsSold()
        {
            try
            {
                int totalCarsSold = await saleStore.GetTotalCarsSold();
                return Ok(totalCarsSold);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("totaltestdriverequested")]
        public async Task<IActionResult> GetTotalTestDriveRequsted()
        {
            try
            {
                int totalTestDriveRequested = await saleStore.GetTotalTestDriveRequsted();
                return Ok(totalTestDriveRequested);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("totalcustomers")]
        public async Task<IActionResult> GetTotalCustomers()
        {
            try
            {
                int totalCustomers = await saleStore.GetTotalCustomers();
                return Ok(totalCustomers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}