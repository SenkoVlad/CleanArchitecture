using CleanArchitecture.Application.Notes.Commands;
using CleanArchitecture.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Notes.Commands
{
    public class CreateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new CreateNoteCommandHandler(Context);
            var noteDetails = "note details";
            var noteTitle = "note title";

            //Act
            var noteId = await handler.Handle(new CreateNoteCommand
            {
                Details = noteDetails,
                Title = noteTitle,
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

            //Assert
            Assert.NotNull(await Context.Notes.FirstOrDefaultAsync(note => note.Details == noteDetails &&
                                                                           note.Title == noteTitle &&
                                                                           note.Id == noteId));
        }
    }
}
