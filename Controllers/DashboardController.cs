using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace CarWebsiteBackend.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ISaleStore saleStore;

        public DashboardController(ISaleStore saleStore)
        {
            this.saleStore = saleStore;
        }

        [HttpGet("sales/day/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalSalesByDay(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("sales/month/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalSalesByMonth(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("sales/year/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalSalesByYear(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("cars/day/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalCarsSoldByDay(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("cars/month/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalCarsSoldByMonth(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("cars/year/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalCarsSoldByYear(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("testdrive/day/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalTestDriveByDay(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("testdrive/month/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalTestDriveByMonth(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("testdrive/year/{unixTime}"), Authorize]
        public async Task<ActionResult<int>> GetTotalTestDriveByYear(int unixTime)
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("totalsales"), Authorize]
        public async Task<IActionResult> GetTotalSales()
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("totalcarssold"), Authorize]
        public async Task<IActionResult> GetTotalCarsSold()
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("totaltestdriverequested"), Authorize]
        public async Task<IActionResult> GetTotalTestDriveRequsted()
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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

        [HttpGet("totalcustomers"), Authorize]
        public async Task<IActionResult> GetTotalCustomers()
        {
            string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
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