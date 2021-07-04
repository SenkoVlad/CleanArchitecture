using AutoMapper;
using CleanArchitecture.Application.Notes.Commands;
using CleanArchitecture.Application.Notes.Queries;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        private IMapper _mapper;
        public NoteController(IMapper mapper) =>
            _mapper = mapper;

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<NoteListVm>> GetAll()
        {
            var query = new GetNotesListQuery
            {
                UserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        
        [Authorize]
        [HttpGet("{id}")]
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
        {
            var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
            command.UserId = UserId;
            var noteId = await Mediator.Send(command);
            return Ok(noteId);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            command.UserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
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
