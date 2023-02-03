using Asp.Net7.API.Core;
using Asp.Net7.API.Data;
using Asp.Net7.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        // tüm komutlarımızı uow'den çekeceğiz instance yaptık
        private readonly IUnitOfWork _unitOfWork;
        public ToDoController(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _unitOfWork.ToDos.All());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
                var todo = await _unitOfWork.ToDos.GetById(id);
                if (todo == null) return NotFound();
                return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDo toDo)
        {
                // Listeye eklenecek toDo modelimizin zorunlu alan kontrolünü yaptırarak uyarı mesajı döndük.
                if (toDo.Name == null || toDo.Category == null)
                {
                    return BadRequest("isim veya kategori boş geçilemez");
                }
                await _unitOfWork.ToDos.Add(toDo);
                await _unitOfWork.CompleteAsync();
                return Created("Yapılacaklar listesine eklendi",toDo); // Created result'ı In-Memory Database kullandığımız için gösterilemeyecektir.
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var toDo = await _unitOfWork.ToDos.GetById(id);

            if (toDo == null || id <= 0) return NotFound();

            await _unitOfWork.ToDos.Delete(toDo);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateToDo(ToDo toDo)
        {
            var existToDo = await _unitOfWork.ToDos.GetById(toDo.Id);
            if (existToDo == null) return NotFound();

            await _unitOfWork.ToDos.Update(toDo);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }


        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        public async Task<IActionResult> UpdateToDo(int id,JsonPatchDocument<ToDo> patchDocument)
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

            patchDocument.ApplyTo(toDoDto,ModelState);

            if (!ModelState.IsValid)
                return BadRequest();
            
            existToDo.Name = toDoDto.Name;

            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
