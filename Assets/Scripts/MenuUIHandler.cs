using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] GameObject highScoreScreen;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] TextMeshProUGUI highScoresColumn1;
    [SerializeField] TextMeshProUGUI highScoresColumn2;

    void Start()
    {
        nameDisplay.text = "Name: " + GlobalManager.Instance.playerName;
        SetHighScoreText();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UpdateName()
    {
        string newName = nameInput.text;
        if (newName.Length > 7) { newName = newName[..7]; }
        if (newName != "")
        {
            GlobalManager.Instance.playerName = newName;
            nameDisplay.text = "Name: " + newName;
        }
    }

    public void ShowHighScores() => highScoreScreen.SetActive(true);

    public void ExitHighScores() => highScoreScreen.SetActive(false);

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        GlobalManager.Instance.SaveHighScores();
    }

    void SetHighScoreText()
    {
        List<string> hsStrs = new();
        foreach (GlobalManager.HighScore hs in GlobalManager.Instance.HighScores)
        {
            string line = (hs.name + " ").PadRight(13, '-') + " " + hs.score.ToString().PadLeft(3, '0');
            hsStrs.Add(line);
        }
        string[] hsCol1 = new string[5];
        string[] hsCol2 = new string[5];
        hsStrs.CopyTo(0, hsCol1, 0, 5);
        hsStrs.CopyTo(5, hsCol2, 0, 5);
        highScoresColumn1.text = string.Join("\n", hsCol1);
        highScoresColumn2.text = string.Join("\n", hsCol2);
    }
}
