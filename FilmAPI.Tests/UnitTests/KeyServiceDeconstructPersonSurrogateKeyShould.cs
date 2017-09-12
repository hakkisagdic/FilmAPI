﻿using FilmAPI.Core.Entities;
using FilmAPI.ViewModels;
using Xunit;

namespace FilmAPI.Tests.UnitTests
{
    public class KeyServiceDeconstructPersonSurrogateKeyShould : KeyTestBase
    {
        [Fact]
        public void BeInverseToConstrtPersonSurrogateKey()
        {
            // Arrange
            string lastName = "Gibson";
            string birthdate = "1949-12-13";
            string key = _keyService.ConstructPersonSurrogateKey(lastName, birthdate);
            PersonViewModel m = new PersonViewModel(new Person(lastName, birthdate), key);

            // Act    
            (string LastName, string Birthdate) = _keyService.DeconstructPesonSurrogateKey(key);

            // Assert
            Assert.Equal(lastName, LastName);
            Assert.Equal(birthdate, Birthdate);
        }
    }
}