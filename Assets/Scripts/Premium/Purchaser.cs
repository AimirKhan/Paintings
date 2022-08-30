using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class Purchaser : MonoBehaviour, IStoreListener
{
    private IStoreController m_StoreController;

    private const string SubscriptionProductIdMon = "com.oak.painting.subscription_1";
    private const string SubscriptionProductIdYear = "com.oak.painting.subscription_2";

    private bool m_subIsActive = false;

    private bool m_debugSub = false;

    private Purchaser m_instance = Purchaser.Instance;

    public static Purchaser Instance;

    private Validator validator;

    private static bool created = false;

    public static bool IsInitialized =>
        Instance != null &&
        Instance.m_StoreController != null;

#if UNITY_IOS
    IAppleExtensions m_AppleExtensions;
#endif

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }

    public void Initialize()
    {
        RunInitialize();
    }


    private async void RunInitialize()
    {
        StringBuilder outputDebug = new StringBuilder("___Purchaser Initialize Info___\n\n");
        outputDebug.AppendLine("[" + DateTime.Now + "] Start Initialize Purchaser");

        if (IsInitialized)
        {
            Debug.LogWarning("Purchaser already initialized");
            return;
        }
        Instance = this;
        m_instance = Instance;
        Instance.validator = new Validator();

        // Validator test

        var result = await Validator.PingValidator();
        outputDebug.AppendLine($"[{DateTime.Now}] Validator: Ping result: {result}");

        if (!result)
        {
            outputDebug.AppendLine($"[{DateTime.Now}] Ping Validator failed. Purchaser not initialized");
            Debug.LogError(outputDebug.ToString());
            return;
        }
        else
        {
            outputDebug.AppendLine($"[{DateTime.Now}] Ping Validator successfully");
        }

        // Final

        InitializePurchasing();

        outputDebug.AppendLine($"[{DateTime.Now}]\nInstance != null: {Instance != null}");
        outputDebug.AppendLine($"Instance.m_StoreController != null: {Instance.m_StoreController != null}");
        outputDebug.AppendLine($"IsInitialized: {IsInitialized}");
        outputDebug.AppendLine($"\n[{DateTime.Now}] Initialization completed");
        outputDebug.AppendLine($"\nProduct names:");
        outputDebug.AppendLine(SubscriptionProductIdMon);
        outputDebug.AppendLine(SubscriptionProductIdYear);
        Debug.Log(outputDebug.ToString());
    }

    private void InitializePurchasing()
    {
        if (m_debugSub)
        {
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(SubscriptionProductIdMon, ProductType.Subscription);
        builder.AddProduct(SubscriptionProductIdYear, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);

    }

    public void BuySubscriptionMon()
    {
        Instance.BuySubscription(SubscriptionProductIdMon);
    }

    public void BuySubscriptionYear()
    {
        Instance.BuySubscription(SubscriptionProductIdYear);
    }

    private void BuySubscription(string SubName)
    {

        if (!IsInitialized)
        {
            Debug.LogWarning("Purchaser not initialized");
            return;
        }

        if (m_subIsActive)
        {
            Debug.LogWarning("You have sub");
            return;
        }

        m_StoreController.InitiatePurchase(SubName);
        Product product = m_StoreController.products.WithID(SubName);

        if (product != null && product.availableToPurchase)
        {
            if (SubName == SubscriptionProductIdMon)
            {
                SendAnalytucsMon();
            }
            else
            {
                SendAnalytucsYear();
            }
        }
        else
        {
            Debug.LogError($"Product: {SubName} not available for purchase");
        }
    }

    private void SendAnalytucsMon()
    {
        AnalyticsHelper.Instance.SendIAPData(1, SubscriptionProductIdMon, -1, 3.99M,
            SceneController.GetActiveScene());
    }

    private void SendAnalytucsYear()
    {
        AnalyticsHelper.Instance.SendIAPData(1, SubscriptionProductIdYear, -1, 19.99M,
            SceneController.GetActiveScene());
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;


        bool subIsActiveMon = UpdateData(SubscriptionProductIdMon);
        bool subIsActiveYear = UpdateData(SubscriptionProductIdYear);

#if UNITY_IOS
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        Debug.Log($"m_AppleExtensions is {m_AppleExtensions != null}");
#endif

        m_subIsActive = subIsActiveMon || subIsActiveYear;
        Debug.Log("In-App Purchasing successfully initialized");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"In-App Purchasing initialize failed: {error}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason} [{(int)failureReason}]");
        
        //if (failureReason == PurchaseFailureReason)
        {
            Restore();
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;

        StartCoroutine(validator.ValidateProductReceipt(product));
        Debug.Log($"Purchase Complete - Product: {product.definition.id}");
        Instance.m_subIsActive = UpdateData(product.definition.id);
        SceneController.ReloadActiveScene();

        return PurchaseProcessingResult.Complete;
    }

    bool IsSubscribedTo(Product subscription)
    {
        if (subscription.receipt == null)
        {
            return false;
        }

        var subscriptionManager = new SubscriptionManager(subscription, null);
        var info = subscriptionManager.getSubscriptionInfo();

        return info.isSubscribed() == Result.True;
    }

    bool UpdateData(string productName)
    {
        var subscriptionProduct = m_StoreController.products.WithID(productName);

        try
        {
            var isSubscribed = IsSubscribedTo(subscriptionProduct);

            if (!m_subIsActive && isSubscribed)
            {
                SceneController.ReloadActiveScene();
            }

            return isSubscribed;
        }
        catch (StoreSubscriptionInfoNotSupportedException)
        {
            var receipt = (Dictionary<string, object>)MiniJson.JsonDecode(subscriptionProduct.receipt);
            var store = receipt["Store"];
            Debug.LogWarning("Couldn't retrieve subscription information because your current store is not supported.\n" +
                $"Your store: \"{store}\"\n\n" +
                "You must use the App Store, Google Play Store or Amazon Store to be able to retrieve subscription information.\n\n");
        }

        return false;
    }

    /// <summary>
    /// Restore Purchase. Initialize purchasing and reload scene.
    /// </summary>
    public void Restore()
    {
        Debug.Log("Started Restore");
#if UNITY_IOS
        Instance.m_AppleExtensions.RestoreTransactions(OnRestore);
#else
        Debug.LogWarning("I can't restore, switch platform.");
#endif
    }

    void OnRestore(bool success)
    {
        if (success)
        {
            var oldStatus = Instance.m_subIsActive;
            Debug.Log("Restore Successful");
            InitializePurchasing();

            Debug.Log($"Old status: {oldStatus}, new status: {Instance.m_subIsActive}");
            SceneController.ReloadActiveScene();
        }
        else
        {
            Debug.Log("Restore Failed");
        }
    }

    //
    // Get
    //

    public static bool isPremium()
    {
        if (!IsInitialized)
        {
            Debug.LogWarning("Purchaser not initialized");
            return false;
        }

        return Instance.m_subIsActive;
    }

    public static string GetSubMon()
    {
        return SubscriptionProductIdMon;
    }

    public static string GetSubYear()
    {
        return SubscriptionProductIdYear;
    }

    //
    // Debug
    //

    public static void DebugActivePremium(bool status)
    {
        Instance.m_debugSub = true;
        Instance.m_subIsActive = status;

        SceneController.ReloadActiveScene();
    }

    public static void Test()
    {
        Debug.Log($"Purcharser Test()" +
            $"Instance == null: {Instance == null}\n" +
            $"Instance.m_StoreController == null: {Instance.m_StoreController == null}");
    }
}
