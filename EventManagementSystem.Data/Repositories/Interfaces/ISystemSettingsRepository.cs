using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using System.Linq.Expressions;
using System;

namespace EventManagementSystem.Data.Repositories.Interfaces
{
    public interface ISystemSettingsRepository
    {
        Task<List<SystemSetting>> GetAllAsync();
        Task<List<SystemSetting>> GetAllAsync(Expression<Func<SystemSetting, bool>> expression);

        Task<SystemSetting> GetSettingByName(string settingName);
        void Add(SystemSetting entity);
        void Delete(SystemSetting entity);
        void Refresh();
    }
}
