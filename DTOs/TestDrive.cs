using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace CarWebsiteBackend.DTOs
{
    public record TestDrive
    {
        public TestDrive([Required] string test_drive_id, [Required] string car_id, [Required] string customer_id,
                         [Required] string time_start, [Required] string time_end)
        {
            this.test_drive_id = test_drive_id;
            this.car_id = car_id;                 //the unique identifier of the car being test driven. 
            this.customer_id = customer_id;        //the unique identifier of the customer scheduling the test drive. 
            this.time_start = time_start;          //the start time of the test drive in ISO format (e.g. "2023-03-01T10:00:00Z") 
            this.time_end = time_end;             //the end time of the test drive in ISO format. 
        }
        public string test_drive_id { get; set; }
        public string car_id { get; set; }
        public string customer_id { get; set; }
        public string time_start { get; set; }
        public string time_end { get; set; }
    }
}