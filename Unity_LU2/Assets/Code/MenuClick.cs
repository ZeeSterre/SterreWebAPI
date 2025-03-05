using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecorationMenu : MonoBehaviour
{
    public RectTransform menuPanel;
    public Button toggleButton;
    public bool toggled = false;

    void Start()
    {
        menuPanel.gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        if(toggled == false)
        {
            menuPanel.gameObject.SetActive(true);
            toggled = true;
        }
        else
        {
            menuPanel.gameObject.SetActive(false);
            toggled = false;
        }
    }

    public void OnButtonClicked()
    {
        ToggleMenu();
    }
}
