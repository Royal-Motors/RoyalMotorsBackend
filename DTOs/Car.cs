using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebsiteBackend.DTOs
{
    public record Car
    {
        public Car([Required] string name, [Required] string make, [Required] string model, [Required] int year,
            [Required] string color, [Required] bool used, [Required] int price, [Optional] string description,
            [Required] int mileage, [Optional] string image_id_list, [Optional] string video_id)
        {
            this.name = name;
            this.make = make;
            this.model = model;
            this.year = year;
            this.color = color;
            this.used = used;                    //(boolean, required)
            this.price = price;                  //(integer, required)
            this.description = description;      //(string, optional)
            this.mileage = mileage;              //(integer, required for used cars)
            this.image_id_list = image_id_list;  //(string, optional)
            this.video_id = video_id;            //(string, optional)
        }

        public int Id { get; set; }
        public string name { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string color { get; set; }
        public bool used { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public int mileage { get; set; }
        public string image_id_list { get; set; }
        public string video_id { get; set; }

        public ICollection<TestDrive> ?TestDrives { get; set; }

    }
}
