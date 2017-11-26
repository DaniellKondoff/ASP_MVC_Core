using AutoMapper;

namespace LearningSystem.Core.Mapping
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}
