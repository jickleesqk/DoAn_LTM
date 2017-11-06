namespace Client
{
    partial class MainForm
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
            this.client_mb = new System.Windows.Forms.TextBox();
            this.client_sendBtn = new System.Windows.Forms.Button();
            this.client_conversation_Lb = new System.Windows.Forms.ListBox();
            this.client_ip_tb = new System.Windows.Forms.TextBox();
            this.client_port_tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.client_connect_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.client_name_tb = new System.Windows.Forms.TextBox();
            this.client_selectFile_btn = new System.Windows.Forms.Button();
            this.client_file_status_lb = new System.Windows.Forms.Label();
            this.client_files = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // client_mb
            // 
            this.client_mb.Location = new System.Drawing.Point(23, 303);
            this.client_mb.Multiline = true;
            this.client_mb.Name = "client_mb";
            this.client_mb.Size = new System.Drawing.Size(293, 91);
            this.client_mb.TabIndex = 3;
            // 
            // client_sendBtn
            // 
            this.client_sendBtn.Location = new System.Drawing.Point(322, 354);
            this.client_sendBtn.Name = "client_sendBtn";
            this.client_sendBtn.Size = new System.Drawing.Size(97, 40);
            this.client_sendBtn.TabIndex = 4;
            this.client_sendBtn.Text = "Send";
            this.client_sendBtn.UseVisualStyleBackColor = true;
            this.client_sendBtn.Click += new System.EventHandler(this.client_sendBtn_Click);
            // 
            // client_conversation_Lb
            // 
            this.client_conversation_Lb.FormattingEnabled = true;
            this.client_conversation_Lb.Location = new System.Drawing.Point(23, 73);
            this.client_conversation_Lb.Name = "client_conversation_Lb";
            this.client_conversation_Lb.Size = new System.Drawing.Size(293, 212);
            this.client_conversation_Lb.TabIndex = 5;
            this.client_conversation_Lb.SelectedIndexChanged += new System.EventHandler(this.client_conversation_Lb_SelectedIndexChanged);
            // 
            // client_ip_tb
            // 
            this.client_ip_tb.Location = new System.Drawing.Point(84, 12);
            this.client_ip_tb.Name = "client_ip_tb";
            this.client_ip_tb.Size = new System.Drawing.Size(150, 20);
            this.client_ip_tb.TabIndex = 6;
            // 
            // client_port_tb
            // 
            this.client_port_tb.Location = new System.Drawing.Point(322, 12);
            this.client_port_tb.Name = "client_port_tb";
            this.client_port_tb.Size = new System.Drawing.Size(97, 20);
            this.client_port_tb.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server\'s IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Server\'s port";
            // 
            // client_connect_btn
            // 
            this.client_connect_btn.Location = new System.Drawing.Point(322, 46);
            this.client_connect_btn.Name = "client_connect_btn";
            this.client_connect_btn.Size = new System.Drawing.Size(97, 23);
            this.client_connect_btn.TabIndex = 10;
            this.client_connect_btn.Text = "Connect";
            this.client_connect_btn.UseVisualStyleBackColor = true;
            this.client_connect_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Username";
            // 
            // client_name_tb
            // 
            this.client_name_tb.Location = new System.Drawing.Point(84, 46);
            this.client_name_tb.Name = "client_name_tb";
            this.client_name_tb.Size = new System.Drawing.Size(232, 20);
            this.client_name_tb.TabIndex = 12;
            // 
            // client_selectFile_btn
            // 
            this.client_selectFile_btn.Location = new System.Drawing.Point(322, 303);
            this.client_selectFile_btn.Name = "client_selectFile_btn";
            this.client_selectFile_btn.Size = new System.Drawing.Size(97, 26);
            this.client_selectFile_btn.TabIndex = 14;
            this.client_selectFile_btn.Text = "Browse";
            this.client_selectFile_btn.UseVisualStyleBackColor = true;
            this.client_selectFile_btn.Click += new System.EventHandler(this.choose_file_btn_Click);
            // 
            // client_file_status_lb
            // 
            this.client_file_status_lb.AutoSize = true;
            this.client_file_status_lb.Location = new System.Drawing.Point(322, 306);
            this.client_file_status_lb.Name = "client_file_status_lb";
            this.client_file_status_lb.Size = new System.Drawing.Size(0, 13);
            this.client_file_status_lb.TabIndex = 15;
            // 
            // client_files
            // 
            this.client_files.Cursor = System.Windows.Forms.Cursors.Hand;
            this.client_files.FormattingEnabled = true;
            this.client_files.Location = new System.Drawing.Point(325, 73);
            this.client_files.Name = "client_files";
            this.client_files.Size = new System.Drawing.Size(94, 212);
            this.client_files.TabIndex = 16;
            this.client_files.SelectedIndexChanged += new System.EventHandler(this.client_files_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 416);
            this.Controls.Add(this.client_files);
            this.Controls.Add(this.client_file_status_lb);
            this.Controls.Add(this.client_selectFile_btn);
            this.Controls.Add(this.client_name_tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.client_connect_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.client_port_tb);
            this.Controls.Add(this.client_ip_tb);
            this.Controls.Add(this.client_conversation_Lb);
            this.Controls.Add(this.client_sendBtn);
            this.Controls.Add(this.client_mb);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox client_mb;
        private System.Windows.Forms.Button client_sendBtn;
        private System.Windows.Forms.ListBox client_conversation_Lb;
        private System.Windows.Forms.TextBox client_ip_tb;
        private System.Windows.Forms.TextBox client_port_tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button client_connect_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox client_name_tb;
        private System.Windows.Forms.Button client_selectFile_btn;
        private System.Windows.Forms.Label client_file_status_lb;
        private System.Windows.Forms.ListBox client_files;
    }
}