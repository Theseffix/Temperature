using System;
using System.Collections.Generic;
using System.Text;

namespace temperature
{
    class TempDataDubs
    {
        public DateTime Date { get; set; }
        public string Area { get; set; }
        public float Temperature { get; set; }
        public int Moisture { get; set; }

        public TempDataDubs(DateTime dateTime, string area, float temp, int moist)
        {
            Date = dateTime;
            Area = area;
            Temperature = temp;
            Moisture = moist;
        }
    }
}
