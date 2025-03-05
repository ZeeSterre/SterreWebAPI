using UnityEngine;
using UnityEngine.UI;

public class HideButtons : MonoBehaviour
{
    public GameObject hideScreen, inputScreen;

    public void HideLogin()
    {

        hideScreen.gameObject.SetActive(false);
        inputScreen.gameObject.SetActive(true);

    }

    public void ShowLogin()
    {
        hideScreen.gameObject.SetActive(true);
        inputScreen.gameObject.SetActive(false);
    }
}
