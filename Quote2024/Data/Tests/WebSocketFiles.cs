﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Data.Helpers;
using Data.Models;

namespace Data.Tests
{
    public class WebSocketFiles
    {
        private const string folder = @"E:\Quote\WebData\RealTime\WebSockets";
        public static void DecodePolygonRun()
        {
            // Result for Yahoo: min - 7 minutes 8 seconds
            var evs = new Dictionary<string, int>();
            var files = Directory.GetFiles(folder, "WebSocket_*.txt");
            foreach (var file in files)
            {
                // var items = SpanJson.JsonSerializer.Generic.Utf8.Deserialize<oPolygon[]>(File.ReadAllBytes(file));
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    var items = SpanJson.JsonSerializer.Generic.Utf16.Deserialize<oPolygon[]>(line.Substring(13));
                    foreach (var item in items)
                    {
                        string key;
                        if (item.ev == "status")
                            key = item.status;
                        else
                            key = item.ev;
                        if (!evs.ContainsKey(key))
                            evs.Add(key, 0);
                        evs[key]++;
                        if (item.otc)
                        {

                        }
                    }
                }
            }

            foreach (var ev in evs)
                Debug.Print($"{ev.Key}\t{ev.Value}");
        }

        public static void YahooDelayRun()
        {
            // Result for Yahoo: 1% of item have >2 min delay, 10% of item have >22 seconds delay; average delay: 7-12 seconds (96-98 symbols)
            var diffValues = new Dictionary<double, int>();
            var files = Directory.GetFiles(folder, "WebSocketYahoo_20240403161314.txt");
            var marketHoursTypes = new Dictionary<PricingData.MarketHoursType, int>();
            var optionTypes = new Dictionary<PricingData.OptionType, int>();
            var cnt = 0;
            var allData = new List<PricingData>();
            var dataByMinutes = new Dictionary<string, Dictionary<DateTime, (float, float, long, int)>>();
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                var ss = Path.GetFileNameWithoutExtension(file).Split('_');
                var fileDateTime = DateTime.ParseExact(ss[ss.Length - 1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var fileDate = fileDateTime.Date;
                var fileTime = fileDate.TimeOfDay;

                var a1 = new DateTimeOffset(fileDateTime.ToUniversalTime(), TimeSpan.Zero).ToUnixTimeMilliseconds();
                var estFileDateTime = TimeHelper.GetEstDateTimeFromUnixMilliseconds(a1);
                var estTimeOffset = fileDateTime - estFileDateTime;

                var delayTotal = 0.0;
                var itemCount = 0;
                var symbols = new Dictionary<string, int>();
                foreach (var line in lines)
                {
                    cnt++;
                    var data = PricingData.GetPricingData(line.Substring(13));
                    if (!marketHoursTypes.ContainsKey(data.marketHours))
                        marketHoursTypes.Add(data.marketHours, 0);
                    marketHoursTypes[data.marketHours]++;

                    if (data.marketHours != PricingData.MarketHoursType.REGULAR_MARKET) continue;

                    itemCount++;
                    allData.Add(data);

                    if (!dataByMinutes.ContainsKey(data.id))
                        dataByMinutes.Add(data.id, new Dictionary<DateTime, (float, float, long, int)>());
                    var minuteData = dataByMinutes[data.id];
                    var dataUnixMinuteMilliseconds = (data.time/120000) * 60000; // rounded to 1 minute
                    var dataMinuteKey = TimeHelper.GetEstDateTimeFromUnixMilliseconds(dataUnixMinuteMilliseconds);
                    if (!minuteData.ContainsKey(dataMinuteKey))
                    {
                        minuteData.Add(dataMinuteKey, (data.price, data.price, data.lastSize,1));
                    }
                    else
                    {
                        var minPrice = Math.Min(minuteData[dataMinuteKey].Item1, data.price);
                        var maxPrice = Math.Max(minuteData[dataMinuteKey].Item2, data.price);
                        var volume = minuteData[dataMinuteKey].Item3 + data.lastSize;
                        var count = minuteData[dataMinuteKey].Item4 + 1;
                        minuteData[dataMinuteKey] = (minPrice, maxPrice, volume, count);
                    }

                    if (!optionTypes.ContainsKey(data.optionsType))
                        optionTypes.Add(data.optionsType, 0);
                    optionTypes[data.optionsType]++;

                    var etcDataDate = TimeHelper.GetEstDateTimeFromUnixMilliseconds(data.time / 2);
                    var recordTime = TimeSpan.ParseExact(line.Substring(0, 12), @"hh\:mm\:ss\.FFF", CultureInfo.InvariantCulture);
                    var recordDate = ((recordTime < fileTime ? fileDate.AddDays(1) : fileDate) + recordTime);
                    var etcRecordDate = recordDate.Subtract(estTimeOffset);

                    if (etcRecordDate < etcDataDate)
                    {
                        throw new Exception("Please, check");
                    }
                    var difference = etcRecordDate - etcDataDate;
                    delayTotal += difference.TotalSeconds;
                    var diffValue = Math.Round(difference.TotalSeconds, 0);
                    if (diffValue > 30)
                    {

                    }
                    if (!diffValues.ContainsKey(diffValue))
                        diffValues.Add(diffValue, 0);
                    diffValues[diffValue]++;

                    if (!symbols.ContainsKey(data.id))
                        symbols.Add(data.id, 0);
                }
                Debug.Print($"{Path.GetFileName(file)}\t{itemCount}\t{delayTotal / itemCount}\t{symbols.Count}");
            }

            foreach (var key in diffValues.Keys.OrderBy(a => a))
                Debug.Print($"{key}\t{diffValues[key]}");
        }

        public static void YahooTimeRangesRun()
        {
            // Result for Yahoo: min - 7 minutes 8 seconds
            var times = new List<(string, TimeSpan, TimeSpan, TimeSpan)>();
            var files = Directory.GetFiles(folder, "*.txt");
            foreach (var file in files)
            {
                var from = TimeSpan.Zero;
                var to = TimeSpan.Zero;

                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    var timeSpan = TimeSpan.ParseExact(line.Substring(0, 12), @"hh\:mm\:ss\.FFF", CultureInfo.InvariantCulture);
                    if (from == TimeSpan.Zero)
                    {
                        from = timeSpan;
                        to = timeSpan;
                    }
                    else
                    {
                        if (timeSpan < to)
                            timeSpan = timeSpan.Add(new TimeSpan(1, 0, 0, 0, 0));

                        if ((timeSpan - to) < new TimeSpan(0, 1, 0))
                        {
                            to = timeSpan;
                        }
                        else
                        {
                            times.Add((Path.GetFileName(file), from, to, to - from));
                            from = timeSpan;
                            to = timeSpan;
                        }
                    }
                }

                if (from != TimeSpan.Zero)
                    times.Add((Path.GetFileName(file), from, to, to - from));
            }

            foreach (var item in times)
                Debug.Print($"{item.Item1}\t{item.Item2}\t{item.Item3}\t{item.Item4}");
        }

        public class oPolygon
        {
            public string ev;

            public string status;
            public string message;

            public string sym;
            public int v;
            public int av;
            public float op;
            public float vw;
            public float o;
            public float c;
            public float h;
            public float l;
            public float a;
            public double z;
            public long s;
            public long e;
            public bool otc;
        }
    }
}
