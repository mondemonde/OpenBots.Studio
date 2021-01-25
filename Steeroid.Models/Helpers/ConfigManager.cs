﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Steeroid.Models.Helpers
{
    public class ConfigManager
    {

        public static string MyConfigPath { get; set; }

        public ConfigManager(string FileNameOnly = "Custom.config")
        {
            //rgalvez: use custom appconfig

            var split = FileNameOnly.Split('\\');
            if (split.Length > 1)
            {
                FileNameOnly = split.Last();
            }

            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();

            //AppDomain.CurrentDomain.BaseDirectory + @"Logs";  //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            MyConfigPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory , FileNameOnly);
            configMap.ExeConfigFilename = MyConfigPath;//@"d:\test\justAConfigFile.config.whateverYouLikeExtension";

            if (!System.IO.File.Exists(MyConfigPath))
            {
                System.IO.File.CreateText(MyConfigPath);
                Configuration config1 = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                AppSettingsSection app = config1.AppSettings;
                app.Settings.Add("Title", "Test");
                config1.Save(ConfigurationSaveMode.Modified);

            }

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

        }

        private static Configuration _myConfig;
        public static Configuration myConfig
        {

            get
            {

                if (_myConfig == null)
                {
                    ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                    configMap.ExeConfigFilename = MyConfigPath;//@"d:\test\justAConfigFile.config.whateverYouLikeExtension";

                    Configuration config1 = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                    _myConfig = config1;
                }
                return _myConfig;
            }


            set { _myConfig = value; }
        }

        AppSettingsSection MySettings
        {
            get
            {
                return myConfig.AppSettings;
            }
        }



        public void Save()
        {
            // myConfig.Save();
            myConfig.Save(ConfigurationSaveMode.Modified);

        }



        public string GetValue(string key)
        {
            try
            {
                return MySettings.Settings[key].Value;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public void SetValue(string key, string value)
        {
            try
            {
                CheckValue(key, value);
                MySettings.Settings[key].Value = value;
                Save();


            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);

            }
        }

        public void CheckValue(string key, string value)
        {

            if (GetValue(key) == null)
            {
                //ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                //configMap.ExeConfigFilename = MyConfigPath;//@"d:\test\justAConfigFile.config.whateverYouLikeExtension";


                //Configuration config1 = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                //AppSettingsSection app = config1.AppSettings;
                //app.Settings.Add(key, value);
                //config1.Save(ConfigurationSaveMode.Modified);

                myConfig.AppSettings.Settings.Add(key, value);
                myConfig.Save(ConfigurationSaveMode.Modified);

            }
        }

    }

}
