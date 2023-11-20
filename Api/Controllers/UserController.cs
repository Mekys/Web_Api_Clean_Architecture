using Application.IRepositories;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        [Route("api/User/Get")]
        public async Task<List<User>> GetAll()
        {
            var data = await _userRepository.GetAllAsync();
            return data.ToList();
        }   
        
        [HttpPost]
        [Route("api/User/Post")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var response = await _userRepository.AddAsync(user);
            return response;
        }

        [HttpDelete]
        [Route("api/User/Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userRepository.DeleteAsync(id);
            return response;
        }       
        
        [HttpGet]
        [Route("api/User/Get/{id}")]
        public async Task<User> Get(int id)
        {
            var response = await _userRepository.GetByIdAsync(id);
            return response;
        }
    }
}
