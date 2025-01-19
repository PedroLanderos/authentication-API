using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationApi.Application.Responses;

namespace UnitTest.AuthenticationApi.Controllers
{
    public class AuthenticationControllerTest
    {
        private readonly IUser authenticationInterface;
        private readonly AuthenticationApiController authenticationController;

        public AuthenticationControllerTest()
        {
            authenticationInterface = A.Fake<IUser>();
            authenticationController = new AuthenticationApiController(authenticationInterface);
        }

        [Fact]
        public async Task Register_ShouldReturn200_WhenSuccessful()
        {
            // Arrange
            var userDTO = new UserDTO(0, "John Doe", "1234567890", "123 Main St", "john@example.com", "password123", "User");
            var response = new ApiResponse(true, "User registered successfully");
            A.CallTo(() => authenticationInterface.Register(userDTO)).Returns(Task.FromResult(response));

            // Act
            var result = await authenticationController.Register(userDTO);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Register_ShouldReturn400_WhenModelStateIsInvalid()
        {
            // Arrange
            var userDTO = new UserDTO(0, "", "1234567890", "123 Main St", "invalid-email", "password123", "User");
            authenticationController.ModelState.AddModelError("Email", "Invalid email address");

            // Act
            var result = await authenticationController.Register(userDTO);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Login_ShouldReturn200_WhenSuccessful()
        {
            // Arrange
            var loginDTO = new LoginDTO("john@example.com", "password123");
            var response = new ApiResponse(true, "Login successful");
            A.CallTo(() => authenticationInterface.Login(loginDTO)).Returns(Task.FromResult(response));

            // Act
            var result = await authenticationController.Login(loginDTO);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Login_ShouldReturn400_WhenModelStateIsInvalid()
        {
            // Arrange
            var loginDTO = new LoginDTO("", "");
            authenticationController.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await authenticationController.Login(loginDTO);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetUser_ShouldReturn200_WhenUserExists()
        {
            // Arrange
            var userDTO = new UserDTO(1, "John Doe", "1234567890", "123 Main St", "john@example.com", "password123", "User");
            A.CallTo(() => authenticationInterface.GetUser(1)).Returns(Task.FromResult(userDTO));

            // Act
            var result = await authenticationController.GetUser(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetUser_ShouldReturn400_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await authenticationController.GetUser(invalidId);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetUser_ShouldReturn404_WhenUserNotFound()
        {
            // Arrange
            A.CallTo(() => authenticationInterface.GetUser(1)).Returns(Task.FromResult<UserDTO>(null));

            // Act
            var result = await authenticationController.GetUser(1);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturn200_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO(1, "John Doe", "1234567890", "123 Main St", "john@example.com", "password123", "User"),
                new UserDTO(2, "Jane Doe", "0987654321", "456 Elm St", "jane@example.com", "password456", "Admin")
            };
            A.CallTo(() => authenticationInterface.GetAllUsers()).Returns(Task.FromResult(users.AsEnumerable()));

            // Act
            var result = await authenticationController.GetAllUsers();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturn404_WhenNoUsersFound()
        {
            // Arrange
            A.CallTo(() => authenticationInterface.GetAllUsers()).Returns(Task.FromResult(Enumerable.Empty<UserDTO>()));

            // Act
            var result = await authenticationController.GetAllUsers();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult?.StatusCode.Should().Be(404);
        }
    }
}
