using System;
using System.ComponentModel.DataAnnotations;

namespace MBLabs.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Id_Client { get; set; }
        public int Id_Event { get; set; }
        public string ticket_id { get; set; }
    }
}
