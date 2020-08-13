using System;
using System.ComponentModel.DataAnnotations;

namespace MBLabs.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isADM { get; set; }
    }
}