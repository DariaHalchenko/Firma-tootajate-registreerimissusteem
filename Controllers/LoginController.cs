using Firma_tootajate_registreerimissusteem.Data;
using Firma_tootajate_registreerimissusteem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // Авторизация пользователя
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            // Ищем пользователя по email
            var kasutaja = _context.Registers.FirstOrDefault(u => u.Email == login.Email);
            if (kasutaja == null)
            {
                return Unauthorized("Vale e-posti aadress või parool.");
            }

            // Проверяем пароль через BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Parool, kasutaja.Parool);
            if (!isPasswordValid)
            {
                return Unauthorized("Vale e-posti aadress või parool.");
            }

            // Возвращаем успешный ответ
            return Ok(new
            {
                Message = "Sisselogimine õnnestus",
                Email = kasutaja.Email,
                Nimi = kasutaja.Nimi
            });
        }
    }
}
