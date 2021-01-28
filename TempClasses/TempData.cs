using System;
using System.Collections.Generic;
using System.Text;

namespace temperature
{
    class TempData
    {
        public DateTime Date { get; set; }
        public float Temperature { get; set; }
        public int Moisture { get; set; }

        public TempData(DateTime dateTime, float temp, int moist)
        {
            Date = dateTime;
            Temperature = temp;
            Moisture = moist;
        }

    }
}
