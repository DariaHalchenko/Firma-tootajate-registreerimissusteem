using Firma_tootajate_registreerimissusteem.Data;
using Firma_tootajate_registreerimissusteem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma_tootajate_registreerimissusteem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Авторизация пользователя с возвратом рабочего времени
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            var kasutaja = _context.Registers
                .Include(u => u.Worktime)
                .FirstOrDefault(u => u.Email == login.Email);

            if (kasutaja == null)
                return Unauthorized("Vale e-posti aadress või parool.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Parool, kasutaja.Parool);
            if (!isPasswordValid)
                return Unauthorized("Vale e-posti aadress või parool.");

            // Преобразуем DayOfWeek в строку
            var worktimes = kasutaja.Worktime.Select(w => new
            {
                Nadalapaev = w.Nadalapaev.ToString(), 
                w.ToopaevaAlgus,
                w.ToopaevaLopp
            }).ToList();

            return Ok(new
            {
                Message = "Sisselogimine õnnestus",
                Email = kasutaja.Email,
                Nimi = kasutaja.Nimi,
                Worktime = worktimes
            });
        }
    }
}
