using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;

public class Validator
{
    private const string ReceiptUrlValidator = "https://game-validator.dkotrack.com/api/receipt";

    private bool m_isValid = false;

    public bool isValid => m_isValid;

    public struct IOSReceiptObjectIAP
    {
        public string Payload;
        public string Store;
        public string TransactionID;

    }

    public IEnumerator ValidateProductReceipt(Product product)
    {
        var jsonData = GetUserDataJsonByProduct(product);
        Debug.Log($"Validator Info:\n{ReceiptUrlValidator}\n\n{jsonData}");

        var request = UnityWebRequest.Post(ReceiptUrlValidator, jsonData);
        request.SetRequestHeader("Content-Type", "Application/json");
        yield return request.SendWebRequest();

        if (IsValid(request))
        {
            //TODO: AnalyticsHelper
        }
    }

    public struct UserData
    {
        public string bundle_id;
        public string user_id;
        public string appsflyer_id;
        public string receipt;
    }

    private string GetUserDataJsonByProduct(in Product product)
    {
        var receipt = JsonUtility.FromJson<IOSReceiptObjectIAP>(product.receipt);
        var userData = new UserData()
        {
            bundle_id = product.definition.id,
            appsflyer_id = "",
            receipt = receipt.Payload,
            user_id = "",
        };

        return JsonUtility.ToJson(userData);
    }

    public struct Response
    {
        public int status;
        public string error;
        public string data;
    }

    private bool IsValid(in UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            var response =
                (Response)JsonUtility.FromJson(request.downloadHandler.text, typeof(Response));

            Debug.Log($"Result:\ndata {response.data},\nerror {response.error},\nstatus {response.status}.");
            return response.status == 0;
        }
        else
        {
            Debug.Log("request.result != UnityWebRequest.Result.Success");
        }

        return false;
    }

    /// <summary>
    /// Ping Validator
    /// </summary>
    /// <returns>Returns True if pinged successfully, else false</returns>

    public static async Task<bool> PingValidator()
    {
        UnityWebRequest request = new UnityWebRequest(ReceiptUrlValidator);
        request.SendWebRequest();
        while (!request.isDone)
        {
            await Task.Yield();
        }

        var res = request.result;
        if (res == UnityWebRequest.Result.ProtocolError || res == UnityWebRequest.Result.DataProcessingError)
        {
            return await Task.FromResult(true);
        }
        else
        {
            return await Task.FromResult(false);
        }
    }
}
