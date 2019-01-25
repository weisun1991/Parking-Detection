namespace TruckParkingProject
{
    partial class MainWindow
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
            this.lblDataFolder = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnReadWIMfiles = new System.Windows.Forms.Button();
            this.TruckDataOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.btnParkingRecords = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDataFolder
            // 
            this.lblDataFolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDataFolder.Location = new System.Drawing.Point(80, 174);
            this.lblDataFolder.Name = "lblDataFolder";
            this.lblDataFolder.Size = new System.Drawing.Size(387, 25);
            this.lblDataFolder.TabIndex = 44;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 45;
            this.label10.Text = "Data Folder:";
            // 
            // btnReadWIMfiles
            // 
            this.btnReadWIMfiles.Location = new System.Drawing.Point(483, 174);
            this.btnReadWIMfiles.Name = "btnReadWIMfiles";
            this.btnReadWIMfiles.Size = new System.Drawing.Size(86, 25);
            this.btnReadWIMfiles.TabIndex = 47;
            this.btnReadWIMfiles.Text = "Read File";
            this.btnReadWIMfiles.UseVisualStyleBackColor = true;
            this.btnReadWIMfiles.Click += new System.EventHandler(this.btnReadWIMfiles_Click);
            // 
            // TruckDataOpenFile
            // 
            this.TruckDataOpenFile.FileName = "openFileDialog1";
            // 
            // btnParkingRecords
            // 
            this.btnParkingRecords.Location = new System.Drawing.Point(582, 175);
            this.btnParkingRecords.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnParkingRecords.Name = "btnParkingRecords";
            this.btnParkingRecords.Size = new System.Drawing.Size(116, 24);
            this.btnParkingRecords.TabIndex = 49;
            this.btnParkingRecords.Text = "Parking Records";
            this.btnParkingRecords.UseVisualStyleBackColor = true;
            this.btnParkingRecords.Click += new System.EventHandler(this.btnParkingLot_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 444);
            this.Controls.Add(this.btnParkingRecords);
            this.Controls.Add(this.btnReadWIMfiles);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblDataFolder);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainWindow";
            this.Text = "Truck Parking Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDataFolder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnReadWIMfiles;
        private System.Windows.Forms.OpenFileDialog TruckDataOpenFile;
        private System.Windows.Forms.Button btnParkingRecords;
    }
}

