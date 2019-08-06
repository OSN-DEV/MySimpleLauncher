namespace MyKeyChangerForDebug {
    partial class TaskTrayMenu {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskTrayMenu));
            this.cNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cWatch = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenu.SuspendLayout();
            // 
            // cNotify
            // 
            this.cNotify.ContextMenuStrip = this.cMenu;
            this.cNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("cNotify.Icon")));
            this.cNotify.Text = "KeyChangeForDebug";
            this.cNotify.Visible = true;
            // 
            // cMenu
            // 
            this.cMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cMenu.Name = "cMenu";
            this.cMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Checked = true;
            this.toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // cWatch
            // 
            this.cWatch.Checked = true;
            this.cWatch.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cWatch.Name = "cWatch";
            this.cWatch.Size = new System.Drawing.Size(108, 22);
            this.cWatch.Text = "Watch";
            this.cMenu.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip cMenu;
        internal System.Windows.Forms.NotifyIcon cNotify;
        private System.Windows.Forms.ToolStripMenuItem cWatch;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
