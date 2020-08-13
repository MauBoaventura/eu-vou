using System;
using System.ComponentModel.DataAnnotations;

namespace MBLabs.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hour { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
    }
}