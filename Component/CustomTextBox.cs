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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySimpleLauncher.Component {

    public enum ImeMode { Disabled, Hiragana, Off }

    public class CustomTextBox : TextBox {

        #region Declaration
        public event EventHandler TextValueChanged;
        private string _text = "";
        #endregion

        #region Public Property
        // IME の設定の種類
        public ImeMode ImeMode { get; set; } = ImeMode.Off;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomTextBox() {
            // 初期化
            this.Initialized += (sender, e) => {
                switch (this.ImeMode) {
                    case ImeMode.Disabled:      
                        InputMethod.SetIsInputMethodEnabled(this, false);
                        break;
                    case ImeMode.Hiragana:       
                        InputMethod.SetPreferredImeState(this, InputMethodState.On);
                        InputMethod.SetPreferredImeConversionMode(this, ImeConversionModeValues.FullShape | ImeConversionModeValues.Native);
                        break;
                    case ImeMode.Off:  
                        //InputMethod.SetPreferredImeState(this, InputMethodState.On);
                        //InputMethod.SetPreferredImeConversionMode(this, ImeConversionModeValues.Alphanumeric);
                        InputMethod.SetPreferredImeState(this, InputMethodState.Off);
                        break;
                }

            };

            this.GotFocus += CustomTextBox_GotFocus;
            this.LostFocus += CustomTextBox_LostFocus;
        }
        #endregion

        #region Event
        private void CustomTextBox_GotFocus(object sender, RoutedEventArgs e) {
            this._text = this.Text;
        }

        private void CustomTextBox_LostFocus(object sender, RoutedEventArgs e) {
            if (this._text != this.Text) {
                if (null != this.TextValueChanged) {
                    this.TextValueChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion
    }
}
