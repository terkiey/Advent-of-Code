namespace AoC.Days;

internal class GuardSleepingPatternTracker
{
    private Dictionary<int, HashSet<DateOnly>> GuardShiftAssignment = [];
    private Dictionary<DateOnly, SleepTrackingEnum[]> ShiftSleepingEvents = [];
    private Dictionary<DateOnly, bool[]> ShiftSleepTracker = [];
    private const int shiftLength = 60;
    
    public void ParseData(string[] inputLines)
    {
        foreach (string info in inputLines)
        {
            string[] splitInfo = info.Split("] ");
            // Parse the time/date of the info.
            string dateTimeString = splitInfo[0][1..];
            int year = int.Parse(dateTimeString[..4]);
            int month = int.Parse(dateTimeString[5..7]);
            int day = int.Parse(dateTimeString[8..10]);
            int hour = int.Parse(dateTimeString[^5..^3]);
            int minute = int.Parse(dateTimeString[^2..]);
            DateOnly logDate = new(year, month, day);
            
            // Log all events into an object that contains the type of event, and when it occurred.
            // Some guards start their shift before midnight, but we are only interested in post-midnight, so just add one to the day to allocate the 'day' of the shift.
            if (hour == 23)
            {
                logDate = logDate.AddDays(1);
            }

            string eventInfo = splitInfo[1];
            if (eventInfo[6] == '#')
            {
                int guardId = int.Parse(eventInfo.Split(' ')[1][1..]);
                GuardShiftAssignment.TryAdd(guardId, []);
                GuardShiftAssignment[guardId].Add(logDate);
            }
            // If its not a guard starting a shift, then assign the initial sleep tracking states from the input.
            else
            {
                SleepTrackingEnum[] sleepTracking;
                if (!ShiftSleepingEvents.ContainsKey(logDate))
                {
                    ShiftSleepingEvents.Add(logDate, new SleepTrackingEnum[shiftLength]);
                    sleepTracking = ShiftSleepingEvents[logDate];
                    for (int i = 0; i < shiftLength; i++)
                    {
                        sleepTracking[i] = SleepTrackingEnum.Unspecified;
                    }
                }

                SleepTrackingEnum[] sleepTrackingAssigned = ShiftSleepingEvents[logDate];
                switch (eventInfo[0])
                {
                    case 'f':
                        sleepTrackingAssigned[minute] = SleepTrackingEnum.Asleep;
                        break;

                    case 'w':
                        sleepTrackingAssigned[minute] = SleepTrackingEnum.Awake;
                        break;

                    default:
                        throw new ArgumentException("Only expecting f or w");
                }
            }
        }

        // Now, given the input sleep tracking states, for each shift, calculate whether the guard was asleep or awake at each minute and fill it all in.
        // (I am currently assuming no fall asleep event lands in the 23rd hour of the day before in that small window after a shift starts, may bite me later).
        // So assuming they start at 00:00 awake based on that...

        foreach (var kvp in ShiftSleepingEvents)
        {
            SleepTrackingEnum[] sleepTrackerInit = kvp.Value;
            bool awake = true;
            bool[] newTracker = new bool[shiftLength];
            for (int minute = 0; minute < shiftLength; minute++)
            {
                switch (sleepTrackerInit[minute])
                {
                    case SleepTrackingEnum.Asleep:
                        awake = false;
                        break;

                    case SleepTrackingEnum.Awake:
                        awake = true;
                        break;
                }
                newTracker[minute] = awake ? false : true;
            }
            ShiftSleepTracker[kvp.Key] = newTracker;
        }

    }

    public int AnswerOne()
    {
        int maxSleep = int.MinValue;
        int maxSleepGuardId = -1;
        int maxSleepCommonMinute = -1;
        foreach(var kvp in GuardShiftAssignment)
        {
            int guardId = kvp.Key;
            HashSet<DateOnly> shifts = kvp.Value;
            int sleepCount = 0;
            Dictionary<int, int> minuteSleepCount = [];
            for (int minute = 0; minute < shiftLength; minute++)
            {
                minuteSleepCount[minute] = 0;
            }

            int commonMinute = -1;
            int commonMinuteHits = 0;
            foreach (var shift in shifts)
            {
                if (!ShiftSleepTracker.TryGetValue(shift, out bool[]? shiftSleepTracker))
                {
                    continue;
                }

                for (int minute = 0; minute < shiftLength; minute++)
                {
                    if (shiftSleepTracker[minute])
                    {
                        sleepCount++;
                        minuteSleepCount[minute]++;
                        if (minuteSleepCount[minute] > commonMinuteHits)
                        {
                            commonMinuteHits = minuteSleepCount[minute];
                            commonMinute = minute;
                        }
                    }
                }
            }

            if (sleepCount > maxSleep)
            {
                maxSleep = sleepCount;
                maxSleepGuardId = guardId;
                maxSleepCommonMinute = commonMinute;
            }
        }

        return maxSleepGuardId * maxSleepCommonMinute;
    }

    public int AnswerTwo()
    {
        int modeMinute = -1;
        int modeMinuteHits = int.MinValue;
        int modeMinuteGuard = -1;

        foreach (var kvp in GuardShiftAssignment)
        {
            int guardId = kvp.Key;
            HashSet<DateOnly> shifts = kvp.Value;
            Dictionary<int, int> minuteSleepCount = [];
            for (int minute = 0; minute < shiftLength; minute++)
            {
                minuteSleepCount[minute] = 0;
            }

            int commonMinute = -1;
            int commonMinuteHits = 0;
            foreach (var shift in shifts)
            {
                if (!ShiftSleepTracker.TryGetValue(shift, out bool[]? shiftSleepTracker))
                {
                    continue;
                }

                for (int minute = 0; minute < shiftLength; minute++)
                {
                    if (shiftSleepTracker[minute])
                    {
                        minuteSleepCount[minute]++;
                        if (minuteSleepCount[minute] > commonMinuteHits)
                        {
                            commonMinuteHits = minuteSleepCount[minute];
                            commonMinute = minute;
                        }
                    }
                }
            }

            if (commonMinuteHits > modeMinuteHits)
            {
                modeMinute = commonMinute;
                modeMinuteHits = commonMinuteHits;
                modeMinuteGuard = guardId;
            }
        }

        return modeMinute * modeMinuteGuard;
    }
}
