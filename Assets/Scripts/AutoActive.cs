using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoActive : MonoBehaviour
{

    bool pressed = false;
    Button button;

    void Awake(){
        button = GetComponent<Button>();
    }
    
     public void onClick()
    {
        var textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        var image = button.GetComponent<Image>();

        var colors = button.colors;
        if (!pressed)
        {
            image.color = Color.black;
            textComponent.color = Color.white;
        }
        else
        {
            image.color = Color.white;
            textComponent.color = Color.black;
        }
        pressed = !pressed;
    }
}
