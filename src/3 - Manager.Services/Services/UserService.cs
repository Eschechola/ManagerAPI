using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EscNet.Cryptography.Interfaces;
using Manager.Core.Communication.Mediator.Interfaces;
using Manager.Core.Communication.Messages.Notifications;
using Manager.Core.Enum;
using Manager.Core.Validations.Message;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;

namespace Manager.Services.Services
{
    public class UserService : IUserService{
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRijndaelCryptography _rijndaelCryptography;
        private readonly IMediatorHandler _mediator;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository,
            IRijndaelCryptography rijndaelCryptography, 
            IMediatorHandler mediator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _rijndaelCryptography = rijndaelCryptography;
            _mediator = mediator;
        }

        public async Task<UserDTO> Create(UserDTO userDTO){
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if (userExists != null)
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                    ErrorMessages.UserAlreadyExists,
                    DomainNotificationType.UserAlreadyExists));

            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            
            if(!user.IsValid)
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                   ErrorMessages.UserInvalid(user.ErrorsToString()),
                   DomainNotificationType.UserInvalid));

            user.SetPassword(_rijndaelCryptography.Encrypt(user.Password));

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<UserDTO> Update(UserDTO userDTO){
            var userExists = await _userRepository.Get(userDTO.Id);

            if (userExists == null)
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                   ErrorMessages.UserNotFound,
                   DomainNotificationType.UserNotFound));

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            if(!user.IsValid)
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                   ErrorMessages.UserInvalid(user.ErrorsToString()),
                   DomainNotificationType.UserInvalid));

            user.SetPassword(_rijndaelCryptography.Encrypt(user.Password));

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }
        public async Task Remove(long id){
            await _userRepository.Remove(id);
        }

        public async Task<UserDTO> Get(long id){
            var user = await _userRepository.Get(id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> Get(){
            var allUsers = await _userRepository.Get();

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByName(string name){
            var allUsers = await _userRepository.SearchByName(name);

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByEmail(string email){
            var allUsers = await _userRepository.SearchByEmail(email);

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<UserDTO> GetByEmail(string email){
            var user = await _userRepository.GetByEmail(email);

            return _mapper.Map<UserDTO>(user);
        }
    }
}