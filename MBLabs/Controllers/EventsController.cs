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
using Microsoft.Data.SqlClient;
using EuVou.Areas.Identity.Data;
using System.Data;
using System.Security.Claims;

namespace EuVou.Models
{
    public class EventsController : Controller
    {
        private readonly EuVouContext _context;

        public EventsController(EuVouContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            ViewBag.IsADM = ThisIsADM();
            return View(await _context.Event.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            if (ThisIsADM())
                return View();
            else
                return Forbid();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Hour,Date,Location,Price")] Event @event)
        {
            if (!ThisIsADM())
                return Forbid();
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(@event);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(@event);
            }

        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ThisIsADM())
                return Forbid();
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var @event = await _context.Event.FindAsync(id);
                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Hour,Date,Location,Price")] Event @event)
        {
            if (!ThisIsADM())
                return Forbid();
            else
            {

                if (id != @event.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(@event);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EventExists(@event.Id))
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
                return View(@event);
            }
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ThisIsADM())
                return Forbid();
            else
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

                return View(@event);
            }
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ThisIsADM())
                return Forbid();
            else
            {
                var @event = await _context.Event.FindAsync(id);
                _context.Event.Remove(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }

        private bool ThisIsADM()
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

            return user.IsADM;

        }
    }
}
