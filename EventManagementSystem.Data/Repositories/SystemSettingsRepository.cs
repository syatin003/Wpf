using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories.Base;
using EventManagementSystem.Data.Repositories.Interfaces;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace EventManagementSystem.Data.Repositories
{
    public class SystemSettingsRepository : EntitiesRepository<SystemSetting>, ISystemSettingsRepository
    {
        private readonly EmsEntities _objectContext;

        public SystemSettingsRepository(EmsEntities objectContext)
            : base(objectContext)
        {
            _objectContext = objectContext;
        }

        public async Task<List<SystemSetting>> GetAllAsync()
        {
            return await _objectContext.SystemSettings.ToListAsync();
        }

        public async Task<List<SystemSetting>> GetAllAsync(Expression<Func<SystemSetting, bool>> expression)
        {
            return await _objectContext.SystemSettings
                .Where(expression).ToListAsync();
        }

        public async Task<SystemSetting> GetSettingByName(string settingName)
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.SystemSettings);
            return await _objectContext.SystemSettings.FirstOrDefaultAsync(setting => setting.Name == settingName);
        }

        public override void Add(SystemSetting entity)
        {
            _objectContext.SystemSettings.AddObject(entity);
        }

        public override void Delete(SystemSetting entity)
        {
            _objectContext.SystemSettings.DeleteObject(entity);
        }

        public void Refresh()
        {
            _objectContext.Refresh(RefreshMode.StoreWins, _objectContext.SystemSettings);
        }
    }
}
