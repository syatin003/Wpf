using System;
using Microsoft.Win32;

namespace EventManagementSystem.Services
{
    public static class ApplicationSettings
    {
        #region Fields

        private static readonly RegistryKey BaseRegistryKey = Registry.CurrentUser;
        private const string SubKey = "SOFTWARE\\EVENT MANAGEMENT SYSTEM\\SETTINGS";

        #endregion

        #region Constructor

        static ApplicationSettings()
        {
            CheckSubKey();
        }

        #endregion

        #region Methods

        private static void CheckSubKey()
        {
            var key = BaseRegistryKey.OpenSubKey(SubKey);

            if (key == null)
            {
                BaseRegistryKey.CreateSubKey(SubKey);
            }
        }

        public static object Read(string keyName)
        {
            RegistryKey baseRegistryKey = BaseRegistryKey;

            RegistryKey key = baseRegistryKey.OpenSubKey(SubKey);

            if (key != null)
            {
                try
                {
                    return key.GetValue(keyName.ToUpper());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("Subkey not found");
            }
        }

        public static void Write(string keyName, object Value)
        {
            RegistryKey rk = BaseRegistryKey;
            RegistryKey sk1 = rk.CreateSubKey(SubKey);

            sk1.SetValue(keyName.ToUpper(), Value);
        }

        #endregion
    }
}
