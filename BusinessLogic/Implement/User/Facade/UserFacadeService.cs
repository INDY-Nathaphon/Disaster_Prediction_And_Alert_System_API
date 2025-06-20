using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.TransactionManager;
using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Model.User;

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

        public async Task<UserInfo> GetEntityById(long id)
        {
            return await _transactionManager.DoworkWithNoTransaction(() =>
            _userService.GetEntityById(id));
        }
        public Task<UserInfo> GetUserByMobileNo(string mobileNo)
        {
            return _transactionManager.DoworkWithNoTransaction(() =>
            _userService.GetUserByMobileNo(mobileNo));
        }
        public Task<PagedResult<UserInfo>> GetEntities(BaseFilter filter)
        {
            return _transactionManager.DoworkWithNoTransaction(() =>
            _userService.GetEntities(filter));
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
