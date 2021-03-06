﻿namespace WindowsFormsApplication1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Values = new System.Windows.Forms.TextBox();
            this.roundButton1 = new RoundButton.RoundButton();
            this.progs = new RoundButton.RoundButton();
            this.roundButton2 = new RoundButton.RoundButton();
            this.HostFileWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.HostFileWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(12, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(172, 23);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Timer";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Minutter";
            // 
            // Values
            // 
            this.Values.BackColor = System.Drawing.Color.White;
            this.Values.Location = new System.Drawing.Point(76, 125);
            this.Values.Name = "Values";
            this.Values.Size = new System.Drawing.Size(127, 20);
            this.Values.TabIndex = 8;
            this.Values.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Values.TextChanged += new System.EventHandler(this.Values_TextChanged);
            // 
            // roundButton1
            // 
            this.roundButton1.BackColor = System.Drawing.Color.White;
            this.roundButton1.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.roundButton1.FlatAppearance.BorderSize = 5;
            this.roundButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.roundButton1.ForeColor = System.Drawing.Color.Black;
            this.roundButton1.Location = new System.Drawing.Point(279, 101);
            this.roundButton1.Name = "roundButton1";
            this.roundButton1.Size = new System.Drawing.Size(91, 54);
            this.roundButton1.TabIndex = 12;
            this.roundButton1.Text = "Ikke tiladte hjemmesider";
            this.roundButton1.UseVisualStyleBackColor = false;
            this.roundButton1.Click += new System.EventHandler(this.roundButton1_Click);
            // 
            // progs
            // 
            this.progs.BackColor = System.Drawing.Color.White;
            this.progs.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.progs.FlatAppearance.BorderSize = 5;
            this.progs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.progs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.progs.ForeColor = System.Drawing.Color.Black;
            this.progs.Location = new System.Drawing.Point(278, 12);
            this.progs.Name = "progs";
            this.progs.Size = new System.Drawing.Size(92, 54);
            this.progs.TabIndex = 11;
            this.progs.Text = "Ikke tiladte programmer";
            this.progs.UseVisualStyleBackColor = false;
            this.progs.Click += new System.EventHandler(this.progs_Click);
            // 
            // roundButton2
            // 
            this.roundButton2.BackColor = System.Drawing.Color.White;
            this.roundButton2.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.roundButton2.FlatAppearance.BorderSize = 5;
            this.roundButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.roundButton2.ForeColor = System.Drawing.Color.Black;
            this.roundButton2.Location = new System.Drawing.Point(13, 65);
            this.roundButton2.Name = "roundButton2";
            this.roundButton2.Size = new System.Drawing.Size(260, 36);
            this.roundButton2.TabIndex = 13;
            this.roundButton2.Text = "Start";
            this.roundButton2.UseVisualStyleBackColor = false;
            this.roundButton2.Click += new System.EventHandler(this.roundButton2_Click);
            // 
            // HostFileWatcher
            // 
            this.HostFileWatcher.EnableRaisingEvents = true;
            this.HostFileWatcher.Path = "C:\\Windows\\System32\\drivers\\etc";
            this.HostFileWatcher.SynchronizingObject = this;
            this.HostFileWatcher.Changed += new System.IO.FileSystemEventHandler(this.Host_Changed);
            this.HostFileWatcher.Created += new System.IO.FileSystemEventHandler(this.Host_Created);
            this.HostFileWatcher.Deleted += new System.IO.FileSystemEventHandler(this.Host_Deleted);
            this.HostFileWatcher.Renamed += new System.IO.RenamedEventHandler(this.Host_Renamed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(373, 171);
            this.Controls.Add(this.roundButton2);
            this.Controls.Add(this.roundButton1);
            this.Controls.Add(this.progs);
            this.Controls.Add(this.Values);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StudyFocus";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HostFileWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Values;
        private RoundButton.RoundButton progs;
        private RoundButton.RoundButton roundButton1;
        private RoundButton.RoundButton roundButton2;
        private System.IO.FileSystemWatcher HostFileWatcher;
    }
}

