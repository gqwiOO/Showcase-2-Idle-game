using System;
using System.Collections.Generic;
using System.Linq;
using Mechanics.DataSettings;

namespace Mechanics.CompaniesRating.Data
{
    [Serializable]
    public class CompaniesGeneratingSettings: ISettingsData
    {
        public int GeneratedCount;
        
        public List<string> Names;
        public List<string> ProductNames;
        
        public List<CompanySettingsByTier> CompanySettingsByTier;


        private Dictionary<CompanyTier, CompanySettingsByTier> _cachedCompanySettingsByTier;

        public IReadOnlyDictionary<CompanyTier, CompanySettingsByTier> CachedCompanySettingsByTier
        {
            get
            {
                if (_cachedCompanySettingsByTier != null)
                    return _cachedCompanySettingsByTier;
                else
                    return GenerateCachedCompanySettings();
            }
        }

        private IReadOnlyDictionary<CompanyTier, CompanySettingsByTier> GenerateCachedCompanySettings()
        {
            _cachedCompanySettingsByTier = CompanySettingsByTier.ToDictionary(item => item.CompanyTier);
            return _cachedCompanySettingsByTier;
        }
    }
}