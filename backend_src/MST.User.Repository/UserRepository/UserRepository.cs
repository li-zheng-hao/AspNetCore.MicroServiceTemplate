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
    // [RegisterService(ServiceType = typeof(IUserRepository))]
    public class UserRepository : CurBaseRepository<Users,long>, IUserRepository
    {
        public UserRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}