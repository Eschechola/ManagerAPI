using Bogus;
using Bogus.DataSets;
using Manager.Domain.Entities;
using Manager.Services.DTO;
using System.Collections.Generic;

namespace Manager.Tests.Fixtures
{
    public class UserFixture
    {
        public static User CreateValidUser()
        {
            return new User(
                name: new Name().FirstName(),
                email: new Internet().Email(),
                password: new Internet().Password());
        }

        public static List<User> CreateListValidUser(int limit = 5)
        {
            var list = new List<User>();

            for (int i = 0; i < limit; i++)
                list.Add(CreateValidUser());

            return list;
        }

        public static UserDTO CreateValidUserDTO(bool newId = false)
        {
            return new UserDTO
            {
                Id = newId ? new Randomizer().Int(0, 1000) : 0,
                Name = new Name().FirstName(),
                Email = new Internet().Email(),
                Password = new Internet().Password()
            };
        }

        public static UserDTO CreateInvalidUserDTO()
        {
            return new UserDTO
            {
                Id = 0,
                Name = "",
                Email = "",
                Password = ""
            };
        }
    }
}
