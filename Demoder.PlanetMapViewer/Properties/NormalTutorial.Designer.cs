﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demoder.PlanetMapViewer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class NormalTutorial : global::System.Configuration.ApplicationSettingsBase {
        
        private static NormalTutorial defaultInstance = ((NormalTutorial)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new NormalTutorial())));
        
        public static NormalTutorial Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ZoomIn {
            get {
                return ((bool)(this["ZoomIn"]));
            }
            set {
                this["ZoomIn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ZoomOut {
            get {
                return ((bool)(this["ZoomOut"]));
            }
            set {
                this["ZoomOut"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool OverlayMode {
            get {
                return ((bool)(this["OverlayMode"]));
            }
            set {
                this["OverlayMode"] = value;
            }
        }
    }
}