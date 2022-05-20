using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NewGameScript : MonoBehaviour
{
    public TMP_InputField NameInput;
    public Button PlayButton;

    private string Name = "";

    // Start is called before the first frame update
    void Awake()
    {
        NameInput.text = "";
        ValidateName();

        NameInput.ActivateInputField();

        NameInput.onValueChanged.AddListener((string value) =>
        {
            Name = value;
            ValidateName();
        });

        NameInput.onSubmit.AddListener((string value) =>
        {
            ValidateName();
            if (PlayButton.interactable)
            {
                PlayGame();
            }
        });

        PlayButton.onClick.AddListener(() =>
        {
            PlayGame();
        });
    }

    void PlayGame()
    {
        GlobalStateScript.Instance.PlayerName = Name;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    void ValidateName()
    {
        bool enabled = Name.Length > 2;
        PlayButton.interactable = enabled;
        var buttonTextTmp = PlayButton.transform.GetComponentInChildren<TMP_Text>();
        if (enabled)
        {
            buttonTextTmp.text = "PLAY";
        }
        else
        {
            string text = "<";
            for (int i = 2 - Name.Length; i > 0; i--)
            {
                text += "<";
            }
            buttonTextTmp.text = text;
        }
    }
}
