﻿using Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Interface;
using Disaster_Prediction_And_Alert_System_API.Common.Constant;
using Disaster_Prediction_And_Alert_System_API.Common.Extensions;
using Disaster_Prediction_And_Alert_System_API.Common.Models.Base;
using Disaster_Prediction_And_Alert_System_API.Common.Models.User;
using Disaster_Prediction_And_Alert_System_API.Common.Validation;
using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Implement.User.Service
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _db;

        public UserService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<UserInfo> GetEntityById(long id)
        {
            #region Validate

            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "User id is invalid.");
            }

            #endregion

            var userInfo = await (from u in _db.Users
                                  where u.Id == id
                                  select new UserInfo
                                  {
                                      Id = u.Id,
                                      Mobile = u.Mobile,
                                      Email = u.Email,
                                      Name = u.Name,
                                      CreatedDate = u.CreatedDate,
                                      UpdatedDate = u.UpdatedDate
                                  }).FirstOrDefaultAsync();

            if (userInfo == null)
            {
                throw new AppException(AppErrorCode.NotFound, "User not found.");
            }

            return userInfo;
        }
        public async Task<UserInfo> GetUserByMobileNo(string mobileNo)
        {

            #region Validate

            if (string.IsNullOrEmpty(mobileNo))
            {
                throw new AppException(AppErrorCode.ValidationError, "User mobile number is required.");
            }

            #endregion

            var userInfo = await (from u in _db.Users
                                  where u.Mobile == mobileNo
                                  select new UserInfo
                                  {
                                      Id = u.Id,
                                      Mobile = u.Mobile,
                                      Email = u.Email,
                                      Name = u.Name,
                                      CreatedDate = u.CreatedDate,
                                      UpdatedDate = u.UpdatedDate
                                  }).FirstOrDefaultAsync();

            if (userInfo == null)
            {
                throw new AppException(AppErrorCode.NotFound, "User not found.");
            }

            return userInfo;
        }
        public async Task<PagedResult<UserInfo>> GetEntities(BaseFilter filter)
        {
            FilterValidator.Validate(filter);

            var userInfos = (from u in _db.Users
                             select new UserInfo
                             {
                                 Id = u.Id,
                                 Mobile = u.Mobile,
                                 Email = u.Email,
                                 Name = u.Name,
                                 CreatedDate = u.CreatedDate,
                                 UpdatedDate = u.UpdatedDate
                             });

            var result = await userInfos.ToPagedResultAsync(filter);

            return result;
        }
        public async Task<UserInfo> Create(UserInfo info)
        {
            if (info == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "User info is null.");
            }

            var currentExistingUser = await _db.Users.AnyAsync(u => u.Mobile == info.Mobile || u.Email == info.Email);

            if (currentExistingUser)
            {
                throw new AppException(AppErrorCode.ValidationError, "User already exists.");
            }

            var currentDate = DateTime.UtcNow;

            var newUser = new Domain.Entities.User
            {
                Mobile = info.Mobile,
                Email = info.Email,
                Name = info.Name,
                CreatedDate = currentDate,
                UpdatedDate = currentDate
            };

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();

            var result = await (from u in _db.Users
                                where u.Id == newUser.Id
                                select new UserInfo
                                {
                                    Id = u.Id,
                                    Mobile = u.Mobile,
                                    Email = u.Email,
                                    Name = u.Name,
                                    CreatedDate = u.CreatedDate,
                                    UpdatedDate = u.UpdatedDate
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new AppException(AppErrorCode.NotFound, "User not found.");
            }

            return result;
        }
        public async Task<UserInfo> Update(long id, UserInfo info)
        {
            #region Validate

            if (info == null)
            {
                throw new AppException(AppErrorCode.ValidationError, "User info is null.");
            }

            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "User id is invalid.");
            }

            #endregion

            var userEntity = await (from u in _db.Users
                                    where u.Id == id
                                    select u
                                    ).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                throw new AppException(AppErrorCode.NotFound, "User not found.");
            }

            userEntity.Mobile = info.Mobile;
            userEntity.Email = info.Email;
            userEntity.Name = info.Name;
            userEntity.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var result = await (from u in _db.Users
                                where u.Id == id
                                select new UserInfo
                                {
                                    Id = u.Id,
                                    Mobile = u.Mobile,
                                    Email = u.Email,
                                    Name = u.Name,
                                    CreatedDate = u.CreatedDate,
                                    UpdatedDate = u.UpdatedDate
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new AppException(AppErrorCode.NotFound, "User not found.");
            }

            return result;
        }
        public async Task Delete(long id)
        {
            #region Validate

            if (id <= 0)
            {
                throw new AppException(AppErrorCode.ValidationError, "User id is invalid.");
            }

            #endregion

            var userEntity = await (from u in _db.Users
                                    where u.Id == id
                                    select u
                                    ).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                throw new AppException(AppErrorCode.NotFound, "User not found.");
            }

            _db.Users.Remove(userEntity);
            await _db.SaveChangesAsync();
        }

        #region Private Methods

        public static void ValidateMobileNo(string mobileNo)
        {
            if (string.IsNullOrWhiteSpace(mobileNo))
            {
                throw new AppException(AppErrorCode.ValidationError, "User mobile number is required.");
            }

            if (!Regex.IsMatch(mobileNo, @"^(06|08|09)\d{8}$"))
            {
                throw new AppException(AppErrorCode.ValidationError, "User mobile number will start with 06, 08 or 09.");
            }
        }

        #endregion
    }
}
