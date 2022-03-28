namespace AlertsApp
{
    partial class AlertsApp
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
            this.groupBoxAlerts = new System.Windows.Forms.GroupBox();
            this.btnRemoveAlert = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.lbValues = new System.Windows.Forms.Label();
            this.lbType = new System.Windows.Forms.Label();
            this.lbParameter = new System.Windows.Forms.Label();
            this.btnCreateAlert = new System.Windows.Forms.Button();
            this.cbEnable = new System.Windows.Forms.CheckBox();
            this.lblAnd = new System.Windows.Forms.Label();
            this.txtValue2 = new System.Windows.Forms.TextBox();
            this.btnUpdateAlert = new System.Windows.Forms.Button();
            this.cbParameter = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.txtAlertsLog = new System.Windows.Forms.RichTextBox();
            this.tpAlerts = new System.Windows.Forms.TabPage();
            this.lbAlertConditions = new System.Windows.Forms.ListBox();
            this.tcAlertsLog = new System.Windows.Forms.TabControl();
            this.btnBroker = new System.Windows.Forms.Button();
            this.groupBoxAlerts.SuspendLayout();
            this.tpLog.SuspendLayout();
            this.tpAlerts.SuspendLayout();
            this.tcAlertsLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxAlerts
            // 
            this.groupBoxAlerts.Controls.Add(this.btnBroker);
            this.groupBoxAlerts.Controls.Add(this.btnRemoveAlert);
            this.groupBoxAlerts.Controls.Add(this.lbMsg);
            this.groupBoxAlerts.Controls.Add(this.txtMessage);
            this.groupBoxAlerts.Controls.Add(this.lbValues);
            this.groupBoxAlerts.Controls.Add(this.lbType);
            this.groupBoxAlerts.Controls.Add(this.lbParameter);
            this.groupBoxAlerts.Controls.Add(this.btnCreateAlert);
            this.groupBoxAlerts.Controls.Add(this.cbEnable);
            this.groupBoxAlerts.Controls.Add(this.lblAnd);
            this.groupBoxAlerts.Controls.Add(this.txtValue2);
            this.groupBoxAlerts.Controls.Add(this.btnUpdateAlert);
            this.groupBoxAlerts.Controls.Add(this.cbParameter);
            this.groupBoxAlerts.Controls.Add(this.cbType);
            this.groupBoxAlerts.Controls.Add(this.txtValue);
            this.groupBoxAlerts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxAlerts.Location = new System.Drawing.Point(16, 15);
            this.groupBoxAlerts.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxAlerts.Name = "groupBoxAlerts";
            this.groupBoxAlerts.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxAlerts.Size = new System.Drawing.Size(1035, 327);
            this.groupBoxAlerts.TabIndex = 0;
            this.groupBoxAlerts.TabStop = false;
            this.groupBoxAlerts.Text = "Define Alerts";
            // 
            // btnRemoveAlert
            // 
            this.btnRemoveAlert.Enabled = false;
            this.btnRemoveAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveAlert.Location = new System.Drawing.Point(728, 66);
            this.btnRemoveAlert.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveAlert.Name = "btnRemoveAlert";
            this.btnRemoveAlert.Size = new System.Drawing.Size(303, 48);
            this.btnRemoveAlert.TabIndex = 13;
            this.btnRemoveAlert.Text = "Remove Alert";
            this.btnRemoveAlert.UseVisualStyleBackColor = true;
            this.btnRemoveAlert.Click += new System.EventHandler(this.btnRemoveAlert_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMsg.Location = new System.Drawing.Point(8, 232);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(138, 18);
            this.lbMsg.TabIndex = 12;
            this.lbMsg.Text = "Message (Optional)";
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(11, 253);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1020, 67);
            this.txtMessage.TabIndex = 11;
            this.txtMessage.Text = "";
            // 
            // lbValues
            // 
            this.lbValues.AutoSize = true;
            this.lbValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbValues.Location = new System.Drawing.Point(188, 140);
            this.lbValues.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbValues.Name = "lbValues";
            this.lbValues.Size = new System.Drawing.Size(52, 18);
            this.lbValues.TabIndex = 10;
            this.lbValues.Text = "Values";
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbType.Location = new System.Drawing.Point(8, 140);
            this.lbType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(40, 18);
            this.lbType.TabIndex = 9;
            this.lbType.Text = "Type";
            // 
            // lbParameter
            // 
            this.lbParameter.AutoSize = true;
            this.lbParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbParameter.Location = new System.Drawing.Point(8, 65);
            this.lbParameter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbParameter.Name = "lbParameter";
            this.lbParameter.Size = new System.Drawing.Size(77, 18);
            this.lbParameter.TabIndex = 8;
            this.lbParameter.Text = "Parameter";
            // 
            // btnCreateAlert
            // 
            this.btnCreateAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateAlert.Location = new System.Drawing.Point(728, 178);
            this.btnCreateAlert.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreateAlert.Name = "btnCreateAlert";
            this.btnCreateAlert.Size = new System.Drawing.Size(303, 48);
            this.btnCreateAlert.TabIndex = 7;
            this.btnCreateAlert.Text = "Create Alert";
            this.btnCreateAlert.UseVisualStyleBackColor = true;
            this.btnCreateAlert.Click += new System.EventHandler(this.btnCreateAlert_Click);
            // 
            // cbEnable
            // 
            this.cbEnable.AutoSize = true;
            this.cbEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEnable.Location = new System.Drawing.Point(11, 192);
            this.cbEnable.Margin = new System.Windows.Forms.Padding(4);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new System.Drawing.Size(75, 22);
            this.cbEnable.TabIndex = 6;
            this.cbEnable.Text = "Enable";
            this.cbEnable.UseVisualStyleBackColor = true;
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnd.Location = new System.Drawing.Point(455, 140);
            this.lblAnd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(32, 18);
            this.lblAnd.TabIndex = 5;
            this.lblAnd.Text = "and";
            // 
            // txtValue2
            // 
            this.txtValue2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue2.Location = new System.Drawing.Point(500, 137);
            this.txtValue2.Margin = new System.Windows.Forms.Padding(4);
            this.txtValue2.Name = "txtValue2";
            this.txtValue2.Size = new System.Drawing.Size(185, 24);
            this.txtValue2.TabIndex = 4;
            // 
            // btnUpdateAlert
            // 
            this.btnUpdateAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateAlert.Location = new System.Drawing.Point(728, 122);
            this.btnUpdateAlert.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateAlert.Name = "btnUpdateAlert";
            this.btnUpdateAlert.Size = new System.Drawing.Size(303, 48);
            this.btnUpdateAlert.TabIndex = 3;
            this.btnUpdateAlert.Text = "Update Alert";
            this.btnUpdateAlert.UseVisualStyleBackColor = true;
            this.btnUpdateAlert.Click += new System.EventHandler(this.btnUpdateAlert_Click);
            // 
            // cbParameter
            // 
            this.cbParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbParameter.FormattingEnabled = true;
            this.cbParameter.Location = new System.Drawing.Point(103, 59);
            this.cbParameter.Margin = new System.Windows.Forms.Padding(4);
            this.cbParameter.Name = "cbParameter";
            this.cbParameter.Size = new System.Drawing.Size(271, 26);
            this.cbParameter.TabIndex = 2;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(60, 134);
            this.cbType.Margin = new System.Windows.Forms.Padding(4);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(119, 26);
            this.cbType.TabIndex = 1;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // txtValue
            // 
            this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Location = new System.Drawing.Point(260, 137);
            this.txtValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(185, 24);
            this.txtValue.TabIndex = 0;
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.txtAlertsLog);
            this.tpLog.Location = new System.Drawing.Point(4, 25);
            this.tpLog.Margin = new System.Windows.Forms.Padding(4);
            this.tpLog.Name = "tpLog";
            this.tpLog.Padding = new System.Windows.Forms.Padding(4);
            this.tpLog.Size = new System.Drawing.Size(1027, 284);
            this.tpLog.TabIndex = 1;
            this.tpLog.Text = "Alerts Log";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // txtAlertsLog
            // 
            this.txtAlertsLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlertsLog.Enabled = false;
            this.txtAlertsLog.Location = new System.Drawing.Point(4, 4);
            this.txtAlertsLog.Name = "txtAlertsLog";
            this.txtAlertsLog.Size = new System.Drawing.Size(1019, 276);
            this.txtAlertsLog.TabIndex = 0;
            this.txtAlertsLog.Text = "";
            // 
            // tpAlerts
            // 
            this.tpAlerts.Controls.Add(this.lbAlertConditions);
            this.tpAlerts.Location = new System.Drawing.Point(4, 25);
            this.tpAlerts.Margin = new System.Windows.Forms.Padding(4);
            this.tpAlerts.Name = "tpAlerts";
            this.tpAlerts.Padding = new System.Windows.Forms.Padding(4);
            this.tpAlerts.Size = new System.Drawing.Size(1027, 284);
            this.tpAlerts.TabIndex = 0;
            this.tpAlerts.Text = "Alert Condictions";
            this.tpAlerts.UseVisualStyleBackColor = true;
            // 
            // lbAlertConditions
            // 
            this.lbAlertConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAlertConditions.FormattingEnabled = true;
            this.lbAlertConditions.ItemHeight = 16;
            this.lbAlertConditions.Location = new System.Drawing.Point(4, 4);
            this.lbAlertConditions.Name = "lbAlertConditions";
            this.lbAlertConditions.Size = new System.Drawing.Size(1019, 276);
            this.lbAlertConditions.TabIndex = 0;
            this.lbAlertConditions.SelectedIndexChanged += new System.EventHandler(this.lbAlertConditions_SelectedIndexChanged);
            // 
            // tcAlertsLog
            // 
            this.tcAlertsLog.Controls.Add(this.tpAlerts);
            this.tcAlertsLog.Controls.Add(this.tpLog);
            this.tcAlertsLog.Location = new System.Drawing.Point(16, 350);
            this.tcAlertsLog.Margin = new System.Windows.Forms.Padding(4);
            this.tcAlertsLog.Name = "tcAlertsLog";
            this.tcAlertsLog.SelectedIndex = 0;
            this.tcAlertsLog.Size = new System.Drawing.Size(1035, 313);
            this.tcAlertsLog.TabIndex = 7;
            // 
            // btnBroker
            // 
            this.btnBroker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBroker.Location = new System.Drawing.Point(728, 22);
            this.btnBroker.Name = "btnBroker";
            this.btnBroker.Size = new System.Drawing.Size(299, 37);
            this.btnBroker.TabIndex = 14;
            this.btnBroker.Text = "Reconnect to Broker";
            this.btnBroker.UseVisualStyleBackColor = true;
            this.btnBroker.Visible = false;
            this.btnBroker.Click += new System.EventHandler(this.btnBroker_Click);
            // 
            // AlertsApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 676);
            this.Controls.Add(this.tcAlertsLog);
            this.Controls.Add(this.groupBoxAlerts);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AlertsApp";
            this.Text = "AlertsApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlertsApp_FormClosing);
            this.Load += new System.EventHandler(this.AlertsApp_Load);
            this.groupBoxAlerts.ResumeLayout(false);
            this.groupBoxAlerts.PerformLayout();
            this.tpLog.ResumeLayout(false);
            this.tpAlerts.ResumeLayout(false);
            this.tcAlertsLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxAlerts;
        private System.Windows.Forms.Button btnUpdateAlert;
        private System.Windows.Forms.ComboBox cbParameter;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblAnd;
        private System.Windows.Forms.TextBox txtValue2;
        private System.Windows.Forms.CheckBox cbEnable;
        private System.Windows.Forms.Button btnCreateAlert;
        private System.Windows.Forms.Label lbValues;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Label lbParameter;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.TabPage tpAlerts;
        private System.Windows.Forms.ListBox lbAlertConditions;
        private System.Windows.Forms.TabControl tcAlertsLog;
        private System.Windows.Forms.Button btnRemoveAlert;
        private System.Windows.Forms.RichTextBox txtAlertsLog;
        private System.Windows.Forms.Button btnBroker;
    }
}

