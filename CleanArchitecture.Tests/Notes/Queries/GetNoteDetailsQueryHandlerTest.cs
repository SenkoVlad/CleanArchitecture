using AutoMapper;
using CleanArchitecture.Application.Notes.Queries;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Persistence;
using CleanArchitecture.Tests.Common;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Notes.Q
{
    [Collection("QueryCollection")]
    public class GetNoteDetailsQueryHandlerTest
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;

        public GetNoteDetailsQueryHandlerTest(QueryTestFixture queryTestFixture)
        {
            Context = queryTestFixture.Context;
            Mapper = queryTestFixture.Mapper;
        }

        [Fact]
        public async Task GetNoteDetailsQueryHandler_Success()
        {
            //Arrange
            var handle = new GetNotesDetailsQueryHandler(Context, Mapper);
            //Act 
            var result = await handle.Handle(new GetNoteDetailsQuery
            {
                UserId = NotesContextFactory.UserBId,
                Id = Guid.Parse("909F7C29-891B-4BE1-8504-21F84F262084")
            }, CancellationToken.None);
            //Assert
            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("Title2");
            result.CreationDate.ShouldBe(DateTime.Today);
        }
    }
}
