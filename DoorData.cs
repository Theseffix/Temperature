using System;
using System.Collections.Generic;
using System.Text;

namespace temperature
{
    public class DoorData
    {
        public DateTime Date;
        public double Temperature;
        public double InsideTemperature;
        public double OutsideTemperature;

        public DoorData(DateTime date, double temp, double iTemp, double oTemp)
        {
            Date = date;
            Temperature = temp;
            InsideTemperature = iTemp;
            OutsideTemperature = oTemp;
        }
    }
}
