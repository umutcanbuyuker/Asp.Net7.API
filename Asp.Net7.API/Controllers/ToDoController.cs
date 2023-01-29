using Asp.Net7.API.Core;
using Asp.Net7.API.Data;
using Asp.Net7.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ToDoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.ToDos.All());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var todo = Ok(await _unitOfWork.ToDos.GetById(id));

            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDo toDo)
        {
            await _unitOfWork.ToDos.Add(toDo);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var toDo = await _unitOfWork.ToDos.GetById(id);

            if (toDo == null) return NotFound();

            await _unitOfWork.ToDos.Delete(toDo);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateDriver(ToDo toDo)
        {
            var existToDo = await _unitOfWork.ToDos.GetById(toDo.Id);
            if (existToDo == null) return NotFound();

            await _unitOfWork.ToDos.Update(toDo);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
