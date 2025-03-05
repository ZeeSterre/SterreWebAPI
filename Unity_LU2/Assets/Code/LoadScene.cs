using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateNewEnvironment : MonoBehaviour
{
    public void LoadNewEnvironment()
    {
        SceneManager.LoadScene("NewEnvironment");
    }

    public void LoadHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }
}