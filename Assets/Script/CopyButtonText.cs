using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CopyButtonText : MonoBehaviour
{
    public Button targetButton; // Assign the button in Inspector

    private void Start()
    {
        targetButton.onClick.AddListener(CopyButtonLabel);
    }

    void CopyButtonLabel()
    {
        // Try to get TMP_Text from the button
        TMP_Text buttonText = targetButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            GUIUtility.systemCopyBuffer = buttonText.text;
            Debug.Log("Copied: " + buttonText.text);
        }
        else
        {
            Debug.LogWarning("Button text not found!");
        }
    }
}
