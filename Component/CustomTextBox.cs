using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MySimpleLauncher.Component {
    #region Declaration
    internal enum ImeMode { Disabled, Hiragana, Off, DoNotCare }
    #endregion

    internal class CustomTextBox : TextBox {
        #region Declaration
        internal event EventHandler TextValueChanged;
        private string _text = "";
        #endregion

        #region Public Property
        // IME の設定の種類
        internal ImeMode ImeMode { get; set; } = ImeMode.DoNotCare;
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
                        InputMethod.SetPreferredImeState(this, InputMethodState.Off);
                        break;
                    default:
                        InputMethod.SetPreferredImeState(this, InputMethodState.DoNotCare);
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
            this.SelectAll();
        }

        private void CustomTextBox_LostFocus(object sender, RoutedEventArgs e) {
            if (this._text != this.Text) {
                if (null != this.TextValueChanged) {
                    TextValueChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion
    }
}
