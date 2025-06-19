using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface;
using Disaster_Prediction_And_Alert_System_API.Domain.Model;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Facade
{
    public class UserFacadeService : IUserFacadeService
    {
        private readonly IUserService _userService;
        private readonly ITransactionManagerService _transactionManager;

        public UserFacadeService(
            IUserService userService,
            ITransactionManagerService transactionManager)
        {
            _userService = userService;
            _transactionManager = transactionManager;
        }

        public async Task<UserInfo> GetById(long id)
        {
            return await _transactionManager.DoworkWithNoTransaction(() =>
            _userService.GetById(id));
        }
        public Task<UserInfo> GetUserByMobileNo(string mobileNo)
        {
            return _transactionManager.DoworkWithNoTransaction(() =>
            _userService.GetUserByMobileNo(mobileNo));
        }
        public Task<List<UserInfo>> GetAll()
        {
            return _transactionManager.DoworkWithNoTransaction(() =>
            _userService.GetAll());
        }
        public Task<UserInfo> Create(UserInfo info)
        {
            return _transactionManager.DoworkWithTransaction(() =>
            _userService.Create(info));
        }
        public Task<UserInfo> Update(long id, UserInfo info)
        {
            return _transactionManager.DoworkWithTransaction(() =>
            _userService.Update(id, info));
        }
        public async Task Delete(long id)
        {
            await _transactionManager.DoworkWithTransaction(() =>
            _userService.Delete(id));
        }
    }
}
