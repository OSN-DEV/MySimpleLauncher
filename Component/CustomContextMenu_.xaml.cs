using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MySimpleLauncher.Component {
    /// <summary>
    /// CustomContextMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomContextMenu_ : Window {

        // https://stackoverflow.com/questions/3193452/programmatically-add-style-trigger
        // https://stackoverflow.com/questions/3199424/how-to-set-the-style-programmatically

        #region Declaration
        public class Item {
            public int Id { set; get; }
            public string Text { set; get; }
            public string ForeGround { set; get; } = "#333333";
            public string MouseOverColor { set; get; } = "#515151";
            public string PressedColor { set; get; } = "#151515";
            //public bool IsSeparator { set; get; }
            public RoutedEventHandler Click { set; get; }
        }
        Dictionary<int, Button> _menuItems = new Dictionary<int, Button>();

        private StringBuilder _buttonStyle = new StringBuilder();

        public string MenuFontName { set; get; } = "Meiryo UI";
        public double MenuFontSize { set; get; } = 11;
        public string MenuHighlightColor { set; get; } = "#EEE";
        public Brush SeparatorColor { set; get; } = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xDD));
        #endregion

        #region Constructor
        public CustomContextMenu_() {
            InitializeComponent();
            this.SetButtonStyle();
        }

        public CustomContextMenu_(Window owner, MouseButtonEventArgs e) : this() {
            var point = owner.PointToScreen(e.GetPosition(owner));
            this.Left = point.X;
            this.Top = point.Y;
        }
        #endregion

        #region Event
        private void Window_Deactivated(object sender, EventArgs e) {
            this.Hide();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            var item = ((Button)sender).Tag as Item;
            if (null != item.Click) {
                item.Click(sender, e);
                this.Hide();
            }
        }
        #endregion

        #region Public Method
        public void ShowMenu(Window owner, MouseButtonEventArgs e) {
            if (this.IsEnabled) {
                var point = owner.PointToScreen(e.GetPosition(owner));
                this.Left = point.X;
                this.Top = point.Y;
                this.Show();
            }
        }

        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="item">item</param>
        public void AddItem(Item item) {
            var menuItem = new Button();
            menuItem.Style = (Style)System.Windows.Markup.XamlReader.Parse(
                _buttonStyle.ToString().Replace("@ForeGround@", item.ForeGround)
                                       .Replace("@MouseOverColor@", item.MouseOverColor)
                                       .Replace("@PressedColor@", item.PressedColor));
            menuItem.Content = item.Text;
            menuItem.Click += MenuItem_Click;
            menuItem.Tag = item;
            this._menuItems.Add(item.Id, menuItem);

            this.cContainer.Children.Add(menuItem);
        }

        /// <summary>
        /// Add separator
        /// </summary>
        public void AddSeparator() {
            var panel = new StackPanel();
            panel.Height = 1;
            panel.Margin = new Thickness(5, 2, 5, 2);
            panel.Background = this.SeparatorColor;
            this.cContainer.Children.Add(panel);
        }

        /// <summary>
        /// Set menu item enabled
        /// </summary>
        /// <param name="id">item id</param>
        /// <param name="enabled">enabled</param>
        public void SetEnabled(int id, bool enabled) {
            if (this._menuItems.ContainsKey(id)) {
                var menuItem = this._menuItems[id];
                menuItem.IsEnabled = enabled;
                menuItem.Opacity = enabled ? 1.0 : 0.1;
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Create style as string
        /// </summary>
        private void SetButtonStyle() {
            //_buttonStyle
            //    .AppendLine(@"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""Button"">")
            //    .AppendFormat(@"    <Setter Property=""FontFamily"" Value=""{0}"" />", this.MenuFontName)
            //    .AppendFormat(@"    <Setter Property=""FontSize"" Value=""{0}"" />", this.MenuFontSize)
            //    .AppendLine(@"    <Setter Property=""Foreground"" Value=""@ForeGround@"" />")
            //    .AppendLine(@"    <Setter Property=""Background"" Value=""#FFF"" />")
            //    //.AppendLine(@"    <Setter Property=""Margin"" Value=""10,5"" />")
            //    .AppendLine(@"    <Setter Property=""Template"">")
            //    .AppendLine("        <Setter.Value>")
            //    .AppendLine(@"            <ControlTemplate TargetType=""Button"">")
            //    .AppendLine(@"                <TextBlock  Text=""{TemplateBinding Content}""")
            //    .AppendLine(@"                            Foreground=""{TemplateBinding Foreground}""")
            //    .AppendLine(@"                            Background=""{TemplateBinding Background}""")
            //    .AppendLine(@"                            Padding=""10,5""")
            //    .AppendLine(@"                            TextWrapping=""Wrap"" />")
            //    .AppendLine("            </ControlTemplate>")
            //    .AppendLine("        </Setter.Value>")
            //    .AppendLine("    </Setter>")
            //    .AppendLine("    <Style.Triggers>")
            //    .AppendLine(@"        <Trigger Property=""IsMouseOver"" Value=""True"">")
            //    .AppendLine(@"            <Setter Property=""Cursor"" Value=""Hand"" />")
            //    .AppendLine(@"            <Setter Property=""Foreground"" Value=""@MouseOverColor@"" />")
            //    .AppendFormat(@"            <Setter Property=""Background"" Value=""{0}"" />", this.MenuHighlightColor)
            //    .AppendLine("        </Trigger>")
            //    .AppendLine(@"        <Trigger Property=""IsPressed"" Value=""True"">")
            //    .AppendLine(@"            <Setter Property=""Cursor"" Value=""Hand"" />")
            //    .AppendLine(@"            <Setter Property=""Foreground"" Value=""@PressedColor@"" />")
            //    .AppendFormat(@"            <Setter Property=""Background"" Value=""{0}"" />", this.MenuHighlightColor)
            //    .AppendLine("        </Trigger>")
            //    .AppendLine("    </Style.Triggers>")
            //    .AppendLine("</Style>");
            _buttonStyle
                .AppendLine(@"<Style xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" TargetType=""MenuItem"">")
                .AppendFormat(@"    <Setter Property=""FontFamily"" Value=""{0}"" />", this.MenuFontName)
                .AppendFormat(@"    <Setter Property=""FontSize"" Value=""{0}"" />", this.MenuFontSize)
                .AppendLine(@"    <Setter Property=""Foreground"" Value=""@ForeGround@"" />")
                .AppendLine(@"    <Setter Property=""Background"" Value=""#FFF"" />")
                //.AppendLine(@"    <Setter Property=""Margin"" Value=""10,5"" />")
                .AppendLine(@"    <Setter Property=""Template"">")
                .AppendLine("        <Setter.Value>")
                .AppendLine(@"            <ControlTemplate TargetType=""MenuItem"">")
                .AppendLine(@"                <TextBlock  Text=""{TemplateBinding Content}""")
                .AppendLine(@"                            Foreground=""{TemplateBinding Foreground}""")
                .AppendLine(@"                            Background=""{TemplateBinding Background}""")
                .AppendLine(@"                            Padding=""10,5""")
                .AppendLine(@"                            TextWrapping=""Wrap"" />")
                .AppendLine("            </ControlTemplate>")
                .AppendLine("        </Setter.Value>")
                .AppendLine("    </Setter>")
                .AppendLine("    <Style.Triggers>")
                .AppendLine(@"        <Trigger Property=""IsMouseOver"" Value=""True"">")
                .AppendLine(@"            <Setter Property=""Cursor"" Value=""Hand"" />")
                .AppendLine(@"            <Setter Property=""Foreground"" Value=""@MouseOverColor@"" />")
                .AppendFormat(@"            <Setter Property=""Background"" Value=""{0}"" />", this.MenuHighlightColor)
                .AppendLine("        </Trigger>")
                .AppendLine(@"        <Trigger Property=""IsPressed"" Value=""True"">")
                .AppendLine(@"            <Setter Property=""Cursor"" Value=""Hand"" />")
                .AppendLine(@"            <Setter Property=""Foreground"" Value=""@PressedColor@"" />")
                .AppendFormat(@"            <Setter Property=""Background"" Value=""{0}"" />", this.MenuHighlightColor)
                .AppendLine("        </Trigger>")
                .AppendLine("    </Style.Triggers>")
                .AppendLine("</Style>");
        }
        #endregion
    }
}
