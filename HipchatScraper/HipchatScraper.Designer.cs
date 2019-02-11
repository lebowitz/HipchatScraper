namespace HipchatScraper
{
    partial class HipchatScraper
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
            this.buttonScrape = new System.Windows.Forms.Button();
            this.labelHipChatPersonalAccessToken = new System.Windows.Forms.Label();
            this.textBoxHipchatPersonalAccessToken = new System.Windows.Forms.TextBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.labelCounts = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelCountsOutput = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonScrape
            // 
            this.buttonScrape.Location = new System.Drawing.Point(286, 16);
            this.buttonScrape.Name = "buttonScrape";
            this.buttonScrape.Size = new System.Drawing.Size(75, 23);
            this.buttonScrape.TabIndex = 0;
            this.buttonScrape.Text = "Scrape";
            this.buttonScrape.UseVisualStyleBackColor = true;
            this.buttonScrape.Click += new System.EventHandler(this.buttonScrape_Click);
            // 
            // labelHipChatPersonalAccessToken
            // 
            this.labelHipChatPersonalAccessToken.AutoSize = true;
            this.labelHipChatPersonalAccessToken.Location = new System.Drawing.Point(13, 21);
            this.labelHipChatPersonalAccessToken.Name = "labelHipChatPersonalAccessToken";
            this.labelHipChatPersonalAccessToken.Size = new System.Drawing.Size(161, 13);
            this.labelHipChatPersonalAccessToken.TabIndex = 1;
            this.labelHipChatPersonalAccessToken.Text = "HipChat Personal Access Token";
            // 
            // textBoxHipchatPersonalAccessToken
            // 
            this.textBoxHipchatPersonalAccessToken.Location = new System.Drawing.Point(180, 18);
            this.textBoxHipchatPersonalAccessToken.Name = "textBoxHipchatPersonalAccessToken";
            this.textBoxHipchatPersonalAccessToken.Size = new System.Drawing.Size(100, 20);
            this.textBoxHipchatPersonalAccessToken.TabIndex = 2;
            this.textBoxHipchatPersonalAccessToken.UseSystemPasswordChar = true;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(13, 45);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(373, 350);
            this.textBoxOutput.TabIndex = 3;
            // 
            // labelCounts
            // 
            this.labelCounts.AutoSize = true;
            this.labelCounts.Location = new System.Drawing.Point(13, 416);
            this.labelCounts.Name = "labelCounts";
            this.labelCounts.Size = new System.Drawing.Size(40, 13);
            this.labelCounts.TabIndex = 4;
            this.labelCounts.Text = "Counts";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 415);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 5;
            // 
            // labelCountsOutput
            // 
            this.labelCountsOutput.AutoSize = true;
            this.labelCountsOutput.Location = new System.Drawing.Point(57, 416);
            this.labelCountsOutput.Name = "labelCountsOutput";
            this.labelCountsOutput.Size = new System.Drawing.Size(154, 13);
            this.labelCountsOutput.TabIndex = 6;
            this.labelCountsOutput.Text = "0 Users, 0 Rooms, 0 Messages";
            // 
            // HipchatScraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 438);
            this.Controls.Add(this.labelCountsOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCounts);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.textBoxHipchatPersonalAccessToken);
            this.Controls.Add(this.labelHipChatPersonalAccessToken);
            this.Controls.Add(this.buttonScrape);
            this.Name = "HipchatScraper";
            this.Text = "HipChat Scraper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonScrape;
        private System.Windows.Forms.Label labelHipChatPersonalAccessToken;
        private System.Windows.Forms.TextBox textBoxHipchatPersonalAccessToken;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label labelCounts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCountsOutput;
    }
}