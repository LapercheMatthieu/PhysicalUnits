using MatthL.PhysicalUnits.Infrastructure.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Infrastructure.Repositories
{
    /// <summary>
    /// The settings for the filtering of the repository 
    /// 
    /// </summary>
    public class RepositorySettings
    {
        private bool _initialized = false;
        public bool ShowMetrics { get; set; } = true;
        public bool ShowImperial { get; set; } = false;
        public bool ShowUS { get; set; } = false;
        public bool ShowAstronomic { get; set; } = false;
        public bool ShowOther { get; set; } = true;

        private string SettingsPath =>
            Path.Combine(Path.GetTempPath(),
                "MatthL", "PhysicalUnits", "settings.json");

        public void Initialize()
        {
            if (_initialized) return;

      /*      _settings = StorageSettings.Load();
            ShowMetrics = _settings.ShowMetrics;
            ShowImperial = _settings.ShowImperial;
            // etc.

            PhysicalUnitLibrary.LoadAll(AvailableUnits);*/
            _initialized = true;
        }

        public void SaveSettings()
        {
   /*         _settings.ShowMetrics = ShowMetrics;
            _settings.ShowImperial = ShowImperial;
            // etc.
            _settings.Save();*/
        }

        public RepositorySettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize<RepositorySettings>(json)
                           ?? new RepositorySettings();
                }
            }
            catch { /* Log error */ }

            return new RepositorySettings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(SettingsPath, json);
            }
            catch { /* Log error */ }
        }
    }
}
