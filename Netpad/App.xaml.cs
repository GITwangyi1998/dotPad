using dotPad.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;

namespace dotPad
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 配置实体
        /// </summary>
        public static ConfigModel Config { get; set; }

        /// <summary>
        /// 配置文件地址
        /// </summary>
        public static string ConfigFilePath { get; set; }

        /// <summary>
        /// 配置文件夹
        /// </summary>
        public static string AppFolder { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dotPad");
            ConfigFilePath = Path.Combine(AppFolder, "config.json");
            if (!Directory.Exists(AppFolder))
            {
                Directory.CreateDirectory(AppFolder);
                //参数默认值
                File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(new ConfigModel()
                {
                    FontFamily = "仿宋",
                    FontSize = 14,
                    IsWrap = true,
                }));              
            }

            // 读取配置信息
            Config = ReadConfig(ConfigFilePath);

            if (e.Args.Length > 0)
            {
                MainWindow mainWindow = new MainWindow(e.Args[0]);
                mainWindow.Show();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        static ConfigModel ReadConfig(string filePath)
        {
            ConfigModel config = new ConfigModel();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                config = JsonConvert.DeserializeObject<ConfigModel>(json);
            }

            return config;
        }

        public static void WriteConfig()
        {
            if (Config != null)
            {
                string json = JsonConvert.SerializeObject(Config);
                File.WriteAllText(ConfigFilePath, json);
            }
        }

        /// <summary>
        /// 设置帧数
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
            typeof(Timeline),
            new FrameworkPropertyMetadata { DefaultValue = 120 }
            );
        }
    }
}
