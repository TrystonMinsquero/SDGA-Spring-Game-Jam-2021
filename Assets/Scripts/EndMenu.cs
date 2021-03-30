using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public GameObject HighScoreCanvas;
    public GameObject round;
    public GameObject difficulty;

    public InputField nameInput;
    public GameObject destroyAfterSubmit;
    public Button highScoreButton;


    public void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        HighScoreCanvas.GetComponent<Canvas>().enabled = false;

        Text[] roundTexts = round.GetComponentsInChildren<Text>();
        foreach(Text text in roundTexts)
            text.text = DataHandler.round.ToString();

        Text[] diffTexts = difficulty.GetComponentsInChildren<Text>();
        switch (DataHandler.startingDifficulty)
        {
            case Difficulty.EASY:
                foreach (Text text in diffTexts)
                    text.text = "EASY";
                break;
            case Difficulty.HARD:
                foreach (Text text in diffTexts)
                    text.text = "HARD";
                break;
            case Difficulty.HARDCORE:
                foreach (Text text in diffTexts)
                    text.text = "HARDCORE";
                break;
        }


    }

    public void Submit()
    {
        Debug.Log(nameInput.text);
        if(nameInput.text != null)
        {
            HighScores.instance.AddNewHighScore(new Score(nameInput.text.Replace(" ","_"), DataHandler.startingDifficulty, DataHandler.round));
            Destroy(destroyAfterSubmit);

        }
    }

    public void HighScore()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        switch (DataHandler.startingDifficulty)
        {
            case Difficulty.EASY: HighScores.instance.SelectEasy();break;
            case Difficulty.HARD: HighScores.instance.SelectHard();break;
            case Difficulty.HARDCORE: HighScores.instance.SelectHardcore();break;
        }
        HighScoreCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
