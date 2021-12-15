using AutoMapper;
using Bogus;
using Bogus.DataSets;
using EscNet.Cryptography.Interfaces;
using FluentAssertions;
using Manager.Core.Communication.Mediator.Interfaces;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Tests.Configurations.AutoMapper;
using Manager.Tests.Fixtures;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Expressions;

namespace Manager.Tests.Projects.Services
{
    public class UserServiceTests
    {
        // Subject Under Test (Quem será testado!)
        private readonly IUserService _sut;

        //Mocks
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRijndaelCryptography> _rijndaelCryptographyMock;
        private readonly Mock<IMediatorHandler> _mediatorHandler;

        public UserServiceTests()
        {
            _mapper = AutoMapperConfiguration.GetConfiguration();
            _userRepositoryMock = new Mock<IUserRepository>();
            _rijndaelCryptographyMock = new Mock<IRijndaelCryptography>();
            _mediatorHandler = new Mock<IMediatorHandler>();

            _sut = new UserService(
                mapper: _mapper,
                userRepository: _userRepositoryMock.Object,
                rijndaelCryptography: _rijndaelCryptographyMock.Object,
                mediator: _mediatorHandler.Object);
        }

        #region Create

        [Fact(DisplayName = "Create Valid User")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserIsValid_ReturnsUserDTO()
        {
            // Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();

            var encryptedPassword = new Lorem().Sentence();
            var userCreated = _mapper.Map<User>(userToCreate);
            userCreated.SetPassword(encryptedPassword);

            _userRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(() => null);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(() => userCreated);

            // Act
            var result = await _sut.CreateAsync(userToCreate);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));
        }

        [Fact(DisplayName = "Create When User Exists")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();
            var userExists = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(() => userExists);

            // Act
            var result = await _sut.CreateAsync(userToCreate);


            // Act
            result.HasValue.Should()
                .BeFalse();
        }

        [Fact(DisplayName = "Create When User is Invalid")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserIsInvalid_ReturnsEmptyOptional()
        {
            // Arrange
            var userToCreate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.CreateAsync(userToCreate);


            // Act
            result.HasValue.Should()
                .BeFalse();
        }

        #endregion

        #region Update

        [Fact(DisplayName = "Update Valid User")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserIsValid_ReturnsUserDTO()
        {
            // Arrange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateValidUserDTO();
            var userUpdated = _mapper.Map<User>(userToUpdate);

            var encryptedPassword = new Lorem().Sentence();

            _userRepositoryMock.Setup(x => x.GetAsync(oldUser.Id))
            .ReturnsAsync(() => oldUser);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(() => userUpdated);

            // Act
            var result = await _sut.UpdateAsync(userToUpdate);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userUpdated));
        }

        [Fact(DisplayName = "Update When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserNotExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userToUpdate = UserFixture.CreateValidUserDTO();

            _userRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.UpdateAsync(userToUpdate);

            // Act
            result.HasValue.Should()
                .BeFalse();
        }

        [Fact(DisplayName = "Update When User is Invalid")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserIsInvalid_ReturnsEmptyOptional()
        {
            // Arrange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(() => oldUser);

            // Act
            var result = await _sut.UpdateAsync(userToUpdate);
 

            // Act
            result.HasValue.Should()
                .BeFalse();
        }

        #endregion

        #region Remove

        [Fact(DisplayName = "Remove User")]
        [Trait("Category", "Services")]
        public async Task Remove_WhenUserExists_RemoveUser()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.RemoveAsync(It.IsAny<int>()))
                .Verifiable();

            // Act
            await _sut.RemoveAsync(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.RemoveAsync(userId), Times.Once);
        }

        #endregion

        #region Get

        [Fact(DisplayName = "Get By Id")]
        [Trait("Category", "Services")]
        public async Task GetById_WhenUserExists_ReturnsUserDTO()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);
            var userFound = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => userFound);

            // Act
            var result = await _sut.GetAsync(userId);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Id When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task GetById_WhenUserNotExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.GetAsync(userId))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAsync(userId);

            // Assert
            result.Value.Should()
                .BeNull();
        }

        [Fact(DisplayName = "Get By Email")]
        [Trait("Category", "Services")]
        public async Task GetByEmail_WhenUserExists_ReturnsUserDTO()
        {
            // Arrange
            var userEmail = new Internet().Email();
            var userFound = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(() => userFound);

            // Act
            var result = await _sut.GetByEmailAsync(userEmail);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Email When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task GetByEmail_WhenUserNotExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userEmail = new Internet().Email();

            _userRepositoryMock.Setup(x => x.GetAsync(
                 It.IsAny<Expression<Func<User, bool>>>(),
                 It.IsAny<bool>()))
             .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetByEmailAsync(userEmail);

            // Assert
            result.Value.Should()
                .BeNull();
        }

        [Fact(DisplayName = "Get All Users")]
        [Trait("Category", "Services")]
        public async Task GetAllUsers_WhenUsersExists_ReturnsAListOfUserDTO()
        {
            // Arrange
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Get All Users When None User Found")]
        [Trait("Category", "Services")]
        public async Task GetAllUsers_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange

            _userRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Value.Should()
                .BeEmpty();
        }

        #endregion

        #region Search

        [Fact(DisplayName = "Search By Name")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenAnyUserFound_ReturnsAListOfUserDTO()
        {
            // Arrange
            var nameToSearch = new Name().FirstName();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchAsync(
                 It.IsAny<Expression<Func<User, bool>>>(),
                 It.IsAny<bool>()))
             .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.SearchByNameAsync(nameToSearch);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Name When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange
            var nameToSearch = new Name().FirstName();

            _userRepositoryMock.Setup(x => x.SearchAsync(
                 It.IsAny<Expression<Func<User, bool>>>(),
                 It.IsAny<bool>()))
             .ReturnsAsync(() => null);

            // Act
            var result = await _sut.SearchByNameAsync(nameToSearch);

            // Assert
            result.Value.Should()
                .BeEmpty();
        }

        [Fact(DisplayName = "Search By Email")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenAnyUserFound_ReturnsAListOfUserDTO()
        {
            // Arrange
            var emailSoSearch = new Internet().Email();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchAsync(
                 It.IsAny<Expression<Func<User, bool>>>(),
                 It.IsAny<bool>()))
             .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.SearchByEmailAsync(emailSoSearch);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Email When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange
            var emailSoSearch = new Internet().Email();

            _userRepositoryMock.Setup(x => x.SearchAsync(
                 It.IsAny<Expression<Func<User, bool>>>(),
                 It.IsAny<bool>()))
             .ReturnsAsync(() => null);

            // Act
            var result = await _sut.SearchByEmailAsync(emailSoSearch);

            // Assert
            result.Value.Should()
                .BeEmpty();
        }

        #endregion
    }
}
