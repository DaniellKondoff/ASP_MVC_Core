using AutoMapper;
using LearningSystem.Web.Infrastructure.Mapping;

namespace LearningSystem.Test
{
    public class TestStartUp
    {
        private static bool TestInitialized = false;

        public static void Initialize()
        {
            if (!TestInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                TestInitialized = true;
            }
        }
    }
}
