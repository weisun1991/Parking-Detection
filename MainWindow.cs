using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TruckParkingProject
{
    public partial class MainWindow : Form
    {
        private static string SourceDir = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectDataFolder_Click(object sender, EventArgs e)
        {

        }

        private void btnReadWIMfiles_Click(object sender, EventArgs e)
        {
            //this.TruckDataFolderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            //TruckDataFolderBrowser.ShowDialog();
            //SourceDir = TruckDataFolderBrowser.SelectedPath;

            this.TruckDataOpenFile.InitialDirectory = @"D:\";
            TruckDataOpenFile.ShowDialog();
            SourceDir = TruckDataOpenFile.FileName;

            lblDataFolder.Text = SourceDir;
            FileIO_Output.ReadTruckParkingDataFile(SourceDir);
        }


        private void btnParkingLot_Click(object sender, EventArgs e)
        {
            frmParkingLot ParkingLotForm = new frmParkingLot(FileIO_Output.RecordList);
            ParkingLotForm.Show();
        }
    }
}
