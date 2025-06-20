using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Constant;
using Disaster_Prediction_And_Alert_System_API.Common.Extensions;
using Disaster_Prediction_And_Alert_System_API.Common.Models.AlertSetting;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Validation;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.DisasterPredictionAndAlert.Service
{
    public class AlertSettingService : IAlertSettingService
    {
        private readonly AppDBContext _db;

        public AlertSettingService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<AlertSettingInfo> GetEntityById(long id)
        {
            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting id is invalid.");
            }

            var info = await (from r in _db.AlertSettings
                              where r.Id == id
                              select new AlertSettingInfo
                              {
                                  Id = r.Id,
                                  CreatedDate = r.CreatedDate,
                                  UpdatedDate = r.UpdatedDate,
                                  DisasterType = r.DisasterType,
                                  RegionId = r.RegionId,
                                  Message = r.Message,
                                  ThresholdScore = r.ThresholdScore
                              }).FirstOrDefaultAsync();

            if (info == null)
            {
                throw new AppException(AppErrorCode.NotFound, "AlertSetting not found.");
            }

            return info;
        }

        public async Task<PagedResult<AlertSettingInfo>> GetEntities(BaseFilter filter)
        {
            FilterValidator.Validate(filter);

            var query = (from r in _db.AlertSettings
                         select new AlertSettingInfo
                         {
                             Id = r.Id,
                             CreatedDate = r.CreatedDate,
                             UpdatedDate = r.UpdatedDate,
                             DisasterType = r.DisasterType,
                             Message = r.Message,
                             RegionId = r.RegionId,
                             ThresholdScore = r.ThresholdScore
                         });

            var result = await query.ToPagedResultAsync(filter);

            return result;
        }

        public async Task<AlertSettingInfo> Create(AlertSettingInfo info)
        {
            if (info == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting info is null.");
            }

            var exists = await _db.AlertSettings.AnyAsync(r => r.DisasterType == info.DisasterType && r.RegionId == info.RegionId);

            if (exists)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting already exists.");
            }

            var currentDate = DateTime.UtcNow;

            var entity = new Domain.Entities.AlertSetting
            {
                DisasterType = info.DisasterType,
                RegionId = info.RegionId,
                ThresholdScore = info.ThresholdScore,
                Message = info.Message,
                CreatedDate = currentDate,
                UpdatedDate = currentDate
            };

            await _db.AlertSettings.AddAsync(entity);
            await _db.SaveChangesAsync();

            var result = await (from r in _db.AlertSettings
                                where r.Id == entity.Id
                                select new AlertSettingInfo
                                {
                                    Id = r.Id,
                                    CreatedDate = r.CreatedDate,
                                    UpdatedDate = r.UpdatedDate,
                                    DisasterType = r.DisasterType,
                                    RegionId = r.RegionId,
                                    Message = r.Message,
                                    ThresholdScore = r.ThresholdScore
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new AppException(AppErrorCode.NotFound, "AlertSetting not found.");
            }

            return result;
        }

        public async Task<AlertSettingInfo> Update(long id, AlertSettingInfo info)
        {
            if (info == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting info is null.");
            }

            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting info is null.");
            }

            var entity = await _db.AlertSettings.FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
            {
                throw new AppException(AppErrorCode.NotFound, "AlertSetting not found.");
            }

            var exists = await _db.AlertSettings.AnyAsync(r => r.DisasterType == info.DisasterType && r.RegionId == info.RegionId && r.Id != id);

            if (exists)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting already exists.");
            }

            entity.DisasterType = info.DisasterType;
            entity.RegionId = info.RegionId;
            entity.ThresholdScore = info.ThresholdScore;
            entity.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var result = await (from r in _db.AlertSettings
                                where r.Id == entity.Id
                                select new AlertSettingInfo
                                {
                                    Id = r.Id,
                                    CreatedDate = r.CreatedDate,
                                    UpdatedDate = r.UpdatedDate,
                                    DisasterType = r.DisasterType,
                                    Message = r.Message,
                                    RegionId = r.RegionId,
                                    ThresholdScore = r.ThresholdScore
                                }
                              ).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new AppException(AppErrorCode.NotFound, "AlertSetting not found.");
            }

            return result;
        }

        public async Task Delete(long id)
        {
            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "AlertSetting id is invalid.");
            }

            var entity = await _db.AlertSettings.FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
            {
                throw new AppException(AppErrorCode.NotFound, "AlertSetting not found.");
            }

            _db.AlertSettings.Remove(entity);
            await _db.SaveChangesAsync();
        }


    }
}
