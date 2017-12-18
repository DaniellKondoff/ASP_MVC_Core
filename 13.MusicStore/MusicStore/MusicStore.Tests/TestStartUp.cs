using AutoMapper;
using MusicStore.Web.Infrastructure.Mapping;

namespace MusicStore.Tests
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
