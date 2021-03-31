using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndMenu : MonoBehaviour
{
    public GameObject HighScoreCanvas;
    public GameObject round;
    public GameObject difficulty;

    public InputField nameInput;
    public GameObject destroyAfterSubmit;
    public Button highScoreButton;


    public Text bgText1;
    public Text bgText2;
    public Text bgText3;
    public Text bgText4;
    public Text bgText5;
    private int xMin = 316;
    private int xMax = 365;
    private int yMin = 200;
    private int yMax = 225;


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
                    text.text = "HELL";
                break;
        }

        //Background code
        int randX = Random.Range(xMin, xMax);
        int randY = Random.Range(yMin, yMax);
        Vector3 newCamPos = new Vector3(randX, randY, 0);
        Camera.main.transform.position = newCamPos;
        StartCoroutine(titleColorRotation());
        StartCoroutine(titleBounce());


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
        DataHandler.reset();
        SceneManager.LoadScene("StartMenu");
    }


    IEnumerator titleColorRotation()
    {
        while (true)
        {
            bgText1.DOColor(new Color(191f / 255f, 89f / 255f, 7f / 255f), 3);
            bgText2.DOColor(new Color(191f / 255f, 89f / 255f, 7f / 255f), 3);
            bgText3.DOColor(new Color(191f / 255f, 89f / 255f, 7f / 255f), 3);
            bgText4.DOColor(new Color(191f / 255f, 89f / 255f, 7f / 255f), 3);
            bgText5.DOColor(new Color(191f / 255f, 89f / 255f, 7f / 255f), 3);
            yield return new WaitForSeconds(3);
            bgText1.DOColor(new Color(3f / 255f, 92f / 255f, 191f / 255f), 3);
            bgText2.DOColor(new Color(3f / 255f, 92f / 255f, 191f / 255f), 3);
            bgText3.DOColor(new Color(3f / 255f, 92f / 255f, 191f / 255f), 3);
            bgText4.DOColor(new Color(3f / 255f, 92f / 255f, 191f / 255f), 3);
            bgText5.DOColor(new Color(3f / 255f, 92f / 255f, 191f / 255f), 3);
            yield return new WaitForSeconds(3);
            bgText1.DOColor(new Color(183f / 255f, 12f / 255f, 191f / 255f), 3);
            bgText2.DOColor(new Color(183f / 255f, 12f / 255f, 191f / 255f), 3);
            bgText3.DOColor(new Color(183f / 255f, 12f / 255f, 191f / 255f), 3);
            bgText4.DOColor(new Color(183f / 255f, 12f / 255f, 191f / 255f), 3);
            bgText5.DOColor(new Color(183f / 255f, 12f / 255f, 191f / 255f), 3);
        }
    }

    IEnumerator titleBounce()
    {
        while (true)
        {
            int randX = Random.Range(xMin, xMax);
            int randY = Random.Range(yMin, yMax);
            Debug.Log(randX);
            Debug.Log(randY);
            Vector3 newCamPos = new Vector3(randX, randY, 0);
            if ((Camera.main.transform.position - newCamPos).magnitude < 5)
            {
                continue;
            }
            Camera.main.transform.DOMove(newCamPos, (Camera.main.transform.position - newCamPos).magnitude / 2.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds((Camera.main.transform.position - newCamPos).magnitude / 2.5f);
        }

    }

}
