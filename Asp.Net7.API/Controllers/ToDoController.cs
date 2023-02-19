using Asp.Net7.API.Core;
using Asp.Net7.API.Data;
using Asp.Net7.API.DTOs;
using Asp.Net7.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
            var allTodos = await _unitOfWork.ToDos.All();
            var _toDoDto = _mapper.Map<IEnumerable<ToDoDto>>(allTodos); // Tüm ToDo listesini mapper kullanarak ToDoDto formatına çevirdik.
            return Ok(_toDoDto);
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var todo = await _unitOfWork.ToDos.GetById(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDoForCreatedDto toDo)
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

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var toDo = await _unitOfWork.ToDos.GetById(id);

            if (toDo == null || id <= 0) return NotFound($"To-Do with Id = {id} not found");

            await _unitOfWork.ToDos.Delete(toDo);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateToDo(int id, ToDoForUpdatedDto toDo)
        {
            if (id != toDo.Id) return BadRequest("Todo Id mismatch");

            var existToDo = await _unitOfWork.ToDos.GetById(id);
            if (existToDo == null) return NotFound($"Todo {id} not found");

            var _toDo = _mapper.Map<ToDo>(toDo);
            await _unitOfWork.ToDos.Update(_toDo);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }


        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateToDoPatch(int id, JsonPatchDocument<ToDo> patchDocument)
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

        //Ek endpointler.

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDescendingToDo()
        {
            var todo = await _unitOfWork.ToDos.AscendingName();
            if (todo == null) return NotFound();
            return Ok(todo);
        }
    }
}
