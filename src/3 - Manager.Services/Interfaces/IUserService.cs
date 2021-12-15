using System.Threading.Tasks;
using System.Collections.Generic;
using Manager.Services.DTO;
using Manager.Core.Structs;

namespace Manager.Services.Interfaces{
    public interface IUserService{
        Task<Optional<UserDTO>> CreateAsync(UserDTO userDTO);
        Task<Optional<UserDTO>> UpdateAsync(UserDTO userDTO);
        Task RemoveAsync(long id);
        Task<Optional<UserDTO>> GetAsync(long id);
        Task<Optional<IList<UserDTO>>> GetAllAsync();
        Task<Optional<IList<UserDTO>>> SearchByNameAsync(string name);
        Task<Optional<IList<UserDTO>>> SearchByEmailAsync(string email);
        Task<Optional<UserDTO>> GetByEmailAsync(string email);
    }
}