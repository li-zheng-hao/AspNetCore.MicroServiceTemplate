using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using MST.User.Contract;
using MST.User.Contract.IRepository;
using MST.User.Model;
using Quickwire.Attributes;

namespace MST.User.Repository.UserRepository
{
    /// <summary>
    /// 仓储模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [RegisterService(ServiceLifetime.Scoped,ServiceType = typeof(IUserRepository))]
    public class UserRepository : DefaultRepository<Users,long>, IUserRepository
    {
        public UserRepository(UnitOfWorkManager uowm) : base(uowm?.Orm,uowm)
        {
        }
    }
}