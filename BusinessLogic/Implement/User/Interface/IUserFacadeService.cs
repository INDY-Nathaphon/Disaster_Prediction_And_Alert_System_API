using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Model.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Model.User;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface
{
    public interface IUserFacadeService : IBaseService<UserInfo, BaseFilter>
    {
        public Task<UserInfo> GetUserByMobileNo(string mobileNo);
    }
}
