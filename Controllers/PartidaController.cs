using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SudokuApi.Context;
using SudokuApi.Models;

namespace SudokuApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly SudokuDbContext _context;
        private readonly IPartidaService _partidaService;

        public PartidaController(SudokuDbContext context, IPartidaService partidaService)
        {
            _context = context;
            _partidaService = partidaService;
        }



        // POST: api/partida/crear
        [HttpPost("crear")]
        [Authorize]  // Asegúrate de que el usuario esté autenticado
        public async Task<ActionResult<Partida>> CrearPartida()
        {
            // Obtener el UserId del usuario logueado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Usuario no autenticado.");
            }
            
            Partida p = _partidaService.Create(userId);

            if (p == null)
            {
                return NotFound("No hay Sudokus disponibles en la base de datos.");
            }

            //Partida p = _partidaService.Create(userId, listaSudokus);
            return Ok(new
            {
                p.Id,
                p.SudokuID,
                p.TiempoInicio,
                p.EstadoTablero,
                p.TiempoResolucion,
                p.UserId
            });
        }




        // POST: api/partida/{id}/guardarEstado
        [HttpPost("{id}/guardarEstado")]
        [Authorize] 
        public async Task<ActionResult<Partida>> GuardarEstado(int id, [FromBody] string estadoTablero)
        {
            var partida = await _context.Partida.FindAsync(id);
            if (partida == null || partida.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound("Partida no encontrada.");
            }
            if (partida.TiempoInicio == null)
            {
                return BadRequest("La partida no ha sido iniciada o está pausada.");
            }
            
            Partida p = _partidaService.SaveState(partida, estadoTablero);


            // return Ok(p);
            return Ok("Estado de la partida guardado correctamente.");
        }

        
        // POST: api/partida/{id}/validarSolucion
        [HttpPost("{id}/validarSolucion")]
        [Authorize] 
        public async Task<ActionResult<bool>> ValidarSolucionx(int id)
        {
            var partida = await _context.Partida.FindAsync(id);
            if (partida == null || partida.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound("Partida no encontrada.");
            }

            string validacion = _partidaService.ValidateSolution(id);
            switch (validacion)
            {
                case "1": return NotFound("sudoku no encontrado");
                case "2": return Ok($"Solución correcta. \n Tiempo total utilizado: {partida.TiempoResolucion}");
                default: return Ok(_partidaService.ValidateIndividual(id));;
            }
        }


        // POST: api/partida/{id}/reanudar
        [HttpPost("{id}/reanudar")]
        [Authorize] 
        public async Task<ActionResult<Partida>> ReanudarPartida(int id)
        {
            var partida = await _context.Partida.FindAsync(id);
            if (partida == null || partida.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound("Partida no encontrada.");
            }

            Partida p = _partidaService.Resume(partida);
            // return Ok(p); 
            return Ok("Partida reanudada correctamente.");
        }

        //  // GET: api/partida/misPartidas
        [HttpGet("misPartidas")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Partida>>> GetMisPartidas()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Usuario no autenticado.");
            }
            List<Partida> partidas = _partidaService.GetAll(userId);
            return Ok(partidas);
        }

    }
}