﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuildIndicatron.Console {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class AppSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static AppSettings defaultInstance = ((AppSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AppSettings())));
        
        public static AppSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("17")]
        public int FeetRedPin {
            get {
                return ((int)(this["FeetRedPin"]));
            }
            set {
                this["FeetRedPin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("24")]
        public int FeetGreenPin {
            get {
                return ((int)(this["FeetGreenPin"]));
            }
            set {
                this["FeetGreenPin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("27")]
        public int LsRedPin {
            get {
                return ((int)(this["LsRedPin"]));
            }
            set {
                this["LsRedPin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9")]
        public int LsGreenPin {
            get {
                return ((int)(this["LsGreenPin"]));
            }
            set {
                this["LsGreenPin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11")]
        public int LsBluePin {
            get {
                return ((int)(this["LsBluePin"]));
            }
            set {
                this["LsBluePin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int ButtonPin {
            get {
                return ((int)(this["ButtonPin"]));
            }
            set {
                this["ButtonPin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("45")]
        public int PassiveInterval {
            get {
                return ((int)(this["PassiveInterval"]));
            }
            set {
                this["PassiveInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7")]
        public int PassiveStartHour {
            get {
                return ((int)(this["PassiveStartHour"]));
            }
            set {
                this["PassiveStartHour"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int PassiveStopHour {
            get {
                return ((int)(this["PassiveStopHour"]));
            }
            set {
                this["PassiveStopHour"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://192.168.2.210:5000/")]
        public string Host {
            get {
                return ((string)(this["Host"]));
            }
            set {
                this["Host"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("zapzap:zap zap|dev :development|WP8:zapper windows phone|DEV -:|Build - :|Deploy " +
            "- :|")]
        public string StringReplaces {
            get {
                return ((string)(this["StringReplaces"]));
            }
            set {
                this["StringReplaces"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Build - Zapper WP8|Build - Zapper WP8 (Release)|Build - ZapZap WP8|Build - ZapZap" +
            " WP8 (Release)")]
        public string CoreProjects {
            get {
                return ((string)(this["CoreProjects"]));
            }
            set {
                this["CoreProjects"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://fulliautomatix:8080/")]
        public string JenkenServer {
            get {
                return ((string)(this["JenkenServer"]));
            }
            set {
                this["JenkenServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Rolf")]
        public string JenkenUsername {
            get {
                return ((string)(this["JenkenUsername"]));
            }
            set {
                this["JenkenUsername"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAEsCG8iuH30eperrCkQ78wwAAAAACAAAAAAADZgAAwAAAABAAA" +
            "AA7MJf55VFW3deOBYPTkhwRAAAAAASAAACgAAAAEAAAAGMgQsDiCt5clb9gyCekpXMQAAAA5oHinm+LO" +
            "HZ4pXhdmcNM5BQAAAD2OdiREScGsIIYqABogNnJlbRw5Q==\r\n")]
        public string JenkenPassword {
            get {
                return ((string)(this["JenkenPassword"]));
            }
            set {
                this["JenkenPassword"] = value;
            }
        }
    }
}
