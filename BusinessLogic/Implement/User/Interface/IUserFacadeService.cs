using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Base;
using Disaster_Prediction_And_Alert_System_API.Model;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface
{
    public interface IUserFacadeService : IBaseService<UserInfo>
    {
        public Task<UserInfo> GetUserByMobileNo(string mobileNo);
    }
}
