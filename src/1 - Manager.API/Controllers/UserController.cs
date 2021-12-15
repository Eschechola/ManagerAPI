using System.Threading.Tasks;
using Manager.API.ViewModes;
using Microsoft.AspNetCore.Mvc;
using Manager.Services.Interfaces;
using AutoMapper;
using Manager.Services.DTO;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Manager.Core.Communication.Messages.Notifications;

namespace Manager.API.Controllers
{

    [ApiController]
    public class UserController : BaseController
    {

        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(
            IMapper mapper,
            IUserService userService,
            INotificationHandler<DomainNotification> domainNotificationHandler)
            : base(domainNotificationHandler)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        [Route("/api/v1/users/create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserViewModel userViewModel)
        {
            var userDTO = _mapper.Map<UserDTO>(userViewModel);
            var userCreated = await _userService.CreateAsync(userDTO);

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuário criado com sucesso!",
                Success = true,
                Data = userCreated.Value
            });
        }

        [HttpPut]
        [Authorize]
        [Route("/api/v1/users/update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserViewModel userViewModel)
        {
            var userDTO = _mapper.Map<UserDTO>(userViewModel);
            var userUpdated = await _userService.UpdateAsync(userDTO);

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuário atualizado com sucesso!",
                Success = true,
                Data = userUpdated.Value
            });
        }

        [HttpDelete]
        [Authorize]
        [Route("/api/v1/users/remove/{id}")]
        public async Task<IActionResult> RemoveAsync(long id)
        {
            await _userService.RemoveAsync(id);

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuário removido com sucesso!",
                Success = true,
                Data = null
            });
        }

        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/get/{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var user = await _userService.GetAsync(id);

            if (HasNotifications())
                return Result();

            if (!user.HasValue)
                return Ok(new ResultViewModel
                {
                    Message = "Nenhum usuário foi encontrado com o ID informado.",
                    Success = true,
                    Data = user.Value
                });

            return Ok(new ResultViewModel
            {
                Message = "Usuário encontrado com sucesso!",
                Success = true,
                Data = user.Value
            });
        }


        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/get-all")]
        public async Task<IActionResult> GetAsync()
        {
            var allUsers = await _userService.GetAllAsync();

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuários encontrados com sucesso!",
                Success = true,
                Data = allUsers.Value
            });
        }


        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/get-by-email")]
        public async Task<IActionResult> GetByEmailAsync([FromQuery] string email)
        {
            var user = await _userService.GetByEmailAsync(email);

            if (HasNotifications())
                return Result();

            if (!user.HasValue)
                return Ok(new ResultViewModel
                {
                    Message = "Nenhum usuário foi encontrado com o email informado.",
                    Success = true,
                    Data = user.Value
                });

            return Ok(new ResultViewModel
            {
                Message = "Usuário encontrado com sucesso!",
                Success = true,
                Data = user.Value
            });
        }

        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/search-by-name")]
        public async Task<IActionResult> SearchByNameAsync([FromQuery] string name)
        {
            var allUsers = await _userService.SearchByNameAsync(name);

            if (HasNotifications())
                return Result();

            if (!allUsers.HasValue)
                return Ok(new ResultViewModel
                {
                    Message = "Nenhum usuário foi encontrado com o nome informado",
                    Success = true,
                    Data = null
                });

            return Ok(new ResultViewModel
            {
                Message = "Usuário encontrado com sucesso!",
                Success = true,
                Data = allUsers
            });
        }


        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/search-by-email")]
        public async Task<IActionResult> SearchByEmailAsync([FromQuery] string email)
        {
            var allUsers = await _userService.SearchByEmailAsync(email);

            if (HasNotifications())
                return Result();

            if (!allUsers.HasValue)
                return Ok(new ResultViewModel
                {
                    Message = "Nenhum usuário foi encontrado com o email informado",
                    Success = true,
                    Data = null
                });

            return Ok(new ResultViewModel
            {
                Message = "Usuário encontrado com sucesso!",
                Success = true,
                Data = allUsers
            });
        }
    }
}