using AutoMapper;
using CleanArchitecture.Application.Notes.Queries;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Persistence;
using CleanArchitecture.Tests.Common;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Notes.Queries
{
    [Collection("QueryCollection")]
    public class GetNotesListQueryHandlerTest
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;

        public GetNotesListQueryHandlerTest(QueryTestFixture queryTestFixture)
        {
            Context = queryTestFixture.Context;
            Mapper = queryTestFixture.Mapper;
        }

        [Fact]
        public async Task GetNotesListQueryHandler_Success()
        {
            //Arragne
            var hanlder = new GetNotesListQueryHandler(Context, Mapper);
            //Act
            var result = await hanlder.Handle(new GetNotesListQuery
            {
                UserId = NotesContextFactory.UserBId
            }, CancellationToken.None);
            //Assert
            result.ShouldBeOfType<NoteListVm>();
            result.Notes.Count.ShouldBe(2);
        }
    }
}
