using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GameData
{
    public string device_id = "";
    public string state = "";
    public int time_remaining = 0;
    public int level_id = 0;
    public int enemies_killed = 0;
    public int bombs_placed = 0;
    public int boxes_destroyed = 0;
}

public class AnalyticsManager : MonoBehaviour
{
    public static IEnumerator PostMethod(string jsonData)
    {
        string url = "http://34.228.111.166:5000/upload_data";

        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (!request.isNetworkError && request.responseCode == (int)System.Net.HttpStatusCode.OK)
            {
                Debug.Log("Data successfully sent to the server");
            }
            else
            {
                Debug.Log("Error sending data to the server: Error " + request.responseCode);
            }
        }
    }
}
