using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Notes.Commands;
using CleanArchitecture.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var updateTitle = "note title";
            //Act
            await handler.Handle(new UpdateNoteCommand
            {
                Title = updateTitle,
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserBId
            }, CancellationToken.None);
            //Assert
            Assert.NotNull(await Context.Notes.FirstOrDefaultAsync(note => note.Id == NotesContextFactory.NoteIdForUpdate &&
                                                                           note.Title == updateTitle));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailForWrongId()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            //Act
            Func<Task> act = async () =>
                               await handler.Handle(new UpdateNoteCommand
                               {
                                   Id = Guid.NewGuid(),
                                   UserId = NotesContextFactory.UserAId
                               }, CancellationToken.None);
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailForUserId()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            //Act
            Func<Task> act = async () =>
                               await handler.Handle(new UpdateNoteCommand
                               {
                                   Id = NotesContextFactory.NoteIdForUpdate,
                                   UserId = NotesContextFactory.UserAId
                               }, CancellationToken.None);
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }
    }
}
