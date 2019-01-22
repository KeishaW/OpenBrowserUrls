using OpenBrowserUrls.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBrowserUrls.Core.Utils
{
    public class BrowserManager
    {
        BrowserUtils bUtils = new BrowserUtils();
        BrowserOpen browserOpen = new BrowserOpen();

        public List<string> GetBrowserNames()
        {
            List<string> broswernames = new List<string>();
            List<string> browsers = bUtils.getListOfBrowsersKeyNames();

            foreach (var browser in browsers)
            {
                if (browser.ToLower().Contains("iexplore"))
                {
                    broswernames.Add("Internet Explorer");
                }
                else if (browser.ToLower().Contains("firefox"))
                {
                    broswernames.Add("Firefox");
                }
                else if (browser.ToLower().Contains("safari"))
                {
                    broswernames.Add("Safari");
                }
                else if (browser.ToLower().Contains("opera"))
                {
                    broswernames.Add("Opera");
                }
                else if (browser.ToLower().Contains("microsoftedge"))
                {
                    broswernames.Add("Microsoft Edge");
                }
                else if (browser.ToLower().Contains("chrome"))
                {
                    broswernames.Add("Google Chrome");
                }
            }

            return broswernames;
        }

        public List<string> GetPaths()
        {
            List<string> browsers = new List<string>();
            List<string> allBrowsers = bUtils.getListOfBrowsers();

            foreach (var browser in allBrowsers)
            {
                browsers.Add(browser);
            }

            return browsers;
        }

        public void OpenBrowser(BrowserModel model, bool isPrivate)
        {
            if (model != null)
            {
                string path = model.Path;

                ///TODO: open brand new browser windows
                switch (model.TabType)
                {
                    case TabType.CurrentTab:
                        browserOpen.StartSingleUrl(path, model.WindowsUrls[0][0], isPrivate);
                        break;
                    case TabType.SelectedTabs:
                    case TabType.CurrentWindow:
                        if (isPrivate)
                        {
                            browserOpen.StartInNewPrivateWindow(path, model.WindowsUrls[0]);
                        }
                        else
                        {
                            browserOpen.StartInNewWindow(path, model.WindowsUrls[0]);
                        }
                        break;
                    case TabType.AllWindows:
                        for (int i = 0; i < model.WindowsUrls.Count; i++)
                        {
                            if (isPrivate)
                            {
                                browserOpen.StartInNewPrivateWindow(path, model.WindowsUrls[i]);
                            }
                            else
                            {
                                //if (path.ToLower().Contains("microsoft-edge"))
                                //{
                                //    browserOpen.OpenNewEdgeWindow();
                                //}
                                browserOpen.StartInNewWindow(path, model.WindowsUrls[i]);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }


}
