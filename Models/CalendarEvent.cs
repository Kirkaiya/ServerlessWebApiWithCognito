using System;

namespace ServerlessSpaWithDotNet.Models
{
    public class CalendarEvent
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
