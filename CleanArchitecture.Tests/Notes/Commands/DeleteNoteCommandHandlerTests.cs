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
    public class DeleteNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new DeleteNoteCommandHandler(Context);
            //Act
            await handler.Handle(new DeleteNoteCommand
            {
                Id = NotesContextFactory.NoteIdForDelete,
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);
            //Assert
            Assert.Null(await Context.Notes.FirstOrDefaultAsync(note => note.Id == NotesContextFactory.NoteIdForDelete));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailForWrongId()
        {
            //Arrange
            var handler = new DeleteNoteCommandHandler(Context);
            //Act
            Func<Task> act = async () =>
                               await handler.Handle(new DeleteNoteCommand
                               {
                                   Id = Guid.NewGuid(),
                                   UserId = NotesContextFactory.UserAId
                               }, CancellationToken.None);
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailForWrongUserId()
        {
            //Arrange
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteId = await createHandler.Handle(new CreateNoteCommand
            {
                Title = "note title",
                Details = "note details",
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);
            //Act
            Func<Task> act = async () =>
                               await deleteHandler.Handle(new DeleteNoteCommand
                               {
                                   Id = noteId,
                                   UserId = NotesContextFactory.UserBId
                               }, CancellationToken.None);
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }
    }
}
