using System;
using System.IO;
using FlexibleProxy.Settings;
using Newtonsoft.Json;

namespace FlexibleProxy.Repositories
{
    public class SettingsFileRepository : ISettingsFileRepository
    {
        private const string SettingsFileName = "FlexibleProxySettings.config";

        public void SaveSettings(ProxySettings settings)
        {
            try
            {
                var serializedSettings = JsonConvert.SerializeObject(settings);
                var stream = File.CreateText(SettingsFileName);
                stream.Write(serializedSettings);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Can't save settings to file " + SettingsFileName, ex);
            }
        }

        public ProxySettings LoadSettings()
        {
            ProxySettings settings = null;
            try
            {
                var fileStream = File.OpenText(SettingsFileName);
                var serializedSettings = fileStream.ReadToEnd();
                fileStream.Close();
                settings = JsonConvert.DeserializeObject<ProxySettings>(serializedSettings);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Can't read settings from file " + SettingsFileName, ex);
            }
            return settings;
        }
    }
}
