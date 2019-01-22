using System.Collections.Generic;

namespace OpenBrowserUrls.Core.Models
{
    public class BrowserModel
    {
        public string Path { get; set; }
        public List<List<string>> WindowsUrls { get; set; }
        public TabType TabType { get; set; }
    }

    public enum TabType
    {
        CurrentTab,
        SelectedTabs,
        CurrentWindow,
        AllWindows
    }
}