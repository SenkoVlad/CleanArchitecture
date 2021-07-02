using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace CleanArchitecture.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly) =>
            ApplyMappingsFromAssembly(assembly);
        
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                                .Where(type => type.GetInterfaces()
                                .Any(i => i.IsGenericType &&
                                          i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                                .ToList();
            foreach (var item in types)
            {
                var instance = Activator.CreateInstance(item);
                var methodTo = item.GetMethod("Mapping");
                methodTo?.Invoke(instance, new object[] {this});
            }
        }
    }
}
