using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Notes.Queries
{
    class GetNotesDetailsQuery : IRequest<NoteDetailsVm> 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    class GetNotesDetailsQueryHandler : IRequestHandler<GetNotesDetailsQuery, NoteDetailsVm>
    {
        private INotesDbContext _notesDbContext;
        private IMapper _mapper;
        public GetNotesDetailsQueryHandler(INotesDbContext notesDbContext, IMapper mapper)
        {
            _notesDbContext = notesDbContext;
            _mapper = mapper;
        }
        public async Task<NoteDetailsVm> Handle(GetNotesDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _notesDbContext.Notes.FirstOrDefaultAsync(note => note.Id == request.Id);
            if (entity == null || entity.UserId != request.UserId)
                throw new NotFoundException(nameof(Note), request.Id);

            return _mapper.Map<NoteDetailsVm>(entity);
        }
    }
}
