using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentCreator : MonoBehaviour
{
    public GameObject panel;
    public int type;
    public TMP_InputField nameField;
    public TextMeshProUGUI resultText;

    public void ClickedOnNewWorld(int typeEnvironment)
    {
        type = typeEnvironment;
        panel.gameObject.SetActive(true);
    }

    public void ClickedOnCreate()
    {
        string environmentName = nameField.text;
        
        if(environmentName == null)
        {
            resultText.text = "Please fill in a name";
            return;
        }
        if (environmentName.Length < 2 || environmentName.Length > 25)
        {
            resultText.text = "Name must be between 2 and 25 characters";
            return;
        }

        CreateEnvironment createEnvironment = new CreateEnvironment
        {
            name = environmentName,
            environmentType = type
        };

        string json = JsonUtility.ToJson(createEnvironment);
        SterreWebAPI.Instance.Post("/Userinfo", json, Received);
    }

    public void Received(APIResponse response)
    {
        if(response.Success)
        {
            SceneManager.LoadScene("HomeScreen");
        }
        else
        {
            resultText.text = "Invalid name.";
        }
    }

}
