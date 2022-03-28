namespace BinaryTranslator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sendData = new System.Windows.Forms.Button();
            this.keepSending = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // sendData
            // 
            this.sendData.Location = new System.Drawing.Point(12, 12);
            this.sendData.Name = "sendData";
            this.sendData.Size = new System.Drawing.Size(107, 23);
            this.sendData.TabIndex = 0;
            this.sendData.Text = "Send Data";
            this.sendData.UseVisualStyleBackColor = true;
            this.sendData.Click += new System.EventHandler(this.sendData_Click);
            // 
            // keepSending
            // 
            this.keepSending.AutoSize = true;
            this.keepSending.Location = new System.Drawing.Point(163, 14);
            this.keepSending.Name = "keepSending";
            this.keepSending.Size = new System.Drawing.Size(119, 21);
            this.keepSending.TabIndex = 1;
            this.keepSending.Text = "Keep Sending";
            this.keepSending.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 47);
            this.Controls.Add(this.keepSending);
            this.Controls.Add(this.sendData);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendData;
        private System.Windows.Forms.CheckBox keepSending;
    }
}

