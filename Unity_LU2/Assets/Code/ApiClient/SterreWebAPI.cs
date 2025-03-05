using System;
using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SterreWebAPI : MonoBehaviour
{

    private static SterreWebAPI _instance;

    private string baseurl = "https://avansict2226111.azurewebsites.net";

    public static SterreWebAPI Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("SterreWebAPI");
                _instance = obj.AddComponent<SterreWebAPI>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }

    }

    private void ResponseHandling(UnityWebRequest request, Action<APIResponse> callback)
    {
        int statusCode = (int)request.responseCode;

        if(request.result != UnityWebRequest.Result.Success)
        {
            APIResponse response = new APIResponse(false, "API has an error! Error: ", null, statusCode);
            callback?.Invoke(response);

            if(statusCode == 405)
            {
                UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();

                if(currentScene.buildIndex != 0)
                {
                    SceneManager.LoadScene(0);
                }

            }
        }
        else
        {
            APIResponse response = new APIResponse(true, "API request was successfull!", request.downloadHandler?.text, statusCode);
            callback?.Invoke(response);
        }
    }    

    public void Get(string path, Action<APIResponse> callback)
    {
        StartCoroutine(GetRequest(path, callback));
    }

    public void Post(string path, string jsonData, Action<APIResponse> callback)
    {
        StartCoroutine(PostRequest(path, jsonData, callback));
    }

    public void Delete(string path, Action<APIResponse> callback)
    {
        StartCoroutine(DeleteRequest(path, callback));
    }


    private IEnumerator GetRequest(string path, Action<APIResponse> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseurl + path))
        {
            yield return request.SendWebRequest();
            ResponseHandling(request, callback);
        }
    }


    private IEnumerator PostRequest(string path, string jsonData, Action<APIResponse> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(baseurl + path, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            ResponseHandling(request, callback);
        }
    }

    private IEnumerator DeleteRequest(string path, Action<APIResponse> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Delete(baseurl + path))
        {
            yield return request.SendWebRequest();
            ResponseHandling(request, callback);
        }
    }


}
