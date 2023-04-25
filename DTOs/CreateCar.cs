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
    public record CreateCar
    {
        public CreateCar([Required] string name, [Required] string make, [Required] string model, [Required] int year,
            [Required] string color, [Required] bool used, [Required] int price, [Optional] string description,
            [Required] int mileage, [Required] int horsepower, [Required] float fuelconsumption, [Required] int fueltankcapacity, [Required] string transmissiontype, [Optional] string image_id_list, [Optional] string video_id)
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
            this.horsepower = horsepower;
            this.fuelconsumption = fuelconsumption;
            this.fueltankcapacity = fueltankcapacity;
            this.transmissiontype = transmissiontype;
            this.image_id_list = image_id_list;  //(string, optional)
            this.video_id = video_id;            //(string, optional)
        }
        public string name { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string color { get; set; }
        public bool used { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public int mileage { get; set; }
        public int horsepower { get; set; }
        public float fuelconsumption { get; set; }

        public int fueltankcapacity { get; set; }

        public string transmissiontype { get; set; }
        public string image_id_list { get; set; }
        public string video_id { get; set; }

    }
}
