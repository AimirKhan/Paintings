using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationsAndroid : MonoBehaviour
{
#if UNITY_ANDROID
    [Inject] SaveLevelsComplete m_SaveLevelsComplete;
    private string m_ChannelId = "channel_id";
    private string m_SmallIconName = "icon_small";
    private string m_LargeIconName = "icon_large";
    [SerializeField] private int m_MinLimitTime = 22;
    [SerializeField] private int m_MaxLimitTime = 9;

    private void Awake()
    {
        InitializeChannel();
    }

    public void ClearScheduledNotifications()
    {
        print("PUSHes cleared");
        AndroidNotificationCenter.CancelAllNotifications();
    }

    public void InitializeChannel()
    {
        AndroidNotificationChannel notificationsChannel = new AndroidNotificationChannel()
        {
            Id = m_ChannelId,
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic Notifications",

        };
        AndroidNotificationCenter.RegisterNotificationChannel(notificationsChannel);
    }

    private void OnApplicationFocus(bool focus)
    {
        var levelsCounter = m_SaveLevelsComplete.m_LevelsComplete % 3;
        if (focus)
        {
            ClearScheduledNotifications();
        }
        else
        {
            if (levelsCounter == 2)
            {
                for (int day = 1; day < 7; day++)
                {
                    //int cur = i;
                    EveryDayNotification(day, false);
                }
                EveryDayNotification(7, true);
                OneLevelForStickerPUSH();
            }
            BuyPremiumPush();
        }
    }

    public void EveryDayNotification(int days, bool everyDay)
    {
        DateTime now = DateTime.Now;
        AndroidNotification everyDayPUSH = new AndroidNotification();
        everyDayPUSH.Title = "You haven't painted for a while.";
        everyDayPUSH.Text = "Your animal friends are missing you!";
        everyDayPUSH.SmallIcon = m_SmallIconName;
        everyDayPUSH.LargeIcon = m_LargeIconName;
        if (IsNight())
        {
            // Set time for PUSH notification to 9:00 local time
            if (now.Hour >= m_MinLimitTime)
            {
                everyDayPUSH.FireTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(days);
            }
            else if (now.Hour < m_MaxLimitTime)
            {
                everyDayPUSH.FireTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(days - 1);
            }
        }
        else
        {
            everyDayPUSH.FireTime = DateTime.Now.AddDays(days);
        }

        if (everyDay)
        {
            everyDayPUSH.RepeatInterval = new TimeSpan(2, 0, 0, 0);
        }

        var notificationId = AndroidNotificationCenter.
            SendNotification(everyDayPUSH, m_ChannelId);

        var notificationStatus = AndroidNotificationCenter.
            CheckScheduledNotificationStatus(notificationId);

        print("Everyday PUSH Scheduled to: " + everyDayPUSH.FireTime + ", with interval: " +
            everyDayPUSH.RepeatInterval + ", with id: " + notificationId);
    }

    public void OneLevelForStickerPUSH()
    {
        DateTime now = DateTime.Now;
        AndroidNotification levelForSticker = new AndroidNotification();
        levelForSticker.Title = "Complete one more level...";
        levelForSticker.Text = "And get a new sticker for your album!";
        levelForSticker.SmallIcon = m_SmallIconName;
        levelForSticker.LargeIcon = m_LargeIconName;
        if (IsNight())
        {
            // Set time for PUSH notification to 9:00 local time
            if (now.Hour >= m_MinLimitTime)
            {
                levelForSticker.FireTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1);
            }
            else if (now.Hour < m_MaxLimitTime)
            {
                levelForSticker.FireTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
            }
        }
        else
        {
            levelForSticker.FireTime = DateTime.Now.AddDays(1);
        }
        levelForSticker.RepeatInterval = new TimeSpan(1, 0, 0, 0);

        var notificationId = AndroidNotificationCenter.
            SendNotification(levelForSticker, m_ChannelId);

        var notificationStatus = AndroidNotificationCenter.
            CheckScheduledNotificationStatus(notificationId);

        print("One level sticker PUSH Scheduled to: " + levelForSticker.FireTime + ", with interval: " +
            levelForSticker.RepeatInterval + ", with id: " + notificationId);
    }

    public void BuyPremiumPush()
    {
        DateTime now = DateTime.Now;
        AndroidNotification premiumNotification = new AndroidNotification();
        premiumNotification.Title = "Try premium levels for free!";
        premiumNotification.Text = "80+ fancy new levels!";
        premiumNotification.SmallIcon = m_SmallIconName;
        premiumNotification.LargeIcon = m_LargeIconName;
        // Set PUSH to 18:00 Local time every 2 days
        premiumNotification.FireTime = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0).AddDays(1);
        premiumNotification.RepeatInterval = new TimeSpan(2, 0, 0, 0);

        var notificationId = AndroidNotificationCenter.
            SendNotification(premiumNotification, m_ChannelId);

        var notificationStatus = AndroidNotificationCenter.
            CheckScheduledNotificationStatus(notificationId);

        print("Premium PUSH Scheduled to: " + premiumNotification.FireTime + ", with interval: " +
            premiumNotification.RepeatInterval + ", with id: " + notificationId);
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