using Firma_tootajate_registreerimissusteem.Data;
using Firma_tootajate_registreerimissusteem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Firma_tootajate_registreerimissusteem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Регистрация нового пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            // Проверка, есть ли пользователь с таким email и именем
            var olemasolevEmail = _context.Registers.FirstOrDefault(u => u.Email == register.Email);
            var olemasolevNimi = _context.Registers.FirstOrDefault(u => u.Nimi == register.Nimi);

            if (olemasolevEmail != null && olemasolevNimi != null)
            {
                return BadRequest("Kasutaja sellise e-posti aadressi ja nimega juba eksisteerib.");
            }
            else if (olemasolevEmail != null)
            {
                return BadRequest("Kasutaja sellise e-posti aadressiga juba eksisteerib.");
            }
            else if (olemasolevNimi != null)
            {
                return BadRequest("Kasutaja sellise nimega juba eksisteerib.");
            }

            // Хешируем пароль
            string hashedParool = BCrypt.Net.BCrypt.HashPassword(register.Parool);

            var uuskasutaja = new Register
            {
                Nimi = register.Nimi,
                Email = register.Email,
                Parool = hashedParool
            };

            _context.Registers.Add(uuskasutaja);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Kasutaja on edukalt registreeritud",
                Id = uuskasutaja.Id,
                Email = uuskasutaja.Email,
                Nimi = uuskasutaja.Nimi
            });
        }
    }
}