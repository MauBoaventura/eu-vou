using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EuVou.Data;
using MBLabs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using System.Data;
using EuVou.Areas.Identity.Data;

namespace EuVou.Models
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly EuVouContext _context;

        public TicketsController(EuVouContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            string userAutenticate = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            EuVouUser user = ThisUser();            
            if (user.IsADM)
            {
                //ViewBag.User = _context.Event.ToList();
                ViewBag.User = user;
                ViewBag.Events = _context.Event.ToList();

                return View(await _context.Ticket.ToListAsync());
            }
            else
            {
                ViewBag.User = user;
                ViewBag.Events = _context.Event.ToList();
                
                return View(_context.Ticket.Where(m => m.Id_Client == userAutenticate));
            }

        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            string userAutenticate = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Pesquisa na tabala de usuarios criados no Identity pelo ID e retorna o EuVouUser
            string strcon = "Server=(localdb)\\mssqllocaldb;Database=EuVou;Trusted_Connection=True;MultipleActiveResultSets=true";

            SqlConnection sqlConnection1 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM AspNetUsers";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            EuVouUser user = null;
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetString(0) == userAutenticate)
                    {
                        user = new EuVouUser { Id = reader["Id"].ToString(), UserName = reader["Username"].ToString(), Email = reader["Email"].ToString(), CPF = reader["CPF"].ToString(), Name = reader["Name"].ToString(), Phone = reader["Phone"].ToString(), IsADM = Convert.ToBoolean(reader["IsADM"].ToString()) };
                    }
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            // Aqui os dados são acessados através do objeto dataReader
            sqlConnection1.Close();


            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == ticket.Id_Event);
            if (@event == null)
            {
                return NotFound();
            }

            ViewBag.User = user;
            ViewBag.Event = @event;


            if (userAutenticate != ticket.Id_Client && !user.IsADM)
            {
                return Forbid();

            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> CreateAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            Ticket ticket = new Ticket();

            ticket.Id_Event = @event.Id;
            Random a = new Random();
            ticket.ticket_id = a.Next(1, 50000).ToString();

            ViewBag.Price = @event.Price;

            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Id_Client,Id_Event,ticket_id")] Ticket ticket_view)
        {
            Console.WriteLine("ID do view " + ticket_view.Id);
            Ticket ticket = new Ticket();
            ticket.Id_Client = ticket_view.Id_Client;
            ticket.Id_Event = ticket_view.Id_Event;
            ticket.ticket_id = ticket_view.ticket_id;
            Console.WriteLine("ID do criado " + ticket.Id);

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Id_Client,Id_Event,ticket_id")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
        private EuVouUser ThisUser()
        {
            string userAutenticate = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string strcon = "Server=(localdb)\\mssqllocaldb;Database=EuVou;Trusted_Connection=True;MultipleActiveResultSets=true";

            SqlConnection sqlConnection1 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM AspNetUsers";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            EuVouUser user = null;
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetString(0) == userAutenticate)
                    {
                        user = new EuVouUser { Id = reader["Id"].ToString(), UserName = reader["Username"].ToString(), Email = reader["Email"].ToString(), CPF = reader["CPF"].ToString(), Name = reader["Name"].ToString(), Phone = reader["Phone"].ToString(), IsADM = Convert.ToBoolean(reader["IsADM"].ToString()) };
                    }
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            // Aqui os dados são acessados através do objeto dataReader
            sqlConnection1.Close();

            return user;

        }
        private List<EuVouUser> AllUsers()
        {
            string userAutenticate = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string strcon = "Server=(localdb)\\mssqllocaldb;Database=EuVou;Trusted_Connection=True;MultipleActiveResultSets=true";

            SqlConnection sqlConnection1 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM AspNetUsers";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            List<EuVouUser> user = new List<EuVouUser>();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetString(0) == userAutenticate)
                    {
                        EuVouUser user_i = new EuVouUser { Id = reader["Id"].ToString(), UserName = reader["Username"].ToString(), Email = reader["Email"].ToString(), CPF = reader["CPF"].ToString(), Name = reader["Name"].ToString(), Phone = reader["Phone"].ToString(), IsADM = Convert.ToBoolean(reader["IsADM"].ToString()) };
                        user.Add(user_i);
                    }
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            // Aqui os dados são acessados através do objeto dataReader
            sqlConnection1.Close();

            return user;

        }
    }
}
