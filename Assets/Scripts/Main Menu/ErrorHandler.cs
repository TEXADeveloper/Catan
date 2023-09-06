using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button button;

    void Start()
    {
        ConnectionManager.RoomError += displayError;
        MenuController.WrongName += displayError;
    }

    private void displayError(int code, string message, RoomActions roomActions)
    {
        panel.SetActive(true);
        codeText.text = "Error Code: " + code;
        messageText.text = "Could not " + ((roomActions == RoomActions.Create) ? "create" : "join") + " room";
        messageText.text += "\nReason: " + message;
        button.Select();
    }


    void OnDestroy()
    {
        ConnectionManager.RoomError -= displayError;
        MenuController.WrongName -= displayError;
    }
}