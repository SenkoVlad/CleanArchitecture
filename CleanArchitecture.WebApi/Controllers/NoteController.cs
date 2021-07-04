using AutoMapper;
using CleanArchitecture.Application.Notes.Commands;
using CleanArchitecture.Application.Notes.Queries;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        private IMapper _mapper;
        public NoteController(IMapper mapper) =>
            _mapper = mapper;

        /// <summary>
        /// Get the list of notes
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Get api/note
        /// </remarks>
        /// <returns>Returns NoteListVm</returns>
        /// <response code="200">Success</response> 
        /// <response code="401">If an user is unauthorized</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteListVm>> GetAll()
        {
            var query = new GetNotesListQuery
            {
                UserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        /// <summary>
        /// Get the note by id
        /// </summary>
        /// <param name="id">Guid id</param>
        /// <remarks>
        /// Sample request:
        /// Get api/note/{GUID}
        /// </remarks>
        /// <returns>Returns NoteDetailsVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If an user is unauthorized</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteDetailsVm>> Get(Guid id)
        {
            var query = new GetNoteDetailsQuery
            {
                Id = id,
                UserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Create the note
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST api/note
        /// {
        ///     "Title" : "new title",
        ///     "Details" : "new details",
        /// }
        /// </remarks>
        /// <param name="createNoteDto">createNoteDto object</param>
        /// <response code="200">Success</response>
        /// <response code="401">If an user is unauthorized</response>
        /// <returns>Guid of a new note</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
        {
            var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
            command.UserId = UserId;
            var noteId = await Mediator.Send(command);
            return Ok(noteId);
        }

        /// <summary>
        /// Update the note
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Put api/note
        /// {
        ///     "Title" : "new title",
        /// }
        /// </remarks>
        /// <param name="updateNoteDto">updateNoteDto object</param>
        /// <response code="200">Success</response>
        /// <response code="401">If an user is unauthorized</response>
        /// <returns>none</returns>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            command.UserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Delete the note by GUID
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Delete api/note/{GUID}
        /// </remarks>
        /// <param name="id">Guid id for deleting</param>
        /// <response code="204">Success</response>
        /// <response code="401">If an user is unauthorized</response>
        /// <returns>none</returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteNoteCommand
            {
                Id = id,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
