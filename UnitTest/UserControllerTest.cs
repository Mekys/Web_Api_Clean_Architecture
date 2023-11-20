using Api.Controllers;
using Application;
using Application.IRepositories;
using Domain.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class UserControllerTest
    {
        [Fact]
        public async void GetAllUser_Count_Greater_Zero()
        {
            // Arrange
            int count = 5;
            var fakeUsers = A.CollectionOfDummy<User>(count).AsEnumerable().ToList();
            var userRepMock = A.Fake<IUserRepository>();
            A.CallTo(() => userRepMock.GetAllAsync()).Returns(fakeUsers);
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.GetAll();

            // Assert
            Assert.Equal(count, actionRes.Count);
        }
        [Fact]
        public async void GetAllAsync_Count_Equal_Zero()
        {
            // Arrange
            int count = 0;
            var fakeUsers = A.CollectionOfDummy<User>(count).AsEnumerable().ToList();
            var userRepMock = A.Fake<IUserRepository>();
            A.CallTo(() => userRepMock.GetAllAsync()).Returns(fakeUsers);
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.GetAll();

            // Assert
            Assert.Equal(count, actionRes.Count);
        }
        [Fact]
        public async void GetByIdAsync_Id_Exist()
        {
            // Arrange
            var userRepMock = A.Fake<IUserRepository>();
            var fakeUser = A.Fake<User>();
            fakeUser.UserID = 5;

            A.CallTo(() => userRepMock.GetByIdAsync(5)).Returns(fakeUser);
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.Get(5);

            // Assert
            Assert.Equal(fakeUser, actionRes);
        }
        [Fact]
        public async void GetByIdAsync_Id_Not_Exist()
        {
            // Arrange
            var userRepMock = A.Fake<IUserRepository>();
            User fakeUser = null;

            A.CallTo(() => userRepMock.GetByIdAsync(5)).Returns(fakeUser);
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.Get(5);

            // Assert
            Assert.Equal(fakeUser, actionRes);
        }
        [Fact]
        public async void DeleteAsync_Id_Not_Exist()
        {
            // Arrange
            var userRepMock = A.Fake<IUserRepository>();


            A.CallTo(() => userRepMock.DeleteAsync(10)).Returns(new NotFoundResult());
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.Delete(10);

            // Assert
            Assert.IsType<NotFoundResult>(actionRes);
        }
        [Fact]
        public async void DeleteAsync_Id_Exist_Try_Delete_User()
        {
            // Arrange
            var userRepMock = A.Fake<IUserRepository>();
/*            var fakeUsers = A.CollectionOfDummy<User>(5).AsEnumerable().ToArray();
            for(int index = 1; index <= 5; index++)
            {
                fakeUsers[index - 1].UserID = index;
            }*/
            

            A.CallTo(() => userRepMock.DeleteAsync(10)).Returns(new OkResult());
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.Delete(10);

            // Assert
            Assert.IsType<OkResult>(actionRes);
        }
        [Fact]
        public async void DeleteAsync_Id_Exist_Try_Delete_Admin()
        {
            // Arrange
            var userRepMock = A.Fake<IUserRepository>();
/*            var fakeUsers = A.CollectionOfDummy<User>(5).AsEnumerable().ToArray();
            for(int index = 1; index <= 5; index++)
            {
                fakeUsers[index - 1].UserID = index;
            }*/
            

            A.CallTo(() => userRepMock.DeleteAsync(7)).Returns(new BadRequestResult());
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.Delete(7);

            // Assert
            Assert.IsType<BadRequestResult>(actionRes);
        }
        [Fact]
        public async void PostAsync_Current_Login_Exist()
        {
            // Arrange
            var userRepMock = A.Fake<IUserRepository>();
            var fakeUser = A.Fake<User>();

            A.CallTo(() => userRepMock.AddAsync(fakeUser)).Returns(new BadRequestResult());
            UserController controller = new UserController(userRepMock);

            // Act
            var actionRes = await controller.Post(fakeUser);

            // Assert
            Assert.IsType<BadRequestResult>(actionRes);
        }

    }
}
