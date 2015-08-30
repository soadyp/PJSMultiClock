using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;

namespace PJSCore
{
    public class PjsConfig : PJSCore.IPjsConfig
    {
        private MachineKeySection _machineKeySection;
        private Configuration _cfg;


        // Get encryption and decryption key information from the configuration.  //open the web.config at app level
        public Configuration GetConfig()
        {
            if (_cfg == null)
            {
                if (HostingEnvironment.IsHosted) // running inside asp.net ?
                { //yes so read web.config at hosting virtual path level
                    _cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
                }
                else
                { //no, se we are testing or running exe version admin tool for example, look for an APP.CONFIG file
                    //var x = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    _cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
            }
            return _cfg;
        }

        public AppSettingsSection GetAppSettings()
        {
            var cfg = GetConfig();
            return cfg == null ? null
                               : cfg.AppSettings;
        }

        public string GetAppSetting(string key)
        {
            // null ref here means key missing.  
            var cfg = GetConfig();
            if (cfg == null) return null;
            var appSettings = cfg.AppSettings;
            if (appSettings == null){
                return null;
            }

            var stg = GetConfig().AppSettings.Settings[key];
            return stg?.Value;
        }

        public bool SaveAppSetting(string key, string value)
        {
            var cfg = GetConfig();
            if (cfg == null) return false;
            var appSettings = cfg.AppSettings;
            if (appSettings.Settings[key] == null)
            {
                 appSettings.Settings.Add(key, value);
            }
            else
            {
                appSettings.Settings[key].Value = value;
            }
           


            cfg.Save(ConfigurationSaveMode.Modified);
            //relaod the section you modified
            ConfigurationManager.RefreshSection(cfg.AppSettings.SectionInformation.Name);
            return true;
        }


        public ConnectionStringSettingsCollection GetConnectionStringsCollection(Configuration configuration) { return configuration.ConnectionStrings.ConnectionStrings; }
        public ConnectionStringSettingsCollection GetConnectionStrings() { return GetConfig().ConnectionStrings.ConnectionStrings; }

        public ConnectionStringSettings GetConnectionString(Configuration configuration, string connectionName) { return GetConnectionStringsCollection(configuration)[connectionName]; }
        public ConnectionStringSettings GetConnectionString(string connectionName) { return GetConnectionStrings()[connectionName]; }

        public string GetConnectionProviderName(string connectionName) { return GetConnectionString(connectionName).ProviderName; }

        public void AddConnectionString(Configuration configuration, ConnectionStringSettings connectionStringSettings)
        {
            var coll = GetConnectionStringsCollection(configuration);
            coll.Add(connectionStringSettings);
            configuration.Save(ConfigurationSaveMode.Modified);
        }

        public void AddConnectionString(ConnectionStringSettings connectionStringSettings)
        {
            var config = GetConfig();
            var coll = GetConnectionStringsCollection(config);
            coll.Add(connectionStringSettings);
            config.Save(ConfigurationSaveMode.Modified);
        }

        public void UpdateConnectionString(Configuration configuration,
                                           ConnectionStringSettings connectionStringSettings)
        {
            var coll = GetConnectionStringsCollection(configuration);
            coll.Remove(connectionStringSettings);
            coll.Add(connectionStringSettings);
            configuration.Save(ConfigurationSaveMode.Modified);
        }

        public ServiceModelSectionGroup GetServiceModelSectionGroup()
        {
            var cfg = GetConfig();

            ServiceModelSectionGroup serviceModelSection = ServiceModelSectionGroup.GetSectionGroup(cfg);

            return serviceModelSection;
        }

        public MachineKeySection GetMachineKeySection()
        {
            // Get encryption and decryption key information from the configuration.  //open the web.config at app level
            var cfg = GetConfig();
            if (cfg != null)
            {
                _machineKeySection = (MachineKeySection)cfg.GetSection("system.web/machineKey");
            }
            return _machineKeySection;
        }



        public string GetValidationKey(bool withoutIsolateAppsString = true) //"HEXSTRING,IsolateApss
        {
            if (GetMachineKeySection() == null)
            {
                return null;
            }
            return withoutIsolateAppsString
                       ? RemoveIsolateAppsFromString(GetMachineKeySection().ValidationKey)
                       : GetMachineKeySection().ValidationKey;
        }

        public string GetDecryptionKey(bool withoutIsolateAppsString = true) //"HEXSTRING,IsolateApss
        {
            if (GetMachineKeySection() == null)
            {
                return null;
            }
            return withoutIsolateAppsString
                       ? RemoveIsolateAppsFromString(GetMachineKeySection().DecryptionKey)
                       : GetMachineKeySection().DecryptionKey;
        }


        private string RemoveIsolateAppsFromString(string sourceMachineKeyString)
        {
            return sourceMachineKeyString.Replace(@",IsolateApps", ""); // remove the ,isolateApps at the end of Key Value
        }
    }
}
