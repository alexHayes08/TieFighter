using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TieFighter.Models
{
    public enum MedalConditionTypes
    {
        /// <summary>
        /// TimeSpan is actually uses the type DateTime.
        /// </summary>
        TimeSpan,
        KillCount,
        TotalTravelDistance,
        WithoutDying,
        StatAt
    }

    // Get x kills all within timespan
    // Survive x amount of time
    // Get x kills
    public class MedalCondition
    {
        public Object ConditionValue { get; set; }
        public MedalConditionTypes ConditionType { get; set; }
        public MedalCondition DependsOn { get; set; }

        public virtual void ActivationTrigger()
        { }
    }
}
