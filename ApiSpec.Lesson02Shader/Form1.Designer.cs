﻿namespace ApiSpec.Lesson02Shader {
    partial class Form1 {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.ucShader1 = new ApiSpec.Lesson02Shader.UCShader();
            this.SuspendLayout();
            // 
            // ucShader1
            // 
            this.ucShader1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucShader1.BackColor = System.Drawing.Color.Red;
            this.ucShader1.Location = new System.Drawing.Point(12, 12);
            this.ucShader1.Name = "ucShader1";
            this.ucShader1.Size = new System.Drawing.Size(776, 426);
            this.ucShader1.TabIndex = 0;
            this.ucShader1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ucShader1_KeyUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ucShader1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private UCShader ucShader1;
    }
}

