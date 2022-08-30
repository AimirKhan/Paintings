using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnalyticsHelper
{
    private AmplitudeAdapter m_AmplitudeAdapter = AmplitudeAdapter.Instance;

    private Dictionary<string, object> m_EventProp = new Dictionary<string, object>();

    //private string[] m_ArrayEventsStatus = new string[] { EventID.LevelStarted, EventID.LevelAborted };
    private string[] m_ArrayModeName = new string[] { UserProperties.ProgressFC, UserProperties.ProgressFP,
        UserProperties.ProgressLC, UserProperties.ProgressDM };

    private static readonly object instanceLock = new object();
    public static AnalyticsHelper Instance;
    private AnalyticsHelper() { }

    public static AnalyticsHelper GetInstance()
    {
        if (Instance == null)
        {
            lock (instanceLock)
            {
                if (Instance == null)
                {
                    Instance = new AnalyticsHelper();
                }
            }

        }
        return Instance;
    }

    // Initialization user properties
    public void SetOnceUserProperty()
    {
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.Progress, 0);
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.ProgressFC, 0);
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.ProgressFP, 0);
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.ProgressLC, 0);
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.ProgressDM, 0);
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.Stickers, 0);
        m_AmplitudeAdapter.SetOnceUserProperty(UserProperties.SubscriptionType, 0);
    }

    public void SendFirstLaunch()
    {
        m_AmplitudeAdapter.LogEvent(EventID.FirstLaunch);
    }

    /// <summary>
    /// Sending data after in-game purchase verification.
    /// </summary>
    /// <param name="productNumb">1 - month, 2 - year</param>
    /// <param name="totalInAp">Purchase quantity</param>
    /// <param name="paymentAmount">Payment amount</param>
    /// <param name="reason">Scene number</param>
    public void SendIAPData(int productNumb, string productID, int totalInAp, decimal paymentAmount, int reason)
    {
        if (productNumb == 1)
        {
            m_AmplitudeAdapter.SetUserProperty(UserProperties.SubscriptionType, 1);
        }
        else if (productNumb == 2)
        {
            m_AmplitudeAdapter.SetUserProperty(UserProperties.SubscriptionType, 2);
        }
        else throw new Exception("Wrong product ID.");

        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.ProductID, productID);
        m_EventProp.Add(EventProperties.TotalIAP, totalInAp);
        m_EventProp.Add(EventProperties.PaymentAmount, paymentAmount);
        m_EventProp.Add(EventProperties.Reason, reason);
        m_AmplitudeAdapter.LogEvent(EventID.InAppPurchase, m_EventProp);
    }

    /// <summary>
    /// Send start level.
    /// </summary>
    /// <param name="modeID">Game mode ID (FC = 0, FP = 1, LC = 2, DM = 3)</param>
    /// <param name="levelID">Level ID</param>
    ///// <param name="levelCount">How many times has the player already run this level?</param>
    public void SendLevelStarted(int modeID, int levelID)//, int levelCount)
    {
        if (modeID >= m_ArrayModeName.Length)
        {
            throw new Exception("Wrong mode ID.");
        }

        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.ModeID, modeID);
        m_EventProp.Add(EventProperties.LevelID, levelID);
        //m_EventProp.Add(EventProperties.LevelCount, levelCount);
        if(m_AmplitudeAdapter == null)
            m_AmplitudeAdapter = AmplitudeAdapter.GetInstance();
        m_AmplitudeAdapter.LogEvent(EventID.LevelStarted, m_EventProp);
    }

    /// <summary>
    /// Send finish level.
    /// </summary>
    /// <param name="modeID">Game mode ID (FC = 0, FP = 1, LC = 2, DM = 3).</param>
    /// <param name="levelID"></param>
    /// <param name="gotSticker"></param>
    /// <param name="countOfHelpers">How many times the helper was activated?</param>
    /// <param name="time">How many times has the player already run this level?</param>
    public void SendLevelFinished(int modeID, int levelID, bool gotSticker, int countOfHelpers, int time)
    {
        if (modeID >= m_ArrayModeName.Length)
        {
            throw new Exception("Wrong mode ID.");
        }

        m_AmplitudeAdapter.AddUserProperty(UserProperties.Progress, 1);
        m_AmplitudeAdapter.AddUserProperty(m_ArrayModeName[modeID], 1);
        if (gotSticker)
        {
            m_AmplitudeAdapter.AddUserProperty(UserProperties.Stickers, 1);
        }

        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.ModeID, modeID);
        m_EventProp.Add(EventProperties.LevelID, levelID);
        m_EventProp.Add(EventProperties.GotSticker, gotSticker);
        m_EventProp.Add(EventProperties.HelperInstances, countOfHelpers);
        m_EventProp.Add(EventProperties.Time, time);
        m_AmplitudeAdapter.LogEvent(EventID.LevelFinished, m_EventProp);
    }

    /// <summary>
    /// Send level aborted.
    /// </summary>
    /// <param name="modeID">Game mode ID (FC = 0, FP = 1, LC = 2, DM = 3).</param>
    /// <param name="levelID">Level ID</param>
    public void SendLevelAborted(int modeID, int levelID)
    {
        if(modeID >= m_ArrayModeName.Length)
        {
            throw new Exception("Wrong mode ID.");
        }

        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.ModeID, modeID);
        m_EventProp.Add(EventProperties.LevelID, levelID);
        m_AmplitudeAdapter.LogEvent(EventID.LevelAborted, m_EventProp);
    }

    public void SendSubscriptionCancelled()
    {
        m_AmplitudeAdapter.LogEvent(EventID.SubscriptionCancelled);
    }

    public void SendPushEnabled(bool status)
    {
        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.Status, status);
        m_AmplitudeAdapter.LogEvent(EventID.PushEnabled, m_EventProp);
    }

    public void SendStickersEntered(bool isNewStickers)
    {
        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.NewStickers, isNewStickers);
        m_AmplitudeAdapter.LogEvent(EventID.StickersEntered, m_EventProp);
    }

    /// <summary>
    /// Send loading complete.
    /// </summary>
    /// <param name="time">Time from starting the game to loading the main menu (seconds).</param>
    public void SendLoadingComplete(int time)
    {
        //Debug.Log("AnalyticsHelper");
        m_EventProp.Clear();
        m_EventProp.Add(EventProperties.Time, time);
        m_AmplitudeAdapter.LogEvent(EventID.LoadingComplete, m_EventProp);
    }

    public void SendResetProgress()
    {
        m_AmplitudeAdapter.SetUserProperty(UserProperties.Progress, 0);
        m_AmplitudeAdapter.SetUserProperty(UserProperties.ProgressFC, 0);
        m_AmplitudeAdapter.SetUserProperty(UserProperties.ProgressFP, 0);
        m_AmplitudeAdapter.SetUserProperty(UserProperties.ProgressLC, 0);
        m_AmplitudeAdapter.SetUserProperty(UserProperties.ProgressDM, 0);
        m_AmplitudeAdapter.SetUserProperty(UserProperties.Stickers, 0);
        m_AmplitudeAdapter.SetUserProperty(UserProperties.SubscriptionType, 0);
        m_AmplitudeAdapter.LogEvent(EventID.ResetProgress);
    }

    private struct EventID
    {
        public const string FirstLaunch = "first_launch";
        public const string InAppPurchase = "in_app_purchase";
        public const string LevelStarted = "level_started";
        public const string LevelFinished = "level_finished";
        public const string LevelAborted = "level_aborted";
        public const string SubscriptionCancelled = "subscription_cancelled";
        public const string PushEnabled = "push_enabled";
        public const string StickersEntered = "stickers_entered";
        public const string LoadingComplete = "loading_complete";
        public const string ResetProgress = "reset_progress";
    }

    private struct EventProperties
    {
        public const string Time = "time";
        public const string ProductID = "product_id";
        public const string TotalIAP = "total_iap";
        public const string PaymentAmount = "payment_amount";
        public const string Reason = "reason";
        public const string ModeID = "mode_id";
        public const string LevelID = "level_id";
        public const string LevelCount = "level_count";
        public const string GotSticker = "got_sticker";
        public const string HelperInstances = "helper_instances";
        public const string Status = "status";
        public const string NewStickers = "new_stickers";
    }

    private struct UserProperties
    {
        public const string Progress = "progress";
        public const string ProgressFC = "progress_FC";
        public const string ProgressFP = "progress_FP";
        public const string ProgressLC = "progress_LC";
        public const string ProgressDM = "progress_DM";
        public const string Stickers = "stickers";
        public const string SubscriptionType = "subscription_type";
    }
}
