using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public TMP_InputField userName, password;
    public GameObject hideScreen, inputScreen;
    private bool login = true;
    public TextMeshProUGUI resultText;

    private void Start()
    {
        ShowLogin();
    }

    public void SubmitClicked()
    {
        string jsonData = $"{{\"userName\":\"{userName.text}\",\"password\":\"{password.text}\"}}";
        string path = login ? "/account/login" : "/account/register";
        SterreWebAPI.Instance.Post(path, jsonData, OnResponse);
    }

    private void OnResponse(APIResponse response)
    {
        if (response.Success)
        {
            resultText.text = login ? "Logged in." : "Signed up successfully.";

            if (login)
            {
                SceneManager.LoadScene("HomeScreen");
            }
        }
        else
        {
            resultText.text = "Invalid username or password.";
        }
    }

    public void HideLogin(string choice)
    {
        if(choice == "Login")
        {
            login = true;
        }
        else
        {
            login = false;
        }

        hideScreen.gameObject.SetActive(false);
        inputScreen.gameObject.SetActive(true);

    }

    public void ShowLogin()
    {
        resultText.text = "";
        userName.text = "";
        password.text = "";
        hideScreen.gameObject.SetActive(true);
        inputScreen.gameObject.SetActive(false);
    }
}