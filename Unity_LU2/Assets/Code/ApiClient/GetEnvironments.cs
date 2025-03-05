using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GetEnvironments : MonoBehaviour
{
    public GameObject[] environments;
    public TextMeshProUGUI[] environmentTexts;
    public Button buttonRemove;
    public EnvironmentDataList environmentDataList;

    void Start()
    {
        SterreWebAPI.Instance.Get("/Userinfo", EnvironmentReceived);
    }

    public void EnvironmentReceived(APIResponse response)
    {
        if (!response.Success)
        {
            Debug.Log("Environment not able to load.");
            return;
        }
        string wrappedJson = $"{{\"environments\": {response.Data} }}";
        environmentDataList = JsonUtility.FromJson<EnvironmentDataList>(wrappedJson);

        int environmentCount = Mathf.Min(environmentDataList.environments.Count, 5);

        for (int e = 0; e < 5; e++)
        {
            if (e < environmentCount)
            {
                environments[e].SetActive(true);

                environmentTexts[e].text = environmentDataList.environments[e].name;
            }
            else
            {
                environments[e].SetActive(false);
            }
        }

        if (environmentCount >= 5)
        {
            buttonRemove.gameObject.SetActive(false);
        }
    }

    public void OnEnvironmentClicked(int index)
    {
        if (index < environmentDataList.environments.Count)
        {
           
            LoadScene(environmentDataList.environments[index].id, environmentDataList.environments[index].environmentType);

        }
    }

    public void LoadScene(string environmentID, int type)
    {
        PlayerPrefs.SetString("environmentID", environmentID);
        if(type == 0)
        {
            SceneManager.LoadScene("Underwater");
        }
        else if(type == 1)
        {
            SceneManager.LoadScene("FairytaleForest");
        }
        else if (type == 2)
        {
            SceneManager.LoadScene("CastleRoom");
        }
        else
        {
            SceneManager.LoadScene("HomeScreen");
        }

    }

    public void ClickedOnEnvironmentDelete(int index)
    {
        if (index < environmentDataList.environments.Count)
        {

            DeleteEnvironment(environmentDataList.environments[index].id);

        }
    }

    public void DeleteEnvironment(string environmentID)
    {
        if(environmentID != null)
        {
            string path = "/UserInfo/" + environmentID;
            SterreWebAPI.Instance.Delete(path, Deleting);
        }
    }

    public void Deleting(APIResponse response)
    {
        if(response.Success)
        {
            SceneManager.LoadScene("HomeScreen");
        }
    }

    
}
