using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SudokuApi.Context;
using SudokuApi.Models;

namespace SudokuApi.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SudokuController : ControllerBase
    {
        private readonly ISudokuService _sudokuService;

        public SudokuController(ISudokuService sudokuService)
        {
            _sudokuService = sudokuService;
        }

        // GET: api/sudoku
        [HttpGet ("Ver todos los sudokus")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<IEnumerable<Sudokus>>> GetAllSudokus()
        {
            return Ok(_sudokuService.GetAll());
        }
        

        // GET: api/sudoku/{id}
        [HttpGet("Ver sudoku por {id}")]
        public async Task<ActionResult<Sudokus>> GetById(int id)
        {
            Sudokus s = _sudokuService.GetById(id);
            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }
    }

}

