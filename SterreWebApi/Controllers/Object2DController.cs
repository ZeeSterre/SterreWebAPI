using SterreWebApi.Models;
using SterreWebApi.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace SterreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Object2DController : ControllerBase
    {
        private readonly IObject2DRepository _repository;

        public Object2DController(IObject2DRepository repository)
        {
            _repository = repository;
        }

        // GET: /Object2D
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Object2D>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: /Object2D/{id}

        [HttpGet("{id}")]

        public async Task<ActionResult<Object2D>> GetById(Guid id)
        {
            var object2D = await _repository.GetByIdAsync(id);
            if (object2D == null)
                return NotFound("Object not found");

            return Ok(object2D);
        }

        // POST: /Object2D

        [HttpPost]

        public async Task<ActionResult<Object2D>> Create([FromBody] Object2D object2D)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdObject2D = await _repository.AddAsync(object2D);
            return CreatedAtAction(nameof(GetById), new { id = createdObject2D.Id }, createdObject2D);
        }

        // PUT: /Object2D/{id}
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(Guid id, [FromBody] Object2D updatedObject2D)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _repository.UpdateAsync(id, updatedObject2D);
            if (!success)
                return NotFound("Object not found.");

            return NoContent();
        }

        // DELETE: /Object2D/{id}

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success)
                return NotFound("Object not found.");

            return NoContent();

        }
    }
}
