using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Notes.Queries
{
    public class GetNoteDetailsQuery : IRequest<NoteDetailsVm> 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetNotesDetailsQueryHandler : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
    {
        private INotesDbContext _notesDbContext;
        private IMapper _mapper;
        public GetNotesDetailsQueryHandler(INotesDbContext notesDbContext, IMapper mapper)
        {
            _notesDbContext = notesDbContext;
            _mapper = mapper;
        }
        public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _notesDbContext.Notes.FirstOrDefaultAsync(note => note.Id == request.Id);
            if (entity == null || entity.UserId != request.UserId)
                throw new NotFoundException(nameof(Note), request.Id);

            return _mapper.Map<NoteDetailsVm>(entity);
        }
    }

    public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>
    {
        public GetNoteDetailsQueryValidator()
        {
            RuleFor(noteDetails => noteDetails.UserId).NotEqual(Guid.Empty);
            RuleFor(noteDetails => noteDetails.Id).NotEqual(Guid.Empty);
        }
    }
}
