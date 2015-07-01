using FlexibleProxy.Settings;

namespace FlexibleProxy.Repositories
{
    public interface ISettingsFileRepository
    {
        void SaveSettings(ProxySettings settings);

        ProxySettings LoadSettings();
    }
}