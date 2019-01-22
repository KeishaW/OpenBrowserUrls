using Newtonsoft.Json;
using OpenBrowserUrls.Core.Models;
using OpenBrowserUrls.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBrowserUrls.App
{
    class Program
    {
        public static BrowserManager manager = new BrowserManager();

        static void Main(string[] args)
        {
            //Get Browser Names
            List<string> browserNames = manager.GetBrowserNames();

            //Get Browser Paths
            List<string> browserPaths = manager.GetPaths();

            //Open Browser Window  
            bool _private = false;

            BrowserModel singleTabModel = new BrowserModel
            {
                TabType = TabType.CurrentTab,
                Path = browserPaths.First(),
                WindowsUrls = new List<List<string>> { new List<string> { "https://www.github.com/" } }
            };
            manager.OpenBrowser(singleTabModel, _private);

            BrowserModel multiTabSingleWindowModel = new BrowserModel
            {
                TabType = TabType.SelectedTabs,
                Path = browserPaths.ElementAt(1),
                WindowsUrls = new List<List<string>> { new List<string> { "https://www.github.com/", "https://www.google.com/", "https://visualstudio.microsoft.com/", "https://azure.microsoft.com/en-us/blog/" } }
            };
            manager.OpenBrowser(multiTabSingleWindowModel, _private);

            BrowserModel multiTabSingleWindowModel1 = new BrowserModel
            {
                TabType = TabType.AllWindows,
                Path = browserPaths.First(),
                WindowsUrls = new List<List<string>>
                {
                    new List<string> { "https://www.google.com/", "https://www.github.com/", "https://visualstudio.microsoft.com/", "https://azure.microsoft.com/en-us/blog/", "https://medium.com/" },
                    new List<string> { "https://www.yahoo.com/", "https://www.youtube.com/", "https://codepen.io/" }
                }
            };
            manager.OpenBrowser(multiTabSingleWindowModel1, _private);

            var asm = Assembly.GetExecutingAssembly();
            var rdr = new StreamReader(asm.GetManifestResourceStream("OpenBrowserUrls.App.data.json"));
            var json = rdr.ReadToEnd();

            BrowserModel multiTabMultiWindowModel2 = JsonConvert.DeserializeObject<BrowserModel>(json);
            manager.OpenBrowser(multiTabMultiWindowModel2, _private);

            Console.ReadKey();
        }
    }
}
