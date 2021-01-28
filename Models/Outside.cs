using System;
using System.Collections.Generic;
using System.Text;

namespace temperature.Models
{
    class Outside
    {
        public int id { get; set; }
        public DateTime Date { get; set; }
        public float Temperature { get; set; }
        public int Moisture { get; set; }
    }
}
