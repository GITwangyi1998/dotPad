using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using dotPad.Command;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using dotPad.Views;
using Netpad.Utils;

namespace dotPad.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(string path, double size)
        {
            if (!string.IsNullOrEmpty(path)) 
            {
                CurrentFilePath = path;
                CurrentFileName = CurrentFilePath.Split('\\').Last();
            }
            if (File.Exists(CurrentFilePath))
            {
                CurrentText = File.ReadAllText(CurrentFilePath);
                CurrentFileName = CurrentFileName.Substring(1);
            }
            FontSize = size;
        }

        #region 属性-缩放百分比
        public string Percent
        {
            get
            { 
                return (FontSize / App.Config.FontSize * 100).ToString("F2")+"%";
            }
        }
        #endregion

        #region 属性-当前文件路径
        public string CurrentFilePath { get; set; }// = @"C:\Users\24265\Desktop\1.txt";
        #endregion

        #region 属性-当前文件名
        public string currentFileName;// = "1.txt";
        public string CurrentFileName
        {
            get
            {
                return currentFileName;
            }
            set
            {
                currentFileName = value;
                OnPropertyChanged(nameof(CurrentFileName));
            }
        }
        #endregion

        #region 属性-当前文本
        private string _currenttext;
        public string CurrentText 
        {
            get 
            {
                return _currenttext;
            }
            set 
            {
                _currenttext = value;
                if (!CurrentFileName.StartsWith("*"))
                {
                    CurrentFileName = "*" + CurrentFileName;
                }
                OnPropertyChanged(nameof(CurrentText));
                OnPropertyChanged(nameof(Count));
            }
        }
        #endregion

        #region 属性-字体大小
        //前台显示的字体大小，与配置文件的大小无关
        private double _fontSize;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
                OnPropertyChanged(nameof(Percent));
            }
        }
        #endregion

        #region 属性-字数
        public string Count
        {
            get
            {
                return CurrentText == null ? "" : CurrentText.Length.ToString()+"字";
            }
        }
        #endregion

        #region 命令-打开文件
        private ICommand _openCommand;

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new CustomCommand(ExecuteOpenCommand);
                }
                return _openCommand;
            }
        }

        private void ExecuteOpenCommand(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Select a text file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CurrentFilePath = openFileDialog.FileName;
                CurrentFileName = CurrentFilePath.Split('\\').Last();

                //CurrentText = File.ReadAllText(CurrentFilePath);
                CurrentText = Util.ReadTextFileWithEncoding(CurrentFilePath);
                CurrentFileName = CurrentFileName.Substring(1);
            }
        }
        #endregion

        #region 命令-保存
        private ICommand _saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new CustomCommand(ExecuteSaveCommand);
                }
                return _saveCommand;
            }
        }

        private void ExecuteSaveCommand(object o)
        {
            if (string.IsNullOrEmpty(CurrentFilePath))
            {
                return;
            }
            CurrentFileName = CurrentFileName.Substring(1);
            File.WriteAllText(CurrentFilePath, CurrentText);
        }
        #endregion

        #region 命令-打开设置窗口
        private ICommand _openSettingsDialogCommand;

        public ICommand OpenSettingsDialogCommand
        {
            get
            {
                if (_openSettingsDialogCommand == null)
                {
                    _openSettingsDialogCommand = new CustomCommand(ExecuteOpenSettingsDialogCommand);
                }
                return _openSettingsDialogCommand;
            }
        }

        private void ExecuteOpenSettingsDialogCommand(object o)
        {
            var opwin = new Option();
            opwin.ShowDialog();

            //设置前台的字体大小为用户的设置
            FontSize = App.Config.FontSize;
        }
        #endregion

        #region 函数-字体设置
        public void ChangeFontSize(object sender, RoutedEventArgs args)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (args is MouseWheelEventArgs eventArgs)
                {
                    double fontSizeChangeRatio = 0.05;
                    if (eventArgs.Delta > 0)
                    {
                        FontSize *= (1 + fontSizeChangeRatio);
                    }
                    else
                    {
                        FontSize *= (1 - fontSizeChangeRatio);
                        // 防止字体大小小于某个最小值（例如10）
                        if (FontSize < 10)
                        {
                            FontSize = 10;
                        }
                    }          
                }
            }
        }

        public void ResetFontSize(object sender, RoutedEventArgs args)
        {
            FontSize = App.Config.FontSize;
        }
        #endregion
    }
}
