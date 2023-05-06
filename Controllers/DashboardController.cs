using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Collections;
using CarWebsiteBackend.Storage;

namespace CarWebsiteBackend.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardInterface dashbaordInterface;
        private readonly CarInterface carInterface;
        private readonly ITestDriveInterface testdriveInterface;
        private readonly IAccountInterface accountStore;


        public DashboardController(IDashboardInterface dashbaordInterface, CarInterface carInterface, ITestDriveInterface testdriveInterface, IAccountInterface accountStore)
        {
            this.dashbaordInterface = dashbaordInterface;
            this.carInterface = carInterface;
            this.testdriveInterface = testdriveInterface;
            this.accountStore = accountStore;
        }

        [HttpGet("sales/day/{unixTime}")]
        public async Task<ActionResult<int>> GetTotalSalesByDay(int unixTime)
        {
            try
            {
                int totalSalesByDay = await dashbaordInterface.GetTotalSalesByDay(unixTime);
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
                int totalSalesByMonth = await dashbaordInterface.GetTotalSalesByMonth(unixTime);
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
                int totalSalesByYear = await dashbaordInterface.GetTotalSalesByYear(unixTime);
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
                int totalCarsSoldByDay = await dashbaordInterface.GetTotalCarsSoldByDay(unixTime);
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
                int totalCarsSoldByMonth = await dashbaordInterface.GetTotalCarsSoldByMonth(unixTime);
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
                int totalCarsSoldByYear = await dashbaordInterface.GetTotalCarsSoldByYear(unixTime);
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
                int totalTestDrivesByDay = await dashbaordInterface.GetTotalTestDriveByDay(unixTime);
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
                int totalTestDrivesByMonth = await dashbaordInterface.GetTotalTestDriveByMonth(unixTime);
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
                int totalTestDrivesByYear = await dashbaordInterface.GetTotalTestDriveByYear(unixTime);
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
                int totalSales = await dashbaordInterface.GetTotalSales();
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
                int totalCarsSold = await dashbaordInterface.GetTotalCarsSold();
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
                int totalTestDriveRequested = await dashbaordInterface.GetTotalTestDriveRequsted();
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
                int totalCustomers = await dashbaordInterface.GetTotalCustomers();
                return Ok(totalCustomers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}