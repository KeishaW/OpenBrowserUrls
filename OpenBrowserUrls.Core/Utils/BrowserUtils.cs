using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OpenBrowserUrls.Core.Utils
{
    public class BrowserUtils
    {
        public List<string> getListOfBrowsers()
        {            
            List<string> browsers = new List<string>();
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            {
                AddBrowserPathsFromRegistry(browsers, hklm);
            }

            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                AddBrowserPathsFromRegistry(browsers, hklm);
            }

            RegistryKey edgeKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Schemas");
            if (edgeKey != null)
            {
                browsers.Add("microsoft-edge:");
            }
            return browsers;
        }

        private  void AddBrowserPathsFromRegistry(List<string> browsers, RegistryKey hklm)
        {
            RegistryKey webClientsRootKey = hklm.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            if (webClientsRootKey != null)
            {
                foreach (var subKeyName in webClientsRootKey.GetSubKeyNames())
                {
                    if (webClientsRootKey.OpenSubKey(subKeyName) != null)
                    {
                        if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell") != null)
                        {
                            if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open") != null)
                            {
                                if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command") != null)
                                {
                                    string commandLineUri = (string)webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue(null);
                                    if (string.IsNullOrEmpty(commandLineUri))
                                        continue;
                                    commandLineUri = commandLineUri.Trim("\"".ToCharArray());

                                    if (!browsers.Contains(commandLineUri))
                                    {
                                        browsers.Add(commandLineUri);
                                    }                                    
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public List<string> getListOfBrowsersKeyNames()
        {
            List<string> browsers = new List<string>();
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            {
                AddBrowsersFromRegistry(browsers, hklm);
            }

            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                AddBrowsersFromRegistry(browsers, hklm);
            }

            RegistryKey edgeKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Schemas");
            if (edgeKey != null)
            {
                browsers.Add("MicrosoftEdge.exe");
            }
            
            return browsers;
        }

        private void AddBrowsersFromRegistry(List<string> browsers, RegistryKey hklm)
        {
            RegistryKey webClientsRootKey = hklm.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            if (webClientsRootKey != null)
            {
                foreach (var subKeyName in webClientsRootKey.GetSubKeyNames())
                {
                    if (!browsers.Contains(subKeyName))
                    {
                        browsers.Add(subKeyName);
                    }
                }
            }
        }
    }
}