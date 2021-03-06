﻿using System;
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
using MySimpleLauncher.Model;


namespace MySimpleLauncher.UI {
    /// <summary>
    /// アイテム削除確認
    /// </summary>
    public partial class ItemDeleteConfirm : Window {
        #region Declaration
        public bool? DeleteFile {
            get { return this.cDeleteFile.IsChecked; }
        }
        #endregion

        #region Constructor
        private ItemDeleteConfirm() {
            InitializeComponent();
        }

        internal ItemDeleteConfirm(Window owner, ItemModel model) : this() {
            this.Owner = owner;
            this.cConfirmMessage.Text = string.Format("[{0}]を削除しますか？", model.DisplayName);
            this.cFilePath.Text = model.FilePath;
        }
        #endregion

        #region Event
        private void cOK_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        #endregion
    }
}
