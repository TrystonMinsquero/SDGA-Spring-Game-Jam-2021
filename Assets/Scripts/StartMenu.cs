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

    private GameObject controlsPanel;
    private Text BGText;
    private Button controlExitButton;

    private RectTransform blackScreen;
    // Start is called before the first frame update
    void Start()
    {
        startPanel = GameObject.Find("StartPanel");
        difficultyPanel = GameObject.Find("DifficultySelection");
        controlsPanel = GameObject.Find("ControlsPanel");
        controlExitButton = GameObject.Find("ControlExitButton").GetComponent<Button>();
        startButton = GameObject.Find("Start").GetComponent<Button>();
        difficultyButton = GameObject.Find("Difficulty").GetComponent<Button>();
        controlsButton = GameObject.Find("Controls").GetComponent<Button>();
        easyButton = GameObject.Find("Easy").GetComponent<Button>();
        mediumButton = GameObject.Find("Medium").GetComponent<Button>();
        hardButton = GameObject.Find("Hard").GetComponent<Button>();
        BGText = GameObject.Find("BGtext").GetComponent<Text>();

        startButton.onClick.AddListener(onStart);
        difficultyButton.onClick.AddListener(onDifficultyButtonClick);
        controlsButton.onClick.AddListener(onControlsButtonClick);
        easyButton.onClick.AddListener(easySelected);
        mediumButton.onClick.AddListener(mediumSelected);
        hardButton.onClick.AddListener(hardSelected);
        controlExitButton.onClick.AddListener(onControlsExitButtonClick);
        
        
        difficultyPanel.SetActive(false);
        controlsPanel.SetActive(false);
        controlsPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(titleColorRotation());
    }

    IEnumerator titleColorRotation() {
        while (true) {
            BGText.DOColor(new Color(191f/255f,89f/255f,7f/255f),3);
            yield return new WaitForSeconds(3);
            BGText.DOColor(new Color(183f/255f,12f/255f,191f/255f),3);
            yield return new WaitForSeconds(3);
            BGText.DOColor(new Color(3f/255f,92f/255f,191f/255f),3);
        }
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
        controlsPanel.SetActive(true);
    }

    private void onControlsExitButtonClick()
    {

        controlsPanel.SetActive(false);
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
