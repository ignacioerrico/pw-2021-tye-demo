using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Business;
using Todo.Core.Dto;

namespace Todo.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoFacade _todoFacade;

        public TodoController(ITodoFacade todoFacade)
        {
            _todoFacade = todoFacade;
        }

        // GET: api/<TodoController>
        /// <summary>
        /// Get all tasks based on criteria.
        /// </summary>
        /// <param name="includeDeleted">Include deleted tasks</param>
        /// <param name="includePast">Include past tasks</param>
        /// <param name="includeCompleted">Include completed tasks</param>
        /// <returns>All tasks</returns>
        /// <response code="200">Returns all the tasks</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<TodoNoteDto> Get(bool includeDeleted = false, bool includePast = false, bool includeCompleted = false)
        {
            return _todoFacade.GetAll(includeDeleted, includePast, includeCompleted);
        }

        // GET api/<TodoController>/5
        /// <summary>
        /// Get a single task.
        /// </summary>
        /// <param name="id">ID of the task</param>
        /// <returns>The task</returns>
        /// <response code="200">Returns the task</response>
        /// <response code="404">If the task is not found</response>
        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TodoNoteDto> Get(int id)
        {
            var todoNoteModel = _todoFacade.GetById(id);
            if (todoNoteModel is null)
                return NotFound();

            return Ok(todoNoteModel);
        }

        // GET: api/<TodoController>/stats
        /// <summary>
        /// Gets the statistics.
        /// </summary>
        /// <returns>The stats</returns>
        /// <response code="200">Returns the task</response>
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<StatsDto> GetStats()
        {
            var statsModel = _todoFacade.GetStats();
            return Ok(statsModel);
        }

        // POST api/<TodoController>
        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="todoNoteDto"></param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created task</response>
        /// <response code="400">If the data submitted is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TodoNoteDto> Post([FromBody] TodoNoteDto todoNoteDto)
        {
            if (todoNoteDto is null)
                return BadRequest();

            var newTodoNoteModel = _todoFacade.CreateNew(todoNoteDto);

            return CreatedAtRoute("GetById", new { newTodoNoteModel.Id }, newTodoNoteModel);
        }

        // POST api/<TodoController>/markdone/5
        /// <summary>
        /// Marks a task as done.
        /// </summary>
        /// <param name="id">ID of the task</param>
        /// <returns></returns>
        /// <response code="204">Task marked as done</response>
        /// <response code="404">If the task is not found</response>
        [HttpPost("markdone")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult MarkDone([FromBody] int id)
        {
            var todoNoteToMarkAsDone = _todoFacade.MarkAsDone(id);
            if (todoNoteToMarkAsDone is null)
                return NotFound();

            return NoContent();
        }

        [HttpGet("wordfreq/{word}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetFrequency(string word)
        {
            var frequency = await _todoFacade.GetFrequencyAsync(word);
            return Ok(frequency);
        }

        // POST: api/<TodoController>/wordfreq
        /// <summary>
        /// Gets word frequencies.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/todo/wordfreq
        ///     {
        ///         "the",
        ///         "world",
        ///         "is",
        ///         "the",
        ///         "world"
        ///     }
        /// </remarks>
        /// <response code="200">Returns the word frequencies</response>
        [HttpPost("wordfreq")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> AddKeywords([FromBody] List<string> words)
        {
            var wordsAdded = await _todoFacade.AddKeywordsAsync(words);
            return Ok(wordsAdded);
        }

        // PUT api/<TodoController>/5
        /// <summary>
        /// Updates a task.
        /// </summary>
        /// <param name="id">ID of the task</param>
        /// <param name="todoNoteForUpdateDto"></param>
        /// <returns></returns>
        /// <response code="204">Task marked has been updated</response>
        /// <response code="400">If the data submitted is invalid</response>
        /// <response code="404">If the task is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Put(int id, [FromBody] TodoNoteForUpdateDto todoNoteForUpdateDto)
        {
            if (todoNoteForUpdateDto is null)
                return BadRequest();

            var todoNoteToUpdate = _todoFacade.UpdateExisting(id, todoNoteForUpdateDto);
            if (todoNoteToUpdate is null)
                return NotFound();

            return NoContent();
        }

        // DELETE api/<TodoController>/5
        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="id">ID of the task</param>
        /// <returns></returns>
        /// <response code="204">Task deleted</response>
        /// <response code="404">If the task is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var todoNoteToDelete = _todoFacade.Delete(id);
            if (todoNoteToDelete is null)
                return NotFound();

            return NoContent();
        }
    }
}
