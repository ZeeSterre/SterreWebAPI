using UnityEngine;
using UnityEngine.UI;

public class ShowCardOnClick : MonoBehaviour
{
    public GameObject cardImage;  // Sleep hier de kaartafbeelding naartoe in de Inspector

    // Start is called before the first frame update
    void Start()
    {
        // Zorg ervoor dat de kaart eerst verborgen is
        cardImage.SetActive(false);
    }

    public void OnButtonClick()
    {
        // Toggle de zichtbaarheid van de kaart (als het zichtbaar is, verberg het; als het verborgen is, toon het)
        bool isActive = cardImage.activeSelf;
        cardImage.SetActive(!isActive);
    }
}
