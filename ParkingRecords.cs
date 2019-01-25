using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace TruckParkingProject
{
    public partial class frmParkingRecords : Form
    {
        public enum ChartTypes
        {
            ExactTimeChart = 1,
            IntervalTrucksChart = 2,
            IntervalOccupancyChart = 3,
            TimelineChart = 4
        }
        ChartTypes TypeOfChart = ChartTypes.ExactTimeChart;
        //SwashStatistics_ChartingControl.ctrlDistributionDisplay ctrlDistributionDisplay1 = new SwashStatistics_ChartingControl.ctrlDistributionDisplay();
        List<TruckParkingRecord> TruckRecords = new List<TruckParkingRecord>();
        List<double> xValueList = new List<double>();
        List<double> yValueList = new List<double>();
        List<List<double>> xLists = new List<List<double>>();
        List<List<double>> yLists = new List<List<double>>();
        int Year = 2016;
        byte Month = 5;
        byte Day = 12;
        byte Hour = 6;
        byte Minute = 0;
        byte Second = 0;
        byte SpaceNum = 10; //default as 10, should read from the file
        int TimeStamp = 21600; //default: 6:00
        int IntervalStartTimeStamp = 0;
        int IntervalEndTimeStamp = 64800; //default: 18:00
        int TotalTimeStamp = 86400;
        byte IntervalStartHour = 0;
        byte IntervalStartMinute = 0;
        byte IntervalStartSecond = 0;
        byte IntervalEndHour = 18;
        byte IntervalEndMinute = 0;
        byte IntervalEndSecond = 0;
        int yInterval = 1;
        int yMax = 1;
        int yMin = 0;
        int xInterval = 1;
        int xMax = 1;
        int xMin = 0;
        List<List<double>> InTimeListsAllSpots = new List<List<double>>();
        List<List<double>> OutTimeListsAllSpots = new List<List<double>>();
        List<double> InTimes;
        List<double> OutTimes;
        List<int[,]> timeStampRecordsBySpace; //First MonthTimeStamp, Second 1:parking, 0: no parking
        List<List<int[,]>> timeStampRecords = new List<List<int[,]>>();
        public frmParkingRecords(List<TruckParkingRecord> recordsImport)
        {
            InitializeComponent();
            TruckRecords = recordsImport;
            LoadTruckRecords();
            GetInOutTimeLists();
            GetTimeStampRecords();
        }
        private void GetInOutTimeLists()
        {
            InTimeListsAllSpots.Clear();
            OutTimeListsAllSpots.Clear();

            for (int Space = 1; Space <= SpaceNum; Space++)
            {
                InTimes = new List<double>();
                OutTimes = new List<double>();
                for (int RecordNum = 0; RecordNum < TruckRecords.Count; RecordNum++)
                {
                    if (TruckRecords[RecordNum].SpaceNumber == Space)
                    {
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {
                            InTimes.Add(CalculateMonthTimeStamp(TruckRecords[RecordNum].Month,TruckRecords[RecordNum].Day,TruckRecords[RecordNum].Hour, TruckRecords[RecordNum].Minute, TruckRecords[RecordNum].Second));
                        }
                        else if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            OutTimes.Add(CalculateMonthTimeStamp(TruckRecords[RecordNum].Month, TruckRecords[RecordNum].Day,TruckRecords[RecordNum].Hour, TruckRecords[RecordNum].Minute, TruckRecords[RecordNum].Second));
                        }
                    }
                }
                InTimeListsAllSpots.Add(InTimes);
                OutTimeListsAllSpots.Add(OutTimes);
            }
        }
        private void GetTimeStampRecords()
        {
            int StartTimeStamp = 5 * 2678400 + 11 * 86400 + 22 * 3600; // 11 May, 22:00
            int EndTimeStamp = 5 * 2678400 + 12 * 86400 + 8 * 3600;  // 12 May, 8:00
            timeStampRecords.Clear();
            int[,] stampRecord;
            for (int space = 0; space < SpaceNum;space++)
            {
                timeStampRecordsBySpace = new List<int[,]>();
                for(int timeStamp =StartTimeStamp; timeStamp < EndTimeStamp; timeStamp +=60) //Every one minute
                {
                    stampRecord = new int[1, 2];
                    stampRecord[0, 0] = timeStamp;
                    for (int NumInOut=0; NumInOut < OutTimeListsAllSpots[space].Count;NumInOut++)
                    {
                        
                        if (timeStamp >= InTimeListsAllSpots[space][NumInOut] && timeStamp <= OutTimeListsAllSpots[space][NumInOut])
                        {
                            stampRecord[0, 1] = 1;
                            break;
                        }
                        else
                        {
                            stampRecord[0, 1] = 0;
                        }
                        
                    }
                    timeStampRecordsBySpace.Add(stampRecord);

                }
                timeStampRecords.Add(timeStampRecordsBySpace);
            }
        }
        private void ChartSetting()
        {
            if (TypeOfChart == ChartTypes.ExactTimeChart)
            {
                GetInputExactTimeChart();
                yInterval = 1;
                yMax = 1;
                yMin = 0;
                xMin = 0;
                xMax = 10; //NumOfSpots

            }
            else if (TypeOfChart == ChartTypes.IntervalTrucksChart)
            {
                GetInputIntervalChart();
            }
            else if (TypeOfChart == ChartTypes.IntervalOccupancyChart)
            {
                GetInputParkingTimeChart();
                yInterval = 1500;
                yMax = 30000;
                yMin = 0;

            }
            else if (TypeOfChart == ChartTypes.TimelineChart)
            {
                GetInputTimelineChart();
                yInterval = 1;
                yMax = 10; // NumSpots
                yMin = 0;
            }
        }
        private void CreateChart()
        {
            ChartSetting();
            SwashStatistics_ChartingControl.ChartSettings newChartSettings = new SwashStatistics_ChartingControl.ChartSettings(xInterval, xInterval, xMin, xMax, yInterval, yInterval, yMin, yMax, "Space #", "Truck Parking", true, false);
            if (TypeOfChart != ChartTypes.TimelineChart)
            {
                Series newSeries = new Series("Truck Parking Display");
                newSeries.ChartType = SeriesChartType.Column;
                newSeries.XAxisType = AxisType.Primary;
                newSeries.YAxisType = AxisType.Primary;
                //newSeries.IsVisibleInLegend = true;
                newSeries.BorderWidth = 3;
                newSeries.Color = Color.Blue;
                newChartSettings.SetDataSeries(newSeries, xValueList, yValueList);

                newChartSettings.DataSeries.Add(newSeries);
                ctrlDistributionDisplay1.CreateChart(newChartSettings);
            }
            else if (TypeOfChart == ChartTypes.TimelineChart)
            {


            }
        }
        private void GetInputExactTimeChart()
        {
            xValueList.Clear();
            yValueList.Clear();
            byte xValue = 0;
            TimeStamp = CalculateHourTimeStamp(Hour, Minute, Second);

            for (int Space = 1; Space <= SpaceNum; Space++)
            {
                xValue++;
                bool IsParking = false;

                for (int RecordNum = 0; RecordNum < TruckRecords.Count; RecordNum++)
                {

                    if (TruckRecords[RecordNum].SpaceNumber == Space)
                    {
                        int PairIndicator = 0;
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {

                            if (TruckRecords[RecordNum].Year < Year || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month < Month || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day < Day || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day == Day & TruckRecords[RecordNum].HourTimeStamp <= TimeStamp) // In before the time selected
                            {
                                IsParking = true;
                            }
                            PairIndicator++;

                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            if (TruckRecords[RecordNum].Year < Year || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month < Month || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day < Day || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day == Day & TruckRecords[RecordNum].HourTimeStamp < TimeStamp)
                            {
                                IsParking = false;
                            }
                            PairIndicator++;
                        }
                        if (PairIndicator > 1 & IsParking == true)
                        {
                            break;
                        }
                    }
                }
                if (IsParking == true)
                {
                    yValueList.Add(1);
                }
                else
                {
                    yValueList.Add(0);
                }
                xValueList.Add(xValue); //defalt 10 values
            }

        }
        private void GetInputIntervalChart()
        {
            IntervalStartTimeStamp = CalculateHourTimeStamp(IntervalStartHour, IntervalStartMinute, IntervalStartSecond);
            IntervalEndTimeStamp = CalculateHourTimeStamp(IntervalEndHour, IntervalEndMinute, IntervalEndSecond);


        }
        private void GetInputParkingTimeChart()
        {
            xValueList.Clear();
            yValueList.Clear();
            IntervalStartTimeStamp = CalculateHourTimeStamp(IntervalStartHour, IntervalStartMinute, IntervalStartSecond);
            IntervalEndTimeStamp = CalculateHourTimeStamp(IntervalEndHour, IntervalEndMinute, IntervalEndSecond);
            byte xValue = 0;

            for (int Space = 1; Space <= SpaceNum; Space++)
            {
                xValue++;
                int OccupancyTimeStamp = 0;
                bool IsParking = false;
                bool IsParkedBeforeStart = false;
                bool IsParkedAfterEnd = false;
                int RecordStart = 0;
                int RecordEnd = 0;
                //isParkedBeforeStart
                for (int RecordNum = 0; RecordNum < TruckRecords.Count; RecordNum++)
                {
                    if (TruckRecords[RecordNum].SpaceNumber == Space)
                    {
                        int PairIndicator = 0;
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {
                            if (TruckRecords[RecordNum].Year < Year || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month < Month || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day < Day || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day == Day & TruckRecords[RecordNum].HourTimeStamp <= IntervalStartTimeStamp) // In before the time selected
                            {
                                IsParking = true;
                                PairIndicator++;
                            }

                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            if (TruckRecords[RecordNum].Year < Year || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month < Month || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day < Day || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day == Day & TruckRecords[RecordNum].HourTimeStamp < IntervalStartTimeStamp)
                            {
                                IsParking = false;
                                RecordStart = RecordNum;
                            }
                            PairIndicator++;
                        }
                        if (IsParking == true)
                        {
                            IsParkedBeforeStart = true;
                            break;
                        }
                    }

                }
                //isParkedAfterEnd
                IsParking = false;
                for (int RecordNum = 0; RecordNum < TruckRecords.Count; RecordNum++)
                {
                    if (TruckRecords[RecordNum].SpaceNumber == Space)
                    {
                        int PairIndicator = 0;
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {

                            if (TruckRecords[RecordNum].Year < Year || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month < Month || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day < Day || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day == Day & TruckRecords[RecordNum].HourTimeStamp <= IntervalEndTimeStamp) // In before the time selected
                            {
                                IsParking = true;
                                RecordEnd = RecordNum;
                            }

                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            if (TruckRecords[RecordNum].Year < Year || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month < Month || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day < Day || TruckRecords[RecordNum].Year == Year & TruckRecords[RecordNum].Month == Month & TruckRecords[RecordNum].Day == Day & TruckRecords[RecordNum].HourTimeStamp < IntervalEndTimeStamp)
                            {
                                IsParking = false;
                            }
                        }
                        if (IsParking == true)
                        {
                            IsParkedAfterEnd = true;
                            break;
                        }
                    }

                }
                //OccupancyTimeStamp
                if (IsParkedBeforeStart == true & IsParkedAfterEnd == true)
                {
                    for (int RecordNum = RecordStart; RecordNum <= RecordEnd; RecordNum++)
                    {
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {
                            OccupancyTimeStamp -= TruckRecords[RecordNum].HourTimeStamp;
                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            OccupancyTimeStamp += TruckRecords[RecordNum].HourTimeStamp;
                        }
                        OccupancyTimeStamp = OccupancyTimeStamp + IntervalEndTimeStamp - IntervalStartTimeStamp;
                    }
                }
                if (IsParkedBeforeStart == true & IsParkedAfterEnd == false)
                {
                    for (int RecordNum = RecordStart; RecordNum <= RecordEnd; RecordNum++)
                    {
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {
                            OccupancyTimeStamp -= TruckRecords[RecordNum].HourTimeStamp;
                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            OccupancyTimeStamp += TruckRecords[RecordNum].HourTimeStamp;
                        }
                        OccupancyTimeStamp = OccupancyTimeStamp - IntervalStartTimeStamp;
                    }
                }
                if (IsParkedBeforeStart == false & IsParkedAfterEnd == true)
                {
                    for (int RecordNum = RecordStart; RecordNum <= RecordEnd; RecordNum++)
                    {
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {
                            OccupancyTimeStamp -= TruckRecords[RecordNum].HourTimeStamp;
                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            OccupancyTimeStamp += TruckRecords[RecordNum].HourTimeStamp;
                        }
                        OccupancyTimeStamp = OccupancyTimeStamp + IntervalEndTimeStamp;
                    }
                }
                if (IsParkedBeforeStart == false & IsParkedAfterEnd == false)
                {
                    for (int RecordNum = RecordStart; RecordNum <= RecordEnd; RecordNum++)
                    {
                        if (TruckRecords[RecordNum].IsComingIn == true)
                        {
                            OccupancyTimeStamp -= TruckRecords[RecordNum].HourTimeStamp;
                        }
                        if (TruckRecords[RecordNum].IsComingIn == false)
                        {
                            OccupancyTimeStamp += TruckRecords[RecordNum].HourTimeStamp;
                        }
                    }
                }
                yValueList.Add(OccupancyTimeStamp);
                xValueList.Add(xValue);
            }
        }
        private void GetInputTimelineChart()
        {

        }
        private void GetInputOccupancyChart()
        {
            xValueList.Clear();
            yValueList.Clear();
            IntervalStartTimeStamp = CalculateHourTimeStamp(IntervalStartHour, IntervalStartMinute, IntervalStartSecond);
            IntervalEndTimeStamp = CalculateHourTimeStamp(IntervalEndHour, IntervalEndMinute, IntervalEndSecond);
            byte xValue = 0;
            for (int Space = 1; Space <= SpaceNum; Space++)
            {

            }
        }
        private int CalculateMonthTimeStamp(byte month, byte day, byte hour, byte minute, byte second)
        {
            int timeStamp = Convert.ToInt32(month* 2678400+ day* 86400+hour * 3600 + minute * 60 + second);
            return timeStamp;
        }
        private int CalculateHourTimeStamp(byte hour, byte minute, byte second)
        {
            int timeStamp = Convert.ToInt32(hour * 3600 + minute * 60 + second);
            return timeStamp;
        }
        private void btnSpecifiedChart_Click(object sender, EventArgs e)
        {
            CreateChart();
        }

        private void updnHour_ValueChanged(object sender, EventArgs e)
        {
            Hour = Convert.ToByte(updnHour.Value);
        }

        private void updnMinute_ValueChanged(object sender, EventArgs e)
        {
            Minute = Convert.ToByte(updnMinute.Value);
        }

        private void updnSecond_ValueChanged(object sender, EventArgs e)
        {
            Second = Convert.ToByte(updnSecond.Value);
        }

        private void updnYear_ValueChanged(object sender, EventArgs e)
        {
            Year = Convert.ToByte(updnYear.Value);
        }

        private void updnMonth_ValueChanged(object sender, EventArgs e)
        {
            Month = Convert.ToByte(updnMonth.Value);
        }

        private void updnDay_ValueChanged(object sender, EventArgs e)
        {
            Day = Convert.ToByte(updnDay.Value);
        }

        private void cboChartTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChartTypes.SelectedIndex == 0)
            {
                TypeOfChart = ChartTypes.ExactTimeChart;
            }
            else if (cboChartTypes.SelectedIndex == 1)
            {
                TypeOfChart = ChartTypes.IntervalTrucksChart;
            }
            else if (cboChartTypes.SelectedIndex == 2)
            {
                TypeOfChart = ChartTypes.IntervalOccupancyChart;
            }
            else if (cboChartTypes.SelectedIndex == 3)
            {
                TypeOfChart = ChartTypes.TimelineChart;
            }
            SetControlStatus();
        }
        private void SetControlStatus()
        {
            if (TypeOfChart == ChartTypes.ExactTimeChart)
            {
                updnHour.Enabled = true;
                updnMinute.Enabled = true;
                updnSecond.Enabled = true;
                grpIntervalSpecification.Enabled = false;

            }
            else if (TypeOfChart == ChartTypes.IntervalTrucksChart)
            {
                updnHour.Enabled = false;
                updnMinute.Enabled = false;
                updnSecond.Enabled = false;
                grpIntervalSpecification.Enabled = true;
            }
            else if (TypeOfChart == ChartTypes.IntervalOccupancyChart)
            {
                updnHour.Enabled = false;
                updnMinute.Enabled = false;
                updnSecond.Enabled = false;
                grpIntervalSpecification.Enabled = true;
            }
            else if (TypeOfChart == ChartTypes.TimelineChart)
            {
                updnHour.Enabled = false;
                updnMinute.Enabled = false;
                updnSecond.Enabled = false;
                grpIntervalSpecification.Enabled = false;
            }
        }

        private void updnStartHour_ValueChanged(object sender, EventArgs e)
        {
            IntervalStartHour = Convert.ToByte(updnStartHour.Value);
        }

        private void updnStartMinute_ValueChanged(object sender, EventArgs e)
        {
            IntervalStartMinute = Convert.ToByte(updnStartMinute.Value);
        }

        private void updnStartSecond_ValueChanged(object sender, EventArgs e)
        {
            IntervalStartSecond = Convert.ToByte(updnStartSecond.Value);
        }

        private void updnEndHour_ValueChanged(object sender, EventArgs e)
        {
            IntervalEndHour = Convert.ToByte(updnEndHour.Value);
        }

        private void updnEndMinute_ValueChanged(object sender, EventArgs e)
        {
            IntervalEndMinute = Convert.ToByte(updnEndMinute.Value);
        }

        private void updnEndSecond_ValueChanged(object sender, EventArgs e)
        {
            IntervalEndSecond = Convert.ToByte(updnEndSecond.Value);
        }

        private void tabDataViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabDataViewer.SelectedTab == tabRecordList)
            {
                LoadTruckRecords();
            }
            else if (tabDataViewer.SelectedTab == tabCharts)
            {
                panelChartControl.Controls.Add(ctrlDistributionDisplay1);
                SetControlStatus();
                CreateChart();
            }
        }
        private void LoadTruckRecords()
        {
            dgvTruckRecords.Rows.Clear();

            int TotalVehs = TruckRecords.Count;
            int RowCounter = 0;
            string InOrOut = "";
            string VehType = "";
            dgvTruckRecords.Rows.Add(TotalVehs);

            foreach (TruckParkingRecord record in TruckRecords)
            {
                dgvTruckRecords.Rows[RowCounter].Cells[colID.Name].Value = (RowCounter + 1).ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colSpaceNum.Name].Value = record.SpaceNumber.ToString();
                if (record.IsComingIn == true)
                {
                    InOrOut = "In";
                }
                else
                {
                    InOrOut = "Out";
                }
                dgvTruckRecords.Rows[RowCounter].Cells[colInOrOut.Name].Value = InOrOut;
                dgvTruckRecords.Rows[RowCounter].Cells[colDate.Name].Value = record.Date.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colYear.Name].Value = record.Year.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colMonth.Name].Value = record.Month.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colDay.Name].Value = record.Day.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colTime.Name].Value = record.Time.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colHour.Name].Value = record.Hour.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colMinute.Name].Value = record.Minute.ToString();
                dgvTruckRecords.Rows[RowCounter].Cells[colSecond.Name].Value = record.Second.ToString();
                if (record.Class == 1)
                {
                    VehType = "Truck";
                }
                else
                {
                    VehType = "Other";
                }
                dgvTruckRecords.Rows[RowCounter].Cells[colVehType.Name].Value = VehType;
                dgvTruckRecords.Rows[RowCounter].Cells[colAlignment.Name].Value = record.Alignment;
                dgvTruckRecords.Rows[RowCounter].Cells[colIdentifyingInfo.Name].Value = record.IdentifyingInformation;
                RowCounter++;
            }


        }

        private void btnViewOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;

            saveFileDialog.Filter = "Results Files (*.csv)|*.csv|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileIO_Output.WriteOutputs(saveFileDialog.FileName, timeStampRecords);                         
            }
        }
    }
}
