using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Constant;
using Disaster_Prediction_And_Alert_System_API.Common.Extensions;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Region;
using Disaster_Prediction_And_Alert_System_API.Common.Validation;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.Region.Service
{
    public class RegionService : IRegionService
    {
        private readonly AppDBContext _db;

        public RegionService(AppDBContext db)
        {
            _db = db;
        }
        public async Task<RegionInfo> GetEntityById(long id)
        {
            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "Region id is invalid.");
            }

            var region = await (from r in _db.Regions
                                where r.Id == id
                                select new RegionInfo
                                {
                                    Id = r.Id,
                                    Name = r.Name,
                                    Latitude = r.Latitude,
                                    Longitude = r.Longitude,
                                    CreatedDate = r.CreatedDate,
                                    UpdatedDate = r.UpdatedDate
                                }).FirstOrDefaultAsync();

            if (region == null)
            {
                throw new AppException(AppErrorCode.NotFound, "Region not found.");
            }

            return region;
        }

        public async Task<PagedResult<RegionInfo>> GetEntities(BaseFilter filter)
        {
            FilterValidator.Validate(filter);

            var query = (from r in _db.Regions
                         select new RegionInfo
                         {
                             Id = r.Id,
                             Name = r.Name,
                             Latitude = r.Latitude,
                             Longitude = r.Longitude,
                             CreatedDate = r.CreatedDate,
                             UpdatedDate = r.UpdatedDate
                         });

            var pagedResult = await query.ToPagedResultAsync(filter);

            return pagedResult;
        }

        public async Task<RegionInfo> Create(RegionInfo info)
        {
            if (info == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "Region info is null.");
            }

            var exists = await _db.Regions.AnyAsync(r => r.Longitude == info.Longitude && r.Latitude == info.Latitude);

            if (exists)
            {
                throw new AppException(AppErrorCode.ValidationError, "Region already exists.");
            }

            var currentDate = DateTime.UtcNow;

            var newRegion = new Domain.Entities.Region
            {
                Name = info.Name,
                Latitude = info.Latitude,
                Longitude = info.Longitude,
                CreatedDate = currentDate,
                UpdatedDate = currentDate
            };

            await _db.Regions.AddAsync(newRegion);
            await _db.SaveChangesAsync();

            var result = await (from r in _db.Regions
                                where r.Id == newRegion.Id
                                select new RegionInfo
                                {
                                    Id = r.Id,
                                    Name = r.Name,
                                    Latitude = r.Latitude,
                                    Longitude = r.Longitude,
                                    CreatedDate = r.CreatedDate,
                                    UpdatedDate = r.UpdatedDate
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new AppException(AppErrorCode.NotFound, "Region not found.");
            }

            return result;
        }

        public async Task<RegionInfo> Update(long id, RegionInfo info)
        {
            if (info == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "Region info is null.");
            }

            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "Region id is invalid.");
            }

            var regionEntity = await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionEntity == null)
            {
                throw new AppException(AppErrorCode.NotFound, "Region not found.");
            }

            var exists = await _db.Regions.AnyAsync(r => r.Longitude == info.Longitude && r.Latitude == info.Latitude && r.Id != id);

            if (exists)
            {
                throw new AppException(AppErrorCode.NotFound, "Region already exists.");
            }

            regionEntity.Name = info.Name;
            regionEntity.Latitude = info.Latitude;
            regionEntity.Longitude = info.Longitude;
            regionEntity.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var result = await (from r in _db.Regions
                                where r.Id == regionEntity.Id
                                select new RegionInfo
                                {
                                    Id = r.Id,
                                    Name = r.Name,
                                    Latitude = r.Latitude,
                                    Longitude = r.Longitude,
                                    CreatedDate = r.CreatedDate,
                                    UpdatedDate = r.UpdatedDate
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new AppException(AppErrorCode.NotFound, "Region not found.");
            }

            return result;
        }

        public async Task Delete(long id)
        {
            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "Region id is invalid.");
            }

            var region = await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (region == null)
            {
                throw new AppException(AppErrorCode.NotFound, "Region not found.");
            }

            _db.Regions.Remove(region);
            await _db.SaveChangesAsync();
        }
    }

}
