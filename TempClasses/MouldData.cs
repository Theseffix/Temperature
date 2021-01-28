using System;
using System.Collections.Generic;
using System.Text;

namespace temperature
{
    class MouldData
    {
        public DateTime Date;
        public double AverageTemperature;
        public double AverageMoisture;
        public double MouldIndex;



        public MouldData(DateTime date, double avgTemp, double avgMoist, double mould)
        {
            Date = date;
            AverageTemperature = avgTemp;
            AverageMoisture = avgMoist;
            MouldIndex = mould;
        }
    }
}
