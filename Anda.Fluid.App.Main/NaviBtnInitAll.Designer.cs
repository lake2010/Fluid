﻿namespace Anda.Fluid.App.Main
{
    partial class NaviBtnInitAll
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInit
            // 
            this.btnInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInit.Image = global::Anda.Fluid.App.Main.Properties.Resources.Refresh_30px;
            this.btnInit.Location = new System.Drawing.Point(0, 0);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 50);
            this.btnInit.TabIndex = 27;
            this.btnInit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnInit.UseVisualStyleBackColor = true;
            // 
            // NaviBtnInitAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInit);
            this.Name = "NaviBtnInitAll";
            this.Size = new System.Drawing.Size(75, 50);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInit;
    }
}
