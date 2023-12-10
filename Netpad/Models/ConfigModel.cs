using dotPad.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotPad.Models
{
    public class ConfigModel : BaseViewModel
    {
        private string _font;
        public string FontFamily
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }

        private double _fontsize;
        public double FontSize
        {
            get
            {
                return _fontsize;
            }
            set
            {
                _fontsize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
    }
}
