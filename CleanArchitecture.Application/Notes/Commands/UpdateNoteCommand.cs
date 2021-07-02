using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Notes.Commands
{
    public class UpdateNoteCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }

    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        private INotesDbContext _notesDbContext;
        public UpdateNoteCommandHandler(INotesDbContext notesDbContext) =>
            _notesDbContext = notesDbContext;
        public async Task<Unit> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _notesDbContext.Notes.FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);
            
            if(entity == null || entity.UserId != request.UserId)
                throw new NotFoundException(nameof(Note), request.Id);

            entity.Details = request.Details;
            entity.Title = request.Title;
            entity.EditDate = DateTime.Now;
            await _notesDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(updateNote => updateNote.Title).NotEmpty().MaximumLength(250);
            RuleFor(updateNote => updateNote.UserId).NotEqual(Guid.Empty);
            RuleFor(updateNote => updateNote.Id).NotEqual(Guid.Empty);
        }
    }
}
