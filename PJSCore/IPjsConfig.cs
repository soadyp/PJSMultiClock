using System.Configuration;
using System.ServiceModel.Configuration;
using System.Web.Configuration;

namespace PJSCore
{
    public interface IPjsConfig
    {
        ServiceModelSectionGroup GetServiceModelSectionGroup();
        MachineKeySection GetMachineKeySection();
        string GetValidationKey(bool withoutIsolateAppsString = true); //"HEXSTRING,IsolateApss
        string GetDecryptionKey(bool withoutIsolateAppsString = true); //"HEXSTRING,IsolateApss
        /// <summary>
        /// Get encryption and decryption key information from the configuration.  //open the web.config or app.config
        /// </summary>
        /// <returns>The configuration in use </returns>
        Configuration GetConfig();

        AppSettingsSection GetAppSettings();
        string GetAppSetting(string key);
        bool SaveAppSetting(string key, string value);
    }
}