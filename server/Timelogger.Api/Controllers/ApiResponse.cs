﻿using System;
using System.Collections.Generic;

namespace Timelogger.Api.Controllers
{
    public class ApiResponse
    {
        public string Message { get; internal set; }
        public List<string> Errors { get; internal set; }
    }

    public class TimeRegistrationDto
    {
        public int? Id { get; set; }
        public int ProjectId { get; set; }
        
        public int FreelancerId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}