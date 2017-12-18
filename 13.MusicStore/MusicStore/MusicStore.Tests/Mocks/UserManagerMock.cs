using Microsoft.AspNetCore.Identity;
using Moq;
using MusicStore.Data.Models;

namespace MusicStore.Tests.Mocks
{
    public class UserManagerMock
    {
        public static Mock<UserManager<User>> New()
        {
            return new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        }
    }
}
