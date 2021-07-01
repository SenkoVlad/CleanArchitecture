using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain;
using System;

namespace CleanArchitecture.Application.ViewModels
{
    public class NoteLookupVm : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string TItle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteLookupVm>()
                   .ForMember(noteDto => noteDto.Id,
                              opt => opt.MapFrom(note => note.Id))
                   .ForMember(noteDto => noteDto.TItle,
                              opt => opt.MapFrom(note => note.Title));
        }
    }
}
