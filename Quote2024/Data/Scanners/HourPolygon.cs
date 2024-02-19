﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Data.Actions.Polygon;
using Data.Helpers;

namespace Data.Scanners
{
    public class HourPolygon
    {
        private const float AveragePeriod = 21f;
        private const float AveragePeriodDelta = 2.6f;
        private static readonly DateTime FromDate = new DateTime(2022, 1, 1);
        private static readonly DateTime ToDate = new DateTime(2099, 12, 29);

        public static void Start()
        {
            var timeCommon = "09:30,10:00,11:00,12:00,13:00,14:00,15:00,15:45,16:00".Split(',').Select(TimeSpan.Parse).ToArray();
            var timeRangeCommon = new List<(TimeSpan, TimeSpan)>();
            for (var k = 0; k < timeCommon.Length - 1; k++)
                timeRangeCommon.Add((timeCommon[k], timeCommon[k + 1]));

            var timeShortened = "09:30,10:00,11:00,12:00,12:45,13:00".Split(',').Select(TimeSpan.Parse).ToArray();

            var timeRangeShortened = new List<(TimeSpan, TimeSpan)>();
            for (var k = 0; k < timeShortened.Length - 1; k++)
                timeRangeShortened.Add((timeShortened[k], timeShortened[k + 1]));

            var allResults = new List<HourPolygon>();
            var resultsCount = 0;

            DbUtils.ClearAndSaveToDbTable(allResults, "dbQ2024..HourPolygon", "Symbol", "Date", "Time", "To", "Open",
                "High", "Low", "Close", "Volume", "TradeCount", "Count", "OpenNext", "OpenNextAt", "PrevWma_20", "Wma_20",
                "CloseAtWma_20", "PrevWma_30", "Wma_30", "CloseAtWma_30", "PrevEma_20", "Ema_20",
                "CloseAtEma_20", "PrevEma_30", "Ema_30", "CloseAtEma_30", "PrevEma2_20", "Ema2_20",
                "CloseAtEma2_20", "PrevEma2_30", "Ema2_30", "CloseAtEma2_30");

            foreach (var oo in Data.Actions.Polygon.PolygonMinuteScan.GetQuotes(FromDate, ToDate))
            {
                var symbol = oo.Item1;
                var date = oo.Item2;
                var ticksFrom = CsUtils.GetUnixMillisecondsFromEstDateTime(date.Add(Settings.MarketStart)); // Unix ticks for 9:30
                var tickTo = CsUtils.GetUnixMillisecondsFromEstDateTime(date.Add(Settings.GetMarketEndTime(date)));
                var quotes = oo.Item3.Where(a => a.t >= ticksFrom && a.t < tickTo && a.IsValid).ToArray();
                var closes = quotes.Select(q => q.c).ToArray();
                var wma_20 = StatMethods.Wma(closes, 20);
                var wma_30 = StatMethods.Wma(closes, 30);
                var ema_20 = StatMethods.Ema(closes, 20);
                var ema_30 = StatMethods.Ema(closes, 30);
                var ema2_20 = StatMethods.Ema2(closes, 20);
                var ema2_30 = StatMethods.Ema2(closes, 30);

                var timeRange = Settings.ShortenedDays.ContainsKey(date) ? timeRangeShortened : timeRangeCommon;
                var results = new List<HourPolygon>();

                foreach (var o1 in timeRange)
                {
                    resultsCount += results.Count;
                    var result = new HourPolygon(symbol, date, o1);
                    results.Add(result);

                    var fromUnixTicks = CsUtils.GetUnixMillisecondsFromEstDateTime(date.Add(o1.Item1));
                    var toUnixTicks = CsUtils.GetUnixMillisecondsFromEstDateTime(date.Add(o1.Item2));
                    var countFull = 0;
                    bool? averageUp = null;
                    bool? wma_20_Up = null;
                    bool? wma_30_Up = null;
                    bool? ema_20_Up = null;
                    bool? ema_30_Up = null;
                    bool? ema2_20_Up = null;
                    bool? ema2_30_Up = null;

                    var average = 0f;
                    foreach (var quote in quotes)
                    {
                        if (quote.t < fromUnixTicks)
                        {
                            result.PrevAverage = average;
                            result.PrevWma_20 = wma_20[countFull];
                            result.PrevWma_30 = wma_30[countFull];
                            result.PrevEma_20 = ema_20[countFull];
                            result.PrevEma_30 = ema_30[countFull];
                            result.PrevEma2_20 = ema2_20[countFull];
                            result.PrevEma2_30 = ema2_30[countFull];
                        }
                        else if (quote.t < toUnixTicks)
                        {
                            result.Count++;
                            result.TradeCount += quote.n;
                            result.Volume += quote.v;

                            if (result.Count == 1)
                            {
                                result.Open = quote.o;
                                result.High = quote.h;
                                result.Low = quote.l;
                                result.Close = quote.c;

                                result.Average = average;
                                result.Wma_20 = wma_20[countFull];
                                result.Wma_30 = wma_30[countFull];
                                result.Ema_20 = ema_20[countFull];
                                result.Ema_30 = ema_30[countFull];
                                result.Ema2_20 = ema2_20[countFull];
                                result.Ema2_30 = ema2_30[countFull];

                                if (average > 0 && (result.Open - 0.01f) > average)
                                    averageUp = true;
                                else if (average > 0 && (result.Open + 0.01f) < average)
                                    averageUp = false;

                                wma_20_Up = GetAverageUpDown(wma_20[countFull]);
                                wma_30_Up = GetAverageUpDown(wma_30[countFull]);
                                ema_20_Up = GetAverageUpDown(ema_20[countFull]);
                                ema_30_Up = GetAverageUpDown(ema_30[countFull]);
                                ema2_20_Up = GetAverageUpDown(ema2_20[countFull]);
                                ema2_30_Up = GetAverageUpDown(ema2_30[countFull]);

                                bool? GetAverageUpDown(float aver)
                                {
                                    if (aver > 0 && (result.Open - 0.01f) > aver) return true;
                                    if (aver > 0 && (result.Open + 0.01f) < aver) return false;
                                    return null;
                                }
                            }
                            else
                            {
                                if (result.Count == 2)
                                {
                                    result.OpenNext = quote.o;
                                    result.OpenNextAt = CsUtils.GetEstDateTimeFromUnixMilliseconds(quote.t).TimeOfDay;
                                }

                                if (quote.h > result.High) result.High = quote.h;
                                if (quote.l < result.Low) result.Low = quote.l;
                                result.Close = quote.c;
                            }

                            if (averageUp.HasValue && !result.CloseAtAverage.HasValue)
                            {
                                if (averageUp == true && quote.l <= average)
                                    result.CloseAtAverage = quote.l;
                                else if (averageUp == false && quote.h >= average)
                                    result.CloseAtAverage = quote.h;
                            }

                            result.CloseAtWma_20 ??= GetCloseAtAver(wma_20[countFull], wma_20_Up);
                            result.CloseAtWma_30 ??= GetCloseAtAver(wma_30[countFull], wma_30_Up);
                            result.CloseAtEma_20 ??= GetCloseAtAver(ema_20[countFull], ema_20_Up);
                            result.CloseAtEma_30 ??= GetCloseAtAver(ema_30[countFull], ema_30_Up);
                            result.CloseAtEma2_20 ??= GetCloseAtAver(ema2_20[countFull], ema2_20_Up);
                            result.CloseAtEma2_30 ??= GetCloseAtAver(ema2_30[countFull], ema2_30_Up);

                            float? GetCloseAtAver(float aver, bool? averUp)
                            {
                                if (averUp == true && quote.l <= aver)
                                    return quote.l;
                                if (averUp == false && quote.h >= aver)
                                    return quote.h;
                                return null;
                            }
                        }

                        if (countFull == 0)
                            average = GetAveragePrice(quote);
                        else
                            average = (average * (AveragePeriod - AveragePeriodDelta) + AveragePeriodDelta * GetAveragePrice(quote)) / AveragePeriod;

                        countFull++;
                    }

                    if (averageUp.HasValue && !result.CloseAtAverage.HasValue)
                        result.CloseAtAverage = result.Close;
                }

                allResults.AddRange(results);
                if (allResults.Count > 100000)
                {
                    DbUtils.SaveToDbTable(allResults, "dbQ2024..HourPolygon", "Symbol", "Date", "Time", "To", "Open",
                        "High", "Low", "Close", "Volume", "TradeCount", "Count", "OpenNext", "OpenNextAt", "PrevWma_20", "Wma_20",
                        "CloseAtWma_20", "PrevWma_30", "Wma_30", "CloseAtWma_30", "PrevEma_20", "Ema_20",
                        "CloseAtEma_20", "PrevEma_30", "Ema_30", "CloseAtEma_30", "PrevEma2_20", "Ema2_20",
                        "CloseAtEma2_20", "PrevEma2_30", "Ema2_30", "CloseAtEma2_30");
                    allResults.Clear();
                }
            }

            if (allResults.Count > 0)
            {
                DbUtils.SaveToDbTable(allResults, "dbQ2024..HourPolygon", "Symbol", "Date", "Time", "To", "Open",
                    "High", "Low", "Close", "Volume", "TradeCount", "Count", "OpenNext", "OpenNextAt", "PrevWma_20", "Wma_20",
                    "CloseAtWma_20", "PrevWma_30", "Wma_30", "CloseAtWma_30", "PrevEma_20", "Ema_20",
                    "CloseAtEma_20", "PrevEma_30", "Ema_30", "CloseAtEma_30", "PrevEma2_20", "Ema2_20",
                    "CloseAtEma2_20", "PrevEma2_30", "Ema2_30", "CloseAtEma2_30");
                allResults.Clear();
            }

            Debug.Print($"Results count: {resultsCount}");

            float GetAveragePrice(PolygonCommon.cMinuteItem q) => (q.o + q.h + q.l + q.c) / 4.0f;
        }

        #region =========  Instance  ==========

        public string Symbol;
        public DateTime Date;
        public TimeSpan Time;
        public TimeSpan To;
        public float Open;
        public float High;
        public float Low;
        public float Close;
        public float Volume;

        public int TradeCount;
        public byte Count;

        public float OpenNext;
        public TimeSpan? OpenNextAt;

        public float PrevAverage;
        public float Average;
        public float? CloseAtAverage;

        public float PrevWma_20;
        public float Wma_20;
        public float? CloseAtWma_20;

        public float PrevWma_30;
        public float Wma_30;
        public float? CloseAtWma_30;

        public float PrevEma_20;
        public float Ema_20;
        public float? CloseAtEma_20;

        public float PrevEma_30;
        public float Ema_30;
        public float? CloseAtEma_30;

        public float PrevEma2_20;
        public float Ema2_20;
        public float? CloseAtEma2_20;

        public float PrevEma2_30;
        public float Ema2_30;
        public float? CloseAtEma2_30;

        private long _fromUnixMilliseconds;
        private long _toUnixMilliseconds;

        public HourPolygon(string symbol, DateTime date, (TimeSpan, TimeSpan) FromTo)
        {
            Symbol = symbol;
            Date = date;
            Time = FromTo.Item1;
            To = FromTo.Item2;
            _fromUnixMilliseconds = CsUtils.GetUnixMillisecondsFromEstDateTime(date.Add(Time));
            _toUnixMilliseconds = CsUtils.GetUnixMillisecondsFromEstDateTime(date.Add(To));
        }
        #endregion
    }
}
