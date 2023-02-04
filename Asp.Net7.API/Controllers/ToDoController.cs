using Asp.Net7.API.Core;
using Asp.Net7.API.Data;
using Asp.Net7.API.DTOs.Incoming;
using Asp.Net7.API.DTOs.Outgoing;
using Asp.Net7.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Controllers
{
    [Route("[controller]")]
    [ApiController] // attribute ile post kısmında parametrenin request body'sinde bulunacağını en baştan default olarak gelir.
    public class ToDoController : ControllerBase
    {
        // tüm metotlarımızı uow'den çekeceğiz instance yaptık
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiDbContext _context;
        public ToDoController(IUnitOfWork unitOfWork, IMapper mapper, ApiDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var allTodos = await _unitOfWork.ToDos.All();
                var _toDoDto = _mapper.Map<IEnumerable<ToDoDto>>(allTodos); // Tüm ToDo listesini mapper kullanarak ToDoDto formatına çevirdik.
                return Ok(_toDoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            try
            {
                var todo = await _unitOfWork.ToDos.GetById(id);
                if (todo == null) return NotFound();
                return Ok(todo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDoForCreatedDto toDo)
        {
            try
            {
                if (toDo == null) return BadRequest();

                // Listeye eklenecek toDo modelimizin zorunlu alan kontrolünü yaptırarak uyarı mesajı döndük.
                if (toDo.Name == null || toDo.Category == null)
                {
                    return BadRequest("Name or category cannot be empty");
                }

                var _toDo = _mapper.Map<ToDo>(toDo); //automapper

                await _unitOfWork.ToDos.Add(_toDo);
                await _unitOfWork.CompleteAsync();
                return Created("Added to to-do list", _toDo); // Created result'ı In-Memory Database kullandığımız için gösterilemeyecektir.
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating To-Do");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            try
            {
                var toDo = await _unitOfWork.ToDos.GetById(id);

                if (toDo == null || id <= 0) return NotFound($"To-Do with Id = {id} not found");

                await _unitOfWork.ToDos.Delete(toDo);
                await _unitOfWork.CompleteAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateToDo(int id, ToDoForUpdatedDto toDo)
        {
            try
            {
                if (id != toDo.Id) return BadRequest("Todo Id mismatch");

                var existToDo = await _unitOfWork.ToDos.GetById(id);
                if (existToDo == null) return NotFound($"Todo {id} not found");

                var _toDo = _mapper.Map<ToDo>(toDo);
                await _unitOfWork.ToDos.Update(_toDo);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }


        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateToDoPatch(int id, JsonPatchDocument<ToDo> patchDocument)
        {
            try
            {
                if (patchDocument == null || id <= 0)
                    return BadRequest();
                var existToDo = await _unitOfWork.ToDos.GetById(id);
                if (existToDo == null)
                    return NotFound();

                var toDoDto = new ToDo
                {
                    Id = existToDo.Id,
                    Name = existToDo.Name,
                    Category = existToDo.Category,
                    PublishDate = existToDo.PublishDate,
                };

                patchDocument.ApplyTo(toDoDto, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest();

                existToDo.Name = toDoDto.Name;

                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error patching data");
            }
        }

        //Ek endpointler.

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAscendingToDo()
        {
            var todo = await _unitOfWork.ToDos.AscendingName();
            if (todo == null) return NotFound();
            return Ok(todo);
        }
    }
}
