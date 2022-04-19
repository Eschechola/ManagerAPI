using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EscNet.Cryptography.Interfaces;
using EscNet.Hashers.Interfaces.Algorithms;
using Manager.Core.Communication.Mediator.Interfaces;
using Manager.Core.Communication.Messages.Notifications;
using Manager.Core.Enum;
using Manager.Core.Structs;
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
        private readonly IArgon2IdHasher _argon2IdHasher;
        private readonly IMediatorHandler _mediator;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository,
            IArgon2IdHasher argon2IdHasher, 
            IMediatorHandler mediator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _argon2IdHasher = argon2IdHasher;
            _mediator = mediator;
        }

        public async Task<Optional<UserDTO>> CreateAsync(UserDTO userDTO)
        {
            Expression<Func<User, bool>> filter = user 
                => user.Email.ToLower() == userDTO.Email.ToLower();

            var userExists = await _userRepository.GetAsync(filter);

            if (userExists != null)
            {
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                    ErrorMessages.UserAlreadyExists,
                    DomainNotificationType.UserAlreadyExists));

                return new Optional<UserDTO>();
            }   

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            if (!user.IsValid)
            {
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                   ErrorMessages.UserInvalid(user.ErrorsToString()),
                   DomainNotificationType.UserInvalid));

                return new Optional<UserDTO>();
            }

            user.SetPassword(_argon2IdHasher.Hash(user.Password));

            var userCreated = await _userRepository.CreateAsync(user);

            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<Optional<UserDTO>> UpdateAsync(UserDTO userDTO){
            var userExists = await _userRepository.GetAsync(userDTO.Id);

            if (userExists == null)
            {
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                   ErrorMessages.UserNotFound,
                   DomainNotificationType.UserNotFound));

                return new Optional<UserDTO>();
            }

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            if (!user.IsValid)
            {
                await _mediator.PublishDomainNotificationAsync(new DomainNotification(
                   ErrorMessages.UserInvalid(user.ErrorsToString()),
                   DomainNotificationType.UserInvalid));

                return new Optional<UserDTO>();
            }

            var sendedHashedPassword = _argon2IdHasher.Hash(user.Password);

            if(sendedHashedPassword != userExists.Password)
                user.SetPassword(sendedHashedPassword);

            var userUpdated = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }
        public async Task RemoveAsync(long id)
            => await _userRepository.RemoveAsync(id);

        public async Task<Optional<UserDTO>> GetAsync(long id){
            var user = await _userRepository.GetAsync(id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<Optional<IList<UserDTO>>> GetAllAsync(){
            var allUsers = await _userRepository.GetAllAsync();
            var allUsersDTO = _mapper.Map<IList<UserDTO>>(allUsers);

            return new Optional<IList<UserDTO>>(allUsersDTO);
        }

        public async Task<Optional<IList<UserDTO>>> SearchByNameAsync(string name){
            Expression<Func<User, bool>> filter = u 
                => u.Name.ToLower().Contains(name.ToLower());

            var allUsers = await _userRepository.SearchAsync(filter);
            var allUsersDTO = _mapper.Map<IList<UserDTO>>(allUsers);

            return new Optional<IList<UserDTO>>(allUsersDTO);
        }

        public async Task<Optional<IList<UserDTO>>> SearchByEmailAsync(string email){
            Expression<Func<User, bool>> filter = user
                => user.Email.ToLower().Contains(email.ToLower());

            var allUsers = await _userRepository.SearchAsync(filter);
            var allUsersDTO = _mapper.Map<IList<UserDTO>>(allUsers);

            return new Optional<IList<UserDTO>>(allUsersDTO);
        }

        public async Task<Optional<UserDTO>> GetByEmailAsync(string email){
            Expression<Func<User, bool>> filter = user
                => user.Email.ToLower() == email.ToLower();

            var user = await _userRepository.GetAsync(filter);
            var userDTO = _mapper.Map<UserDTO>(user);

            return new Optional<UserDTO>(userDTO);
        }
    }
}