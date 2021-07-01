using System.Collections.Generic;

namespace CleanArchitecture.Application.ViewModels
{
    public class NoteListVm 
    {
        public IList<NoteLookupVm> Notes { get; set; }
    }
}
