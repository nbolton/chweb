﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4200
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chweb.Launch.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Chweb.Launch.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;registry&gt;
        ///  &lt;key name=&quot;HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\{1}&quot;&gt;
        ///    &lt;value&gt;{1}&lt;/value&gt;
        ///  &lt;/key&gt;
        ///  &lt;key name=&quot;HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\{1}\Capabilities&quot;&gt;
        ///    &lt;value name=&quot;ApplicationName&quot;&gt;{1}&lt;/value&gt;
        ///    &lt;value name=&quot;ApplicationIcon&quot;&gt;{0},0&lt;/value&gt;
        ///    &lt;value name=&quot;ApplicationDescription&quot;&gt;{1}&lt;/value&gt;
        ///  &lt;/key&gt;
        ///  &lt;key name=&quot;HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\{1}\Capabilities\FileAssociations&quot;&gt;
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RegistryXml {
            get {
                return ResourceManager.GetString("RegistryXml", resourceCulture);
            }
        }
    }
}