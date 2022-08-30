using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNames
{
    [System.Serializable]
    public struct EventID
    {
        public const string FirstLaunch = "first_launch";
        public const string InAppPurchase = "in_app_purchase";
        public const string LevelStarted = "level_started";
        public const string LevelFinished = "level_finished";
        public const string LevelAborted = "level_aborted";
        public const string LevelLocked = "level_locked";
        public const string SubscriptionCancelled = "subscription_cancelled";
        public const string PushDisabled = "push_disabled";
        public const string StickersEntered = "stickers_entered";
    }

    [System.Serializable]
    public struct EventProperties
    {
        public const string Time = "time";
        public const string ProductID = "product_id";
        public const string IsFirstIAP = "is_first_iap";
        public const string IsSecondIAP = "is_second_iap";
        public const string TotalIAP = "total_iap";
        public const string PaymentAmount = "payment_amount";
        public const string Reason = "reason";
        public const string ModeID = "mode_id";
        public const string LevelID = "level_id";
        public const string GotSticker = "got_sticker";
        public const string HelperInstances = "helper_instances";
        public const string Status = "status";
        public const string TimeLocal = "time_local";
        public const string PushID = "push_id";
        public const string NewStickers = "new_stickers";
    }

    [System.Serializable]
    public struct UserProperties
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