﻿using Bogus;
using Core.Test.Dtos;

namespace Core.Test.Bogus
{
    public  class BogusTestFixture
    {
        public Faker Faker;

        public BogusTestFixture()
        {
            Faker = new Faker("pt_BR");
        }

        public UserDto GetUserPassword()
        {
            Faker = new Faker("pt_BR");
            return new UserDto
            {
                UserName = Faker.Internet.Email().ToLower(),
                Password = Faker.Internet.Password(8, false, "", "@1Ab_")
            };
        }
    }
}
