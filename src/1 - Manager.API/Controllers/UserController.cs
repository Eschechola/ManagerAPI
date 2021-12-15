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
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel)
        {
            var userDTO = _mapper.Map<UserDTO>(userViewModel);
            var userCreated = await _userService.Create(userDTO);

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuário criado com sucesso!",
                Success = true,
                Data = userCreated
            });
        }

        [HttpPut]
        [Authorize]
        [Route("/api/v1/users/update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel userViewModel)
        {
            var userDTO = _mapper.Map<UserDTO>(userViewModel);
            var userUpdated = await _userService.Update(userDTO);

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuário atualizado com sucesso!",
                Success = true,
                Data = userUpdated
            });
        }

        [HttpDelete]
        [Authorize]
        [Route("/api/v1/users/remove/{id}")]
        public async Task<IActionResult> Remove(long id)
        {
            await _userService.Remove(id);

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
        public async Task<IActionResult> Get(long id)
        {
            var user = await _userService.Get(id);

            if (HasNotifications())
                return Result();

            if (user == null)
                return Ok(new ResultViewModel
                {
                    Message = "Nenhum usuário foi encontrado com o ID informado.",
                    Success = true,
                    Data = user
                });

            return Ok(new ResultViewModel
            {
                Message = "Usuário encontrado com sucesso!",
                Success = true,
                Data = user
            });
        }


        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/get-all")]
        public async Task<IActionResult> Get()
        {
            var allUsers = await _userService.Get();

            if (HasNotifications())
                return Result();

            return Ok(new ResultViewModel
            {
                Message = "Usuários encontrados com sucesso!",
                Success = true,
                Data = allUsers
            });
        }


        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/get-by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = await _userService.GetByEmail(email);

            if (HasNotifications())
                return Result();

            if (user == null)
                return Ok(new ResultViewModel
                {
                    Message = "Nenhum usuário foi encontrado com o email informado.",
                    Success = true,
                    Data = user
                });

            return Ok(new ResultViewModel
            {
                Message = "Usuário encontrado com sucesso!",
                Success = true,
                Data = user
            });
        }

        [HttpGet]
        [Authorize]
        [Route("/api/v1/users/search-by-name")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var allUsers = await _userService.SearchByName(name);

            if (HasNotifications())
                return Result();

            if (allUsers.Count == 0)
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
        public async Task<IActionResult> SearchByEmail([FromQuery] string email)
        {
            var allUsers = await _userService.SearchByEmail(email);

            if (HasNotifications())
                return Result();

            if (allUsers.Count == 0)
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