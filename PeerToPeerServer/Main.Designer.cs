namespace PeerToPeerServer
{
    partial class Main
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
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.textBoxLogInfo = new System.Windows.Forms.TextBox();
            this.buttonStopServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxConnectedPeers = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.Location = new System.Drawing.Point(12, 12);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(197, 29);
            this.buttonStartServer.TabIndex = 0;
            this.buttonStartServer.Text = "Start";
            this.buttonStartServer.UseVisualStyleBackColor = true;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_Click);
            // 
            // textBoxLogInfo
            // 
            this.textBoxLogInfo.BackColor = System.Drawing.Color.White;
            this.textBoxLogInfo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLogInfo.Location = new System.Drawing.Point(12, 69);
            this.textBoxLogInfo.Multiline = true;
            this.textBoxLogInfo.Name = "textBoxLogInfo";
            this.textBoxLogInfo.ReadOnly = true;
            this.textBoxLogInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogInfo.Size = new System.Drawing.Size(197, 213);
            this.textBoxLogInfo.TabIndex = 1;
            // 
            // buttonStopServer
            // 
            this.buttonStopServer.Location = new System.Drawing.Point(228, 12);
            this.buttonStopServer.Name = "buttonStopServer";
            this.buttonStopServer.Size = new System.Drawing.Size(197, 29);
            this.buttonStopServer.TabIndex = 3;
            this.buttonStopServer.Text = "Stop";
            this.buttonStopServer.UseVisualStyleBackColor = true;
            this.buttonStopServer.Click += new System.EventHandler(this.buttonStopServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Log Information:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Connected Peers:";
            // 
            // listBoxConnectedPeers
            // 
            this.listBoxConnectedPeers.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxConnectedPeers.ForeColor = System.Drawing.Color.Black;
            this.listBoxConnectedPeers.FormattingEnabled = true;
            this.listBoxConnectedPeers.HorizontalScrollbar = true;
            this.listBoxConnectedPeers.Location = new System.Drawing.Point(228, 70);
            this.listBoxConnectedPeers.Name = "listBoxConnectedPeers";
            this.listBoxConnectedPeers.Size = new System.Drawing.Size(197, 212);
            this.listBoxConnectedPeers.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 289);
            this.Controls.Add(this.listBoxConnectedPeers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStopServer);
            this.Controls.Add(this.textBoxLogInfo);
            this.Controls.Add(this.buttonStartServer);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartServer;
        private System.Windows.Forms.TextBox textBoxLogInfo;
        private System.Windows.Forms.Button buttonStopServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxConnectedPeers;
    }
}

