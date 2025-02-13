using SterreWebApi.Models;
using SterreWebApi.Repositorys; 
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace SterreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Environment2DController : ControllerBase
    {
        private readonly Environment2DRepository _repository;

        public Environment2DController(Environment2DRepository repository)
        {
            _repository = repository;
        }

        // GET: /Environment2D
        [HttpGet]

        public ActionResult<IEnumerable<Environment2D>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        // GET: /Environment2D/{id}

        [HttpGet("{id}")]

        public ActionResult<Environment2D> GetById(int id)
        {
            var environment = _repository.GetById(id);
            if (environment == null)
                return NotFound("Environment not found");

            return Ok(environment);
        }

        // POST: /Environment2D

        [HttpPost]

        public ActionResult<Environment2D> Create([FromBody] Environment2D environment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEnvironment = _repository.Add(environment);
            return CreatedAtAction(nameof(GetById), new { id = createdEnvironment.Id }, createdEnvironment);
        }

        // PUT: /Environment2D/{id}
        [HttpPut("{id}")]

        public IActionResult Update(int id, [FromBody] Environment2D updatedEnvironment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _repository.Update(id, updatedEnvironment);
            if (!success)
                return NotFound("Environment not found.");

            return NoContent();
        }

        // DELETE: /Environment2D/{id}

        [HttpDelete("{id}")]

        public IActionResult Delete (int id)
        {
            var success = _repository.Delete(id);
            if (!success)
                return NotFound("Environment not found.");

            return NoContent();

        }
    }
}
