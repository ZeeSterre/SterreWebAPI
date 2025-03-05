using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject panel;

    public void OpenMenu()
    {
        panel.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        panel.gameObject.SetActive(false);
    }

    public void ClickedOnLogout()
    {
        SterreWebAPI.Instance.Post("/account/logout", "", OnLogoutResponse);
    }

    public void ClickedOnBack()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    void OnLogoutResponse(APIResponse response)
    {
        if (response.Success)
        {
            Debug.Log("Logout succesfull");
            SceneManager.LoadScene("LoginRegister");
        }
    }
}
