using SHDocVw; //Microsoft Internet COntrols
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Web;

namespace OpenBrowserUrls.Core.Utils
{
    public class BrowserOpen
    {
        public void StartInNewWindow(string path, List<string> windowsUrls)
        {
            if (path.ToLower().Contains("chrome"))
            {
                OpenNewChromeWindow(path, windowsUrls);
            }
            else if (path.ToLower().Contains("iexplore"))
            {
                OpenNewIEWindow(path, windowsUrls);
            }
            else if (path.ToLower().Contains("firefox"))
            {
                OpenNewFirefoxWindow(path, windowsUrls);
            }
            else if (path.ToLower().Contains("opera"))
            {
                OpenNewOperaWindow(path, windowsUrls);
            }
            else if (path.ToLower().Contains("microsoft-edge"))
            {
                OpenNewEdgeWindow(path, windowsUrls);
            }
        }

        public void StartInNewPrivateWindow(string path, List<string> windowsUrls)
        {
            if (path.ToLower().Contains("chrome"))
            {
                OpenNewChromeWindowIncognito(path, windowsUrls);
            }
            else if (path.ToLower().Contains("iexplore"))
            {
                OpenNewIEWindowPrivate(path, windowsUrls);
            }
            else if (path.ToLower().Contains("firefox"))
            {
                OpenNewFirefoxWindowPrivate(path, windowsUrls);
            }
            else if (path.ToLower().Contains("opera"))
            {
                OpenNewOperaWindowPrivate(path, windowsUrls);
            }
        }
        
        private void OpenNewChromeWindow(string path, List<string> urls)
        {
            Process.Start(path, JoinUrls(urls) + " --new-window").Dispose();
        }

        public void StartSingleUrl(string path, string url, bool shhh)
        {
            if (shhh)
            {
                if (path.ToLower().Contains("chrome"))
                {
                    OpenNewChromeIncognito(path, url);
                }
                else if (path.ToLower().Contains("iexplore"))
                {
                    OpenNewIEPrivate(path, url);
                }
                else if (path.ToLower().Contains("firefox"))
                {
                    OpenNewFirefoxPrivate(path, url);
                }
                else if (path.ToLower().Contains("opera"))
                {
                    OpenNewOperaPrivate(path, url);
                }              
            }
            else
            {
                if (path.ToLower().Contains("microsoft-edge"))
                {
                    Process.Start($"microsoft-edge:{url}");
                }
                else
                {
                    Process.Start(path, url).Dispose();
                }                
            }
        }

        private void OpenNewFirefoxWindow(string path, List<string> urls)
        {
            Process.Start(path, JoinUrls(urls)).Dispose();

            Thread.Sleep(500);
        }

        private string JoinUrls(List<string> urls)
        {
            return string.Join(" ", urls);
        }

        private void OpenNewIEWindow(string path, List<string> urls)
        {
            string script = GetIEPowerShellScript(urls);
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(script);
                PowerShellInstance.Invoke();
            }
        }

        private string GetIEPowerShellScript(List<string> urls)
        {
            string script = "$ie = New-Object -ComObject InternetExplorer.Application; $ie.Navigate2('" + urls[0] + "');";
            if (urls.Count > 1)
            {
                for (int i = 1; i < urls.Count; i++)
                {
                    script += "$ie.Navigate2('" + urls[i]+ "', 0x1000); ";
                }
            }
            //script += "$sw = '[DllImport(\"user32.dll\")] public static extern int ShowWindow(int hwnd, int nCmdShow);' ";
            //script += "$type = Add-Type -Name ShowWindow2 -MemberDefinition $sw -Language CSharpVersion3 -Namespace Utils -PassThru; ";
            //script += "$type::ShowWindow($ie.hwnd, 3); # 3 = maximize ";
            script += "$ie.Visible = $true;";
            return script;
        }

        private void OpenNewIETab(string url)
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("$navOpenInNewTab = 0x800;  $App = New-Object -ComObject shell.application; $IE = $App.Windows() | Select-Object -Last 1; $IE.navigate('" + url + "', $navOpenInNewTab); 'App', 'IE' | ForEach-Object {Remove-Variable $_ -Force}");
                PowerShellInstance.Invoke();
            }
        }

        private void OpenNewOperaWindow(string path, List<string> urls)
        {
            Process.Start(path, "-new-window  " + JoinUrls(urls)).Dispose();
        }

        private void OpenNewEdgeWindow(string path, List<string> urls)
        {
            foreach (var url in urls)
            {
                Process.Start($"microsoft-edge:{url}");
            }
        }

        private void OpenNewChromeWindowIncognito(string path, List<string> urls)
        {
            Process.Start(path, JoinUrls(urls) + " --new-window --incognito").Dispose();
        }

        private void OpenNewFirefoxWindowPrivate(string path, List<string> urls)
        {
            Process.Start(path, "-private-window " + JoinUrls(urls)).Dispose();

            Thread.Sleep(500);
        }

        private void OpenNewIEWindowPrivate(string path, List<string> urls)
        {
            Process.Start(path, "-private " + urls[0]).Dispose();

            for (int i = 1; i < urls.Count; i++)
            {
                OpenNewIETab(urls[i]);
            }

            Thread.Sleep(500);
        }

        private void OpenNewOperaWindowPrivate(string path, List<string> urls)
        {
            Process.Start(path, "-private " + JoinUrls(urls)).Dispose();
        }

        private void OpenNewChromeIncognito(string path, string url)
        {
            Process.Start(path, url + " --incognito").Dispose();
        }

        private void OpenNewFirefoxPrivate(string path, string url)
        {
            Process.Start(path, "-private-window " + url).Dispose();
        }

        private void OpenNewIEPrivate(string path, string url)
        {
            Process.Start(path, "-private " + url).Dispose();
        }

        private void OpenNewOperaPrivate(string path, string url)
        {
            Process.Start(path, "-private " + url).Dispose();
        }

        private void OpenNewEdgePrivate(string url) // not working
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript($@"C:\Windows\System32\cmd.exe /c start shell:AppsFolder\Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge -private {url}");
                PowerShellInstance.Invoke();
            }
        }
    }

    public enum IEBrowserNavConstants
    {
        OpenInNewWindow = 0x1,
        NoHistory = 0x2,
        NoReadFromCache = 0x4,
        NoWriteToCache = 0x8,
        AllowAutosearch = 0x10,
        BrowserBar = 0x20,
        Hyperlink = 0x40,
        EnforceRestricted = 0x80,
        NewWindowsManaged = 0x0100,
        UntrustedForDownload = 0x0200,
        TrustedForActiveX = 0x0400,
        OpenInNewTab = 0x0800,
        OpenInBackgroundTab = 0x1000,
        KeepWordWheelText = 0x2000,
        VirtualTab = 0x4000,
        BlockRedirectsXDomain = 0x8000,
        OpenNewForegroundTab = 0x10000
    };
}