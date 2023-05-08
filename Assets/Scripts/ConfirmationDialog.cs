using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmationDialog : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(string title, string message, UnityAction onYes, UnityAction onNo)
    {
        titleText.text = title;
        messageText.text = message;
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(onYes);
        noButton.onClick.AddListener(onNo);
        yesButton.onClick.AddListener(Close);
        noButton.onClick.AddListener(Close);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
