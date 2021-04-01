using AutoMapper;

namespace MyHome.Application.Common.Profiles
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
