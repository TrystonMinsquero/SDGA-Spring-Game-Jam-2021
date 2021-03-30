using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartMenu : MonoBehaviour
{
    public Difficulty difficulty = Difficulty.EASY;

    public LevelLoader levelLoader;

    private GameObject startPanel;
    private Button startButton;
    private Button difficultyButton;
    private Button controlsButton;

    private GameObject difficultyPanel;
    private Button easyButton;
    private Button mediumButton;
    private Button hardButton;

    private RectTransform blackScreen;
    // Start is called before the first frame update
    void Start()
    {
        startPanel = GameObject.Find("StartPanel");
        difficultyPanel = GameObject.Find("DifficultySelection");
        startButton = GameObject.Find("Start").GetComponent<Button>();
        difficultyButton = GameObject.Find("Difficulty").GetComponent<Button>();
        controlsButton = GameObject.Find("Controls").GetComponent<Button>();
        easyButton = GameObject.Find("Easy").GetComponent<Button>();
        mediumButton = GameObject.Find("Medium").GetComponent<Button>();
        hardButton = GameObject.Find("Hard").GetComponent<Button>();

        startButton.onClick.AddListener(onStart);
        difficultyButton.onClick.AddListener(onDifficultyButtonClick);
        controlsButton.onClick.AddListener(onControlsButtonClick);
        easyButton.onClick.AddListener(easySelected);
        mediumButton.onClick.AddListener(mediumSelected);
        hardButton.onClick.AddListener(hardSelected);
        
        difficultyPanel.SetActive(false);
    }

    private void onStart()
    {
        levelLoader.LoadNextLevel();
    }

    private void onDifficultyButtonClick()
    {
        // Switch panels to difficulty panel
        startPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    private void onControlsButtonClick()
    {
        
    }

    private void easySelected()
    {
        DataHandler.startingDifficulty = Difficulty.EASY;
        difficultyPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    private void mediumSelected()
    {
        DataHandler.startingDifficulty = Difficulty.HARD;
        difficultyPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    private void hardSelected()
    {
        DataHandler.startingDifficulty = Difficulty.HARDCORE;
        difficultyPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}
