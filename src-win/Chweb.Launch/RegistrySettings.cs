using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows;

namespace Chweb.Launch
{
    public class RegistrySettings
    {
        private string xml;
        private string exe;
        private string name;

        public RegistrySettings(string xml, string exe, string name)
        {
            this.xml = xml;
            this.exe = exe;
            this.name = name;
        }

        /// <summary>
        /// Installs in the registry the settings.
        /// </summary>
        public void Apply()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            // for each element (named "key") ensure the key path exists
            foreach (XmlElement element in document.DocumentElement)
            {
                Debug.Assert(element.Name == "key");

                string keyPathFmt = element.GetAttribute("name");
                string keyPath = string.Format(
                    keyPathFmt, exe, name);

                RegistryKey baseKey = getBaseKey(keyPath);
                string keyPathExBase = getKeyPathExBase(keyPath);
                RegistryKey key = baseKey.CreateSubKey(keyPathExBase);

                foreach (XmlElement value in element.ChildNodes)
                {
                    string valueName = value.GetAttribute("name");
                    string valueType = value.GetAttribute("type");
                    object valueObject;

                    string valueSourceFmt = value.InnerText;
                    string valueSource = string.Format(
                        valueSourceFmt, exe, name);

                    switch (valueType)
                    {
                        case "dword":
                            valueObject = int.Parse(valueSource);
                            break;

                        default:
                            valueObject = valueSource;
                            break;
                    }

                    key.SetValue(valueName, valueObject);
                }
            }
        }

        private string getKeyPathExBase(string keyPath)
        {
            var keyPathExBase =
                from key in keyPath.Split('\\')
                where !key.StartsWith("HKEY")
                select key;

            return string.Join("\\", keyPathExBase.ToArray());
        }

        /// <summary>
        /// Takes the first part of a full key path and returns a reference
        /// to the usable base key object.
        /// </summary>
        public RegistryKey getBaseKey(string key)
        {
            string hkLong = key.Split('\\')[0];
            switch (hkLong)
            {
                case "HKEY_LOCAL_MACHINE":
                    return Registry.LocalMachine;

                case "HKEY_CURRENT_USER":
                    return Registry.CurrentUser;

                case "HKEY_CLASSES_ROOT":
                    return Registry.ClassesRoot;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
