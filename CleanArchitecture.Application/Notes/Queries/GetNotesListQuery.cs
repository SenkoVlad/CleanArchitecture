using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Notes.Queries
{
    public class GetNotesListQuery : IRequest<NoteListVm>
    {
        public Guid UserId { get; set; }
    }

    public class GetNotesListQueryHandler : IRequestHandler<GetNotesListQuery, NoteListVm>
    {
        private INotesDbContext _notesDbContext;
        private IMapper _mapper;

        public GetNotesListQueryHandler(INotesDbContext notesDbContext, IMapper mapper)
        {
            _notesDbContext = notesDbContext;
            _mapper = mapper;
        }

        public async Task<NoteListVm> Handle(GetNotesListQuery request, CancellationToken cancellationToken)
        {
            var notesQuery = await _notesDbContext.Notes.Where(note => note.UserId == request.UserId)
                                                        .ProjectTo<NoteLookupVm>(_mapper.ConfigurationProvider)
                                                        .ToListAsync(cancellationToken);
            return new NoteListVm()
            {
                Notes = notesQuery
            };
        }
    }
}
