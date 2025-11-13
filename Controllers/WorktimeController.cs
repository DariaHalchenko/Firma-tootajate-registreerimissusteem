using Firma_tootajate_registreerimissusteem.Data;
using Firma_tootajate_registreerimissusteem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma_tootajate_registreerimissusteem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorktimeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorktimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Worktime>> GetPaevaGraafiks()
        {
            // Tagastame kõik Worktime
            return _context.Worktimes.ToList();
        }

        // 1. Получить рабочее время сотрудника по имени
        [HttpGet("{nimi}")]
        public IActionResult GetByName(string nimi)
        {
            var user = _context.Registers
                .Include(u => u.Worktime)
                .FirstOrDefault(u => u.Nimi == nimi);

            if (user == null)
                return NotFound("Сотрудник не найден.");

            var result = new
            {
                Nimi = user.Nimi,
                Email = user.Email,
                Worktime = user.Worktime.Select(w => new
                {
                    Nadalapaev = w.Nadalapaev.ToString(), 
                    w.ToopaevaAlgus,
                    w.ToopaevaLopp
                }).ToList()
            };

            return Ok(result);
        }

        // 2. Массовое добавление рабочего времени
        [HttpPost("save-many")]
        public IActionResult SaveMany([FromBody] List<Worktime> worktimes)
        {
            foreach (var w in worktimes)
            {
                // Проверка обязательных полей
                if (w.RegisterId == 0)
                    return BadRequest("KasutajaId обязательно для каждой записи Worktime.");

                var user = _context.Registers.Find(w.RegisterId);
                if (user == null)
                    return BadRequest($"Пользователь с Id={w.RegisterId} не найден.");

                if (!Enum.IsDefined(typeof(DayOfWeek), w.Nadalapaev))
                    return BadRequest($"Неверный день недели: {w.Nadalapaev}");

                // Проверка корректности времени
                if (!TimeSpan.TryParse(w.ToopaevaAlgus, out _) || !TimeSpan.TryParse(w.ToopaevaLopp, out _))
                    return BadRequest($"Неверное время начала или конца: {w.ToopaevaAlgus} - {w.ToopaevaLopp}");
            }

            _context.Worktimes.AddRange(worktimes);
            _context.SaveChanges();

            return Ok(worktimes);
        }

        // 3. Обновление рабочего времени
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] Worktime updated)
        {
            var work = _context.Worktimes.Find(id);
            if (work == null)
                return NotFound("Запись не найдена.");

            work.ToopaevaAlgus = updated.ToopaevaAlgus ?? work.ToopaevaAlgus;
            work.ToopaevaLopp = updated.ToopaevaLopp ?? work.ToopaevaLopp;
            work.Nadalapaev = updated.Nadalapaev;

            _context.Worktimes.Update(work);
            _context.SaveChanges();

            return Ok(work);
        }

        // 4. Удаление рабочего времени
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var work = _context.Worktimes.Find(id);
            if (work == null)
                return NotFound("Запись не найдена.");

            _context.Worktimes.Remove(work);
            _context.SaveChanges();

            return Ok(new { Message = "Запись удалена." });
        }
    }
}
