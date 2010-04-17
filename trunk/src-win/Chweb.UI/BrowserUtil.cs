using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Chweb.UI.Properties;

namespace Chweb.UI
{
    public class BrowserUtil
    {
        /// <summary>
        /// When looking for installed browsers, ignore self.
        /// </summary>
        private const string ignoreBrowser = "Chweb";

        /// <summary>
        /// Get a collection of browsers from the registry.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BrowserInfo> GetBrowsers()
        {
            // all installed browsers should exist here
            var smi = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Clients\StartMenuInternet");

            // get browser info for each key except for Chweb
            var browsers =
                from key in smi.GetSubKeyNames()
                where key != ignoreBrowser
                select getBrowserInfo(smi, key);

            return browsers.OrderBy(
                b => b.FileName != Settings.Default.FirstBrowser);
        }

        /// <summary>
        /// Get browser info from key.
        /// </summary>
        private BrowserInfo getBrowserInfo(RegistryKey smi, string key)
        {
            // each browser has it's path in this sub key...
            var open = smi.OpenSubKey(key + @"\shell\open\command");

            // ... the path is always the default value...
            string path = (string)open.GetValue(string.Empty);

            // ... and it sometimes has quotes!
            return new BrowserInfo(path.Trim('"'));
        }
    }
}
