using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ServerlessSpaWithDotNet.Models;

namespace ServerlessSpaWithDotNet.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private static List<CalendarEvent> events;

        public EventsController()
        {
            if (events == null)
            {
                var calEvent = new CalendarEvent()
                {
                    id = Guid.NewGuid().ToString(),
                    title = "Test Event Title",
                    allDay = false,
                    start = DateTime.Now,
                    end = DateTime.Now.AddHours(2)
                };
                events = new List<CalendarEvent>( new[] { calEvent } );
            }
        }

        // GET api/events
        [HttpGet]
        public IEnumerable<CalendarEvent> Get(DateTime? start, DateTime? end)
        {   
            start = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            end = end ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            return events.FindAll(x => x.end >= start && x.start <= end);
        }

        // GET api/events/5
        [HttpGet("{id}")]
        public CalendarEvent Get(string id)
        {
            return events.Find(x => x.id == id);
        }

        // POST api/events
        [HttpPost]
        [Authorize(Policy = "InCalendarWriterGroup")]
        public void Post([FromForm]CalendarEvent calEvent)
        {
            calEvent.id = Guid.NewGuid().ToString();
            events.Add(calEvent);
        }

        // PUT api/events/5
        [HttpPut("{id}")]
        [Authorize(Policy = "InCalendarWriterGroup")]
        public void Put(string id, [FromForm]CalendarEvent calEvent)
        {
            var index = events.FindIndex(x => x.id == id);
            events[index] = calEvent;
        }

    }
}
