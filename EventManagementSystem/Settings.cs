using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Properties
{
    internal sealed partial class Settings
    {
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        [global::System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)]
        public global::System.Collections.Specialized.NameValueCollection CalendarEventTypes
        {
            get
            {
                return ((global::System.Collections.Specialized.NameValueCollection)(this["CalendarEventTypes"]));
            }
            set
            {
                this["CalendarEventTypes"] = value;
            }
        }
    }
}
