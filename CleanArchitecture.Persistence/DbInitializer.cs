using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(NotesDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
