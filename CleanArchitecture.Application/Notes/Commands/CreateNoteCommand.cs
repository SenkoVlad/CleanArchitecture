using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Notes.Commands
{
    public class CreateNoteCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }

    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
    {
        private INotesDbContext _notesDbContext;
        public CreateNoteCommandHandler(INotesDbContext notesDbContext) =>
            _notesDbContext = notesDbContext;

        public async Task<Guid> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Title = request.Title,
                Details = request.Details,
                CreationDate = DateTime.Now,
                EditDate = null
            };
            await _notesDbContext.Notes.AddAsync(note, cancellationToken);
            await _notesDbContext.SaveChangesAsync(cancellationToken);
            return note.Id;
        }
    }

    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(createNote => createNote.Title).NotEmpty().MaximumLength(250);
            RuleFor(createNote => createNote.UserId).NotEqual(Guid.Empty);
        }
    }
}
