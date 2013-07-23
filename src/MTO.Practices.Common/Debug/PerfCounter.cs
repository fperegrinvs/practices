namespace MTO.Practices.Common.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class PerfCounter
    {
        private static Dictionary<string, PerfMeasure> routines = new Dictionary<string, PerfMeasure>();

        public static void MeasureAsync(string routine, long time)
        {
            new Task(() => Measure(routine, time)).Start();
        }

        private static void Measure(string routine, long time)
        {
            if (!routines.ContainsKey(routine))
            {
                routines[routine] = new PerfMeasure();
            }

            routines[routine].Add(time);
        }

        public static List<KeyValuePair<string,PerfMeasure>> Report()
        {
            return routines.Select(x => new KeyValuePair<string, PerfMeasure>(x.Key, x.Value)).ToList();
        }

        public static void MeasureAsync(this Stopwatch watch, string routine)
        {
            if (watch != null)
            {
                if (watch.ElapsedMilliseconds > 1)
                {
                    MeasureAsync(routine, watch.ElapsedMilliseconds);
                }

                watch.Stop();
            }
        }

        public static void StopNullable(this Stopwatch watch)
        {
            if (watch != null)
            {
                watch.Stop();
            }
        }

        public static Stopwatch StartNew()
        {
#if !TRACE
            return null;
#else
            return Stopwatch.StartNew();
#endif
        }
    }

    public class PerfMeasure
    {
        private static object lockKey = new object();

        private List<DateTime> measurements = new List<DateTime>();

        private List<long> times = new List<long>();

        private List<long> count = new List<long>();

        private List<long> max = new List<long>();

        private List<long> min = new List<long>(); 

        public List<List<string>> Report()
        {
            var result = new List<List<string>>();
            lock(lockKey)
            {
                for (int i = 0; i < measurements.Count; i++)
                {
                    var line = new List<string>();
                    line.Add(measurements[i].ToString());
                    line.Add(max[i].ToString());
                    line.Add((times[i] / count[i]).ToString());
                    line.Add(min[i].ToString());
                    line.Add(count[i].ToString());
                    result.Add(line);
                }
            }

            return result;
        }

        internal void Add(long time)
        {
            lock(lockKey)
            {
                var key = GetKey();
                if (!measurements.Any(x => x == key))
                {
                    measurements.Insert(0, key);
                    times.Insert(0, time);
                    max.Insert(0, time);
                    min.Insert(0, time);
                    count.Insert(0, 1);

                    if (measurements.Count > 10)
                    {
                        measurements.RemoveAt(measurements.Count - 1);
                        times.RemoveAt(times.Count - 1);
                        count.RemoveAt(count.Count - 1);
                        max.RemoveAt(measurements.Count - 1);
                        min.RemoveAt(measurements.Count - 1);
                    }
                }
                else
                {
                    var idx = measurements.IndexOf(key);
                    times[idx] += time;
                    count[idx]++;
                    max[idx] = max[idx] > time ? max[idx] : time;
                    min[idx] = min[idx] < time ? min[idx] : time;
                }
            }
        }

        private DateTime GetKey()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
        }
    }
}
