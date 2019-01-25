using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckParkingProject
{
    public class TruckParkingRecord
    {
        private byte _spaceNumber;
        private string _date;
        private string _time;
        private int _year;
        private byte _month;
        private byte _day;
        private byte _hour;
        private byte _minute;
        private byte _second;
        private byte _class; // 1: truck, 0: other
        private bool _isComingIn; //true:in, false: out
        private string _alignment;
        private string _identifyingInformation;
        private int _hourTimeStamp;
        private int _dayTimeStamp;
        private int _monthTimeStamp;

        public TruckParkingRecord()
        {
            _spaceNumber = SpaceNumber;
            _date = Date;
            _time = Time;
            _year = Year;
            _month = Month;
            _day = Day;
            _hour = Hour;
            _minute = Minute;
            _second = Second;
            _class = Class;
            _isComingIn = IsComingIn;
            _alignment = Alignment;
            _identifyingInformation = IdentifyingInformation;
            _hourTimeStamp = HourTimeStamp;
            _dayTimeStamp = DayTimeStamp;
            _monthTimeStamp = MonthTimeStamp;
        }

        public byte SpaceNumber
        {
            get
            {
                return _spaceNumber;
            }

            set
            {
                _spaceNumber = value;
            }
        }

        public byte Month
        {
            get
            {
                return _month;
            }

            set
            {
                _month = value;
            }
        }

        public byte Day
        {
            get
            {
                return _day;
            }

            set
            {
                _day = value;
            }
        }

        public byte Hour
        {
            get
            {
                return _hour;
            }

            set
            {
                _hour = value;
            }
        }

        public byte Minute
        {
            get
            {
                return _minute;
            }

            set
            {
                _minute = value;
            }
        }

        public byte Second
        {
            get
            {
                return _second;
            }

            set
            {
                _second = value;
            }
        }

        public string Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
            }
        }

        public byte Class
        {
            get
            {
                return _class;
            }

            set
            {
                _class = value;
            }
        }

        public int Year
        {
            get
            {
                return _year;
            }

            set
            {
                _year = value;
            }
        }

        public bool IsComingIn
        {
            get
            {
                return _isComingIn;
            }

            set
            {
                _isComingIn = value;
            }
        }

        public string Alignment
        {
            get
            {
                return _alignment;
            }

            set
            {
                _alignment = value;
            }
        }

        public string IdentifyingInformation
        {
            get
            {
                return _identifyingInformation;
            }

            set
            {
                _identifyingInformation = value;
            }
        }

        public string Time
        {
            get
            {
                return _time;
            }

            set
            {
                _time = value;
            }
        }

        public int HourTimeStamp
        {
            get
            {
                return _hourTimeStamp;
            }

            set
            {
                _hourTimeStamp = value;
            }
        }

        public int DayTimeStamp
        {
            get
            {
                return _dayTimeStamp;
            }

            set
            {
                _dayTimeStamp = value;
            }
        }

        public int MonthTimeStamp
        {
            get
            {
                return _monthTimeStamp;
            }

            set
            {
                _monthTimeStamp = value;
            }
        }
    }
}
