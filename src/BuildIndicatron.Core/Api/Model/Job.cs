﻿using System;
using System.Collections.Generic;

namespace BuildIndicatron.Core.Api.Model
{
    public class Job
    {
        public const string FailInColor = "red_anime";
        public const string FailColor = "red";
        public const string SuccessColor = "blue";
        

        public string Name { get; set; }
        public string Color { get; set; }
        public string Url { get; set; }
        public List<Health> HealthReport { get; set; }
        public List<Build> Builds { get; set; }
        
        public LastFailedBuild LastFailedBuild { get; set; }

        public bool IsProcessing()
        {
            return Color.Contains("_anime");
        }

        public bool IsPassed()
        {
            return Color.Contains(SuccessColor);
        }

        public bool IsFailed()
        {
            return Color.Contains(FailColor) || Color.Contains("yellow");
        }
    }

    public class Build
    {
        public int Duration { get; set; }
        public string Result { get; set; }
    }

    public class Health
    {
        public int Score { get; set; }
    }
}