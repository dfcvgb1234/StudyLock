namespace WindowsFormsApplication1
{
    partial class ListOfProcesses
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.Save = new System.Windows.Forms.Button();
            this.CheckAll = new System.Windows.Forms.Button();
            this.DeSelectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 42);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(296, 424);
            this.checkedListBox1.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(44, 472);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(235, 26);
            this.Save.TabIndex = 1;
            this.Save.Text = "Gem ændringer";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // CheckAll
            // 
            this.CheckAll.Location = new System.Drawing.Point(12, 12);
            this.CheckAll.Name = "CheckAll";
            this.CheckAll.Size = new System.Drawing.Size(130, 21);
            this.CheckAll.TabIndex = 2;
            this.CheckAll.Text = "Aktiver alle";
            this.CheckAll.UseVisualStyleBackColor = true;
            this.CheckAll.Click += new System.EventHandler(this.CheckAll_click);
            // 
            // DeSelectAll
            // 
            this.DeSelectAll.Location = new System.Drawing.Point(185, 12);
            this.DeSelectAll.Name = "DeSelectAll";
            this.DeSelectAll.Size = new System.Drawing.Size(123, 22);
            this.DeSelectAll.TabIndex = 3;
            this.DeSelectAll.Text = "Deaktiver alle";
            this.DeSelectAll.UseVisualStyleBackColor = true;
            this.DeSelectAll.Click += new System.EventHandler(this.DeSelectAll_Click);
            // 
            // ListOfProcesses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 503);
            this.Controls.Add(this.DeSelectAll);
            this.Controls.Add(this.CheckAll);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.checkedListBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ListOfProcesses";
            this.Text = "Tiladte Programmer";
            this.Load += new System.EventHandler(this.ListOfProcesses_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button CheckAll;
        private System.Windows.Forms.Button DeSelectAll;
    }
}