using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TruckParkingProject
{
    class FileIO_Output
    {
        private static List<TruckParkingRecord> _recordList;

        public FileIO_Output()
        {
            _recordList = RecordList;
        }

        public static List<TruckParkingRecord> RecordList
        {
            get
            {
                return _recordList;
            }

            set
            {
                _recordList = value;
            }
        }


        public static void ReadTruckParkingDataFile(string fileName)
        {
            string fileNameOnly = Path.GetFileName(fileName);
            string ParkingRecord;
            string[] ParkingRecordArr = new string[60];

            bool SkipRecord;
            byte NumberOfHeaderLines = 4;
            RecordList = new List<TruckParkingRecord>();

            StreamReader sr = new StreamReader(fileName);

            //Read header line of .csv file 
            for (int i = 0; i < NumberOfHeaderLines; i++)
                sr.ReadLine();

            while ((ParkingRecord = sr.ReadLine()) != null)  // read a line of text
            {
                SkipRecord = false;
                TruckParkingRecord NewRecord = new TruckParkingRecord();
                ParkingRecordArr = ParkingRecord.Split(',');  //parse the record into fields, based on comma delimitation

                if (SkipRecord == false)
                {
                    string DateInfo = ParkingRecordArr[0];
                    string[] DateInfoArr = DateInfo.Split('-', '/');
                    NewRecord.Date = ParkingRecordArr[0];
                    NewRecord.Month = Convert.ToByte(DateInfoArr[0]);
                    NewRecord.Day = Convert.ToByte(DateInfoArr[1]);
                    NewRecord.Year = Convert.ToInt32(DateInfoArr[2]);
                    string TimeInfo = "";

                    if (ParkingRecordArr[2] != "")
                    {
                        TimeInfo = ParkingRecordArr[2];
                        NewRecord.IsComingIn = true;
                    }
                    else if (ParkingRecordArr[2] == "" && ParkingRecordArr[3] != "")
                    {
                        TimeInfo = ParkingRecordArr[3];
                        NewRecord.IsComingIn = false;
                    }
                    string[] TimeInfoArr = TimeInfo.Split(':');
                    NewRecord.Time = TimeInfo;
                    NewRecord.Hour = Convert.ToByte(TimeInfoArr[0]);
                    NewRecord.Minute = Convert.ToByte(TimeInfoArr[1]);
                    NewRecord.Second = Convert.ToByte(TimeInfoArr[2]);
                    NewRecord.HourTimeStamp = Convert.ToInt32(NewRecord.Hour * 3600 + NewRecord.Minute * 60 + NewRecord.Second);

                    NewRecord.DayTimeStamp = Convert.ToInt32(NewRecord.Day * 86400 + NewRecord.Hour * 3600 + NewRecord.Minute * 60 + NewRecord.Second);
                    NewRecord.MonthTimeStamp = Convert.ToInt32(NewRecord.Month*2678400+NewRecord.Day * 86400 + NewRecord.Hour * 3600 + NewRecord.Minute * 60 + NewRecord.Second);
                    //one month=31 days

                    NewRecord.SpaceNumber = Convert.ToByte(ParkingRecordArr[1]);
                    NewRecord.Class = Convert.ToByte(ParkingRecordArr[4]);
                    NewRecord.Alignment = Convert.ToString(ParkingRecordArr[5]);
                    NewRecord.IdentifyingInformation = Convert.ToString(ParkingRecordArr[6]);

                    RecordList.Add(NewRecord);
                }

            }

        }
        public static void WriteOutputs(string filename, List<List<int[,]>> timeStampRecordsImport)
        {
            List<List<int[,]>> timeStampRecords = new List<List<int[,]>>();
            //List<TruckParkingRecord> Records = new List<TruckParkingRecord>();
            timeStampRecords = timeStampRecordsImport;
            byte NumOfSpots = 10; //default
            StreamWriter sw = new StreamWriter(filename);
            sw.Write("Date, Time, Space1,Space2,Space3,Space4,Space5,Space6,Space7,Space8,Space9,Space10");
            sw.WriteLine();
            int totRecords = timeStampRecords[0].Count();
            double Month = new int();
            double Day = new int();
            double Hour = new int();
            double Minute = new int();
            double Second = new int();
            for (int RecordsNum = 0; RecordsNum < totRecords; RecordsNum++)
            {
                Month = Math.Floor((double)timeStampRecords[0][RecordsNum][0, 0] / 2678400);
                Day = Math.Floor((double)(timeStampRecords[0][RecordsNum][0, 0] - Month * 2678400) / 86400);
                Hour = Math.Floor((double)(timeStampRecords[0][RecordsNum][0, 0] - Month * 2678400 - Day * 86400) / 3600);
                Minute = Math.Floor((double)(timeStampRecords[0][RecordsNum][0, 0] - Month * 2678400 - Day * 86400 - Hour * 3600) / 60);
                Second = Math.Floor((double)(timeStampRecords[0][RecordsNum][0, 0] - Month * 2678400 - Day * 86400 - Hour * 3600 - Minute * 60) / 60);
                sw.Write(Day.ToString() + "/" + Month.ToString() + "/16");
                sw.Write(",");
                sw.Write(Hour.ToString() + ":" + Minute.ToString() + ":" + Second.ToString());
                sw.Write(",");
                for (int SpotsNum = 0; SpotsNum < NumOfSpots; SpotsNum++)
                {
                    sw.Write(timeStampRecords[SpotsNum][RecordsNum][0, 1].ToString());
                    sw.Write(",");
                }
                sw.WriteLine();
            }
            
            //    for (int SpotsNum = 0; SpotsNum < NumOfSpots; SpotsNum++)
            //{
            //    for (int RecordsNum = 0; RecordsNum < totRecords; RecordsNum++)
            //    {
            //        Month = Math.Floor((double)timeStampRecords[SpotsNum][RecordsNum][0, 0]/ 2678400);
            //        Day = Math.Floor((double)(timeStampRecords[SpotsNum][RecordsNum][0, 0] - Month * 2678400) / 86400);
            //        Hour = Math.Floor((double)(timeStampRecords[SpotsNum][RecordsNum][0, 0] - Month * 2678400-Day*86400) / 3600);
            //        Minute = Math.Floor((double)(timeStampRecords[SpotsNum][RecordsNum][0, 0] - Month * 2678400 - Day * 86400-Hour*3600) / 60);
            //        Second = Math.Floor((double)(timeStampRecords[SpotsNum][RecordsNum][0, 0] - Month * 2678400 - Day * 86400 - Hour * 3600-Minute*60) / 60);
            //        sw.Write(Day.ToString()+"/"+Month.ToString()+"/16");
            //        sw.Write(",");
            //        sw.Write(Hour.ToString()+":"+Minute.ToString()+":"+Second.ToString());
            //        sw.Write(",");
            //        sw.Write((SpotsNum+1).ToString());
            //        sw.Write(",");
            //        sw.Write(timeStampRecords[SpotsNum][RecordsNum][0, 1].ToString());
            //        sw.WriteLine();

            //    }

            //}

            sw.Close();
        }

    }
}
