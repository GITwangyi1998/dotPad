using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dotPad.Views
{
    /// <summary>
    /// Option.xaml 的交互逻辑
    /// </summary>
    public partial class Option : Window
    {
        public string fontFamily;

        public Option()
        {
            InitializeComponent();
            fontbox.SelectedItem = fontbox.Items.OfType<ComboBoxItem>().First(i => i.Content as string == App.Config.FontFamily);
            sizebox.Text = App.Config.FontSize.ToString();
            this.Closed += new EventHandler(OnClosed);
        }

        private void ChangeFont(object sender, SelectionChangedEventArgs e)
        {
            fontFamily = (fontbox.SelectedItem as ComboBoxItem).Content as string;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            App.Config.FontSize = double.Parse(sizebox.Text);
            App.Config.FontFamily = fontFamily;
            App.WriteConfig();
        }
    }
}
