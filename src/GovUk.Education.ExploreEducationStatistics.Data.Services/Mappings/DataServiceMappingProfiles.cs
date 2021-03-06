using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using GovUk.Education.ExploreEducationStatistics.Common.Model.Data;
using GovUk.Education.ExploreEducationStatistics.Data.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Services.ViewModels;

namespace GovUk.Education.ExploreEducationStatistics.Data.Services.Mappings
{
    public class DataServiceMappingProfiles : Profile
    {
        public DataServiceMappingProfiles()
        {
            CreateMap<BoundaryLevel, BoundaryLevelIdLabel>();

            CreateMap<Location, LocationViewModel>();

            AppDomain.CurrentDomain.GetAssemblies().SelectMany(GetTypesFromAssembly)
                .Where(p => typeof(IObservationalUnit).IsAssignableFrom(p))
                .ToList().ForEach(type =>
                {
                    if (type == typeof(LocalAuthority))
                    {
                        CreateMap<LocalAuthority, CodeNameViewModel>()
                            .ForMember(model => model.Code,
                                opts => opts.MapFrom(localAuthority => localAuthority.GetCodeOrOldCodeIfEmpty()));
                    }
                    else
                    {
                        CreateMap(type, typeof(CodeNameViewModel));
                    }
                });

            CreateMap<Publication, TopicPublicationViewModel>();

            CreateMap<Subject, IdLabel>()
                .ForMember(dest => dest.Label, opts => opts.MapFrom(subject => subject.Name));

            CreateMap<Theme, ThemeViewModel>()
                .ForMember(dest => dest.Topics, opts => opts.Ignore());

            CreateMap<Topic, TopicViewModel>()
                .ForMember(dest => dest.Publications, opts => opts.Ignore());
        }

        private static IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
