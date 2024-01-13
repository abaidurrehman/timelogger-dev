using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class TimeRegistrationsController : Controller
    {
        private readonly TimeloggerDbContext _context;

        public TimeRegistrationsController(TimeloggerDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddTimeRegistration(
            [FromBody] TimeRegistrationDto timeRegistration)
        {
            timeRegistration.StartTime = CombineDateAndTime(timeRegistration.Date, timeRegistration.StartTime);
            timeRegistration.EndTime = CombineDateAndTime(timeRegistration.Date, timeRegistration.EndTime);

            if (timeRegistration.StartTime >= timeRegistration.EndTime)
                return BadRequest(new ApiResponse { Message = "End time should be greater than start time." });

            var isDuplicate = await _context.TimeRegistrations.AnyAsync(tr =>
                tr.ProjectId == timeRegistration.ProjectId &&
                tr.StartTime <= timeRegistration.EndTime &&
                tr.EndTime >= timeRegistration.StartTime);

            if (isDuplicate)
                return BadRequest(new ApiResponse { Message = "Duplicate time registration for the same project." });

            _context.TimeRegistrations.Add(new TimeRegistration
            {
                ProjectId = timeRegistration.ProjectId,
                Date = timeRegistration.Date,
                EndTime = timeRegistration.EndTime,
                StartTime = timeRegistration.StartTime,
                TaskDescription = timeRegistration.TaskDescription
            });
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse { Message = "Time registration added successfully." });
        }

        private DateTime CombineDateAndTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }
    }
}