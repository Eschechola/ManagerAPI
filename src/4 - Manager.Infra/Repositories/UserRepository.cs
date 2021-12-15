using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;

namespace Manager.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository{
        private readonly ManagerContext _context;

        public UserRepository(ManagerContext context) : base(context)
        {
            _context = context;
        }
    }
}