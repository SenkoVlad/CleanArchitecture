using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Notes.Commands
{
    public class DeleteNoteCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
    {
        private INotesDbContext _notesDbContext;
        public DeleteNoteCommandHandler(INotesDbContext notesDbContext) =>
            _notesDbContext = notesDbContext;

        public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _notesDbContext.Notes.FirstOrDefaultAsync(note => note.Id == request.Id);
            if (entity == null || entity.UserId != request.UserId)
                throw new NotFoundException(nameof(Note), request.Id);

            _notesDbContext.Notes.Remove(entity);
            await _notesDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
