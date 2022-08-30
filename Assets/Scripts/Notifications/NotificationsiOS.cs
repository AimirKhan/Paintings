using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif


public class NotificationsiOS : MonoBehaviour
{
#if UNITY_IOS
    [Inject] SaveLevelsComplete m_SaveLevelsComplete;
    [SerializeField] private int m_MinLimitTime = 22;
    [SerializeField] private int m_MaxLimitTime = 9;

    public void RequestPUSHes()
    {
        StartCoroutine(RequestAuthorization());
    }

    private void ClearNotifications()
    {
        print("PUSHes cleared");
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
    }



    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while(!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
            if (req.Granted)
            {
                AnalyticsHelper.Instance.SendPushEnabled(true);
            }
            else if (!req.Granted)
            {
                AnalyticsHelper.Instance.SendPushEnabled(false);
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        var levelsCounter = m_SaveLevelsComplete.m_LevelsComplete % 4;
        if (focus)
        {
            ClearNotifications();
        }
        else
        {
            int monthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            if (levelsCounter == 3)
            {
                for (int day = 1; day <= 7; day += 2)
                {
                    if (DateTime.Now.Day + day <= monthDays)
                    {
                        EveryDayNotification(DateTime.Now.Day + day, DateTime.Now.Month);
                        OneLevelForStickerPUSH(DateTime.Now.Day + day, DateTime.Now.Month);
                    }
                    else
                    {
                        var month = DateTime.Now.Month < 12 ? DateTime.Now.Month + 1 : 1;
                        var dayInNextMonth = DateTime.Now.Day + day - monthDays;
                        if (dayInNextMonth == 0)
                        {
                            continue;
                        }
                        EveryDayNotification(dayInNextMonth, month);
                        OneLevelForStickerPUSH(dayInNextMonth, month);
                    }

                }
                for (int day = 2; day <= 21; day += 2)
                {
                    if (DateTime.Now.Day + day <= monthDays)
                    {
                        //Debug.Log($"day = {day} ; month = {DateTime.Now.Month}");
                        EveryDayNotification(DateTime.Now.Day + day, DateTime.Now.Month);
                        OneLevelForStickerPUSH(DateTime.Now.Day + day, DateTime.Now.Month);

                    }
                    else
                    {
                        var nextMonth = DateTime.Now.Month < 12 ? DateTime.Now.Month + 1 : 1;
                        var dayInNextMonth = DateTime.Now.Day + day - monthDays;
                        if (dayInNextMonth == 0)
                        {
                            continue;
                        }
                        //Debug.Log($"day = {dayInNextMonth} ; month = {nextMonth}");
                        EveryDayNotification(dayInNextMonth, nextMonth);
                        OneLevelForStickerPUSH(dayInNextMonth, nextMonth);
                    }
                }
            }

            if (!Purchaser.isPremium())
            {
                for (int day = 2; day <= 21; day += 2)
                {
                    if (DateTime.Now.Day + day <= monthDays)
                    {
                        BuyPremiumPush(DateTime.Now.Day + day, DateTime.Now.Month);
                    }
                    else
                    {
                        var nextMonth = DateTime.Now.Month < 12 ? DateTime.Now.Month + 1 : 1;
                        var dayInNextMonth = DateTime.Now.Day + day - monthDays;
                        if (dayInNextMonth == 0)
                        {
                            continue;
                        }
                        //Debug.Log($"day = {dayInNextMonth} ; month = {nextMonth}");
                        BuyPremiumPush(dayInNextMonth, nextMonth);
                    }

                }
            }
        }
    }

    private void EveryDayNotification(int day, int month)
    {
        int tempMonth = month;
        int tempDay = day;
        int tempHour = DateTime.Now.Hour;
        Debug.Log($"day = {tempDay} ; month = {tempMonth}");

        int monthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

        // Set time for PUSH notification to 9:00 local time
        if (IsNight())
        {
            if (tempHour >= m_MinLimitTime)
            {
                tempDay++;
                if (tempDay > monthDays)
                {
                    tempDay = 1;
                    tempMonth = DateTime.Now.Month < 12 ? DateTime.Now.Month + 1 : 1;

                }
                tempHour = 9;
            }
            else if (tempHour < m_MaxLimitTime)
            {
                tempHour = 9;
            }
        }

        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Month = tempMonth,
            Day = tempDay,
            Hour = tempHour,
            Repeats = false
        };

        var everyDayNotification = new iOSNotification()
        {
            Title = "You haven't painted for a while.",
            Body = "Your animal friends are missing you!",
            ThreadIdentifier = "every_day_push",
            Trigger = calendarTrigger
        };

        iOSNotificationCenter.ScheduleNotification(everyDayNotification);
        print("Next push be: " + everyDayNotification.Data);
    }

    private void OneLevelForStickerPUSH(int day, int month)
    {
        int tempMonth = month;
        int tempDay = day;
        int tempHour = DateTime.Now.Hour;
        Debug.Log($"day = {tempDay} ; month = {tempMonth}");

        int monthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

        // Set time for PUSH notification to 9:00 local time
        if (IsNight())
        {
            if (tempHour >= m_MinLimitTime)
            {
                tempDay++;
                if (tempDay > monthDays)
                {
                    tempDay = 1;
                    tempMonth = DateTime.Now.Month < 12 ? DateTime.Now.Month + 1 : 1;

                }
                tempHour = 9;
            }
            else if (tempHour < m_MaxLimitTime)
            {
                tempHour = 9;
            }
        }

        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Month = tempMonth,
            Day = tempDay,
            Hour = tempHour,
            Repeats = false
        };

        var levelForSticker = new iOSNotification()
        {
            Title = "Complete one more level...",
            Body = "And get a new sticker for your album!",
            ThreadIdentifier = "last_sticker_push",
            Trigger = calendarTrigger
        };

        iOSNotificationCenter.ScheduleNotification(levelForSticker);
        print("Next push be: " + levelForSticker.Data);
    }

    void BuyPremiumPush(int day, int month)
    {
        int tempMonth = month;
        int tempDay = day;
        Debug.Log($"day = {tempDay} ; month = {tempMonth}");
        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Month = tempMonth,
            Day = tempDay,
            Hour = 18,
            Repeats = false
        };

        var premiumNotification = new iOSNotification()
        {
            Title = "Try premium levels for free!",
            Body = "50+ fancy new levels!",
            ThreadIdentifier = "premium_push",
            Trigger = calendarTrigger
        };

        iOSNotificationCenter.ScheduleNotification(premiumNotification);
        //        print("Next push scheduled to: " + calendarTrigger);
    }

    private bool IsNight()
    {
        int curHour = DateTime.Now.Hour;
        if (curHour >= m_MinLimitTime || curHour < m_MaxLimitTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
#endif
}