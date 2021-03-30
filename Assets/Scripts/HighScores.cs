using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighScores : MonoBehaviour
{
    public static HighScores instance;

    private static string[] privateCode = {
        "aA3-WA7yKUqiewP48VBaeA5GORZ0h0h0WSb8dh8BkU3A",
        "S4L0jrfwvEKO7Lw1H8-8bQGX5SWkv_m0WtONtMMUfgSg",
        "iVtwqVB4XUiDAqusP3uyTg03yr5yQBykmNcNeBf4yUMw"
    };
    private static string[] publicCode = {
        "5f978c2beb371809c4b44a91",
        "6062b91a8f421366b0565c98",
        "6062b9818f421366b0565ce9"
    };
    private static string webURL = "http://dreamlo.com/lb/";

    private static List<Score>[] highScores;

    public Button HighScoresButton;
    public GameObject usernamesParent;
    public GameObject scoresParent;

    public Button easyButton;
    public Button hardButton;
    public Button hardcoreButton;
    public Button PageUpButton;
    public Button PageDownButton;

    public static Button pageUp;
    public static Button pageDown;
    public static Text[] usernames;
    public static Text[] scores;



    public static Button highScoresButton;

    private static bool[] downloaded = { false, false, false };
    private static int[] pageIndex = { 0, 0, 0 };
    private static int[] maxPageIndex = { 0, 0, 0 };
    private static int maxScoresPerPage = 9;

    public void Awake()
    {
        highScoresButton = HighScoresButton;

        ToggleHSButton(false);

        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;


        //Instaitate and add score
        highScores = new List<Score>[3];
        
        highScores[0] = new List<Score>();
        highScores[1] = new List<Score>();
        highScores[2] = new List<Score>();

        pageUp = PageUpButton;
        pageDown = PageDownButton;

        DownloadHighScores();

        usernames = usernamesParent.GetComponentsInChildren<Text>();
        scores = scoresParent.GetComponentsInChildren<Text>();
        //prevPage.interactable = false;
        

    }

    public static void displayHighScores(Difficulty diff)
    {
        pageDown.interactable = pageIndex[((int)diff) - 1] > 0;
        pageUp.interactable = pageIndex[((int)diff) - 1] < maxPageIndex[((int)diff) - 1];

        //Clear text boxes
        for (int j = 0; j < maxScoresPerPage; j++)
        {
            usernames[j].text = "";
            scores[j].text = "";
        }


        //Print to Screen
        for (int i = 0; i < maxScoresPerPage; ++i)
        {
            int place = i + (maxScoresPerPage * pageIndex[((int)diff) - 1]);
            if (place < highScores[((int)diff) - 1].Count)
            {
                usernames[i].text = place + 1 + ". " + highScores[((int)diff) - 1].ElementAt<Score>(place).username;
                scores[i].text = "" + highScores[((int)diff) - 1].ElementAt<Score>(place).round;
            }
        }

    }

    public void AddNewHighScore(Score newScore)
    {
        ToggleHSButton(false);
        StartCoroutine(UploadNewHighscore(newScore));
    }
    public void DownloadHighScores()
    {
        ToggleHSButton(false);
        Debug.Log("Intiate Download");
        StartCoroutine(DownloadHighScoresCoroutine(Difficulty.EASY));
        StartCoroutine(DownloadHighScoresCoroutine(Difficulty.HARD));
        StartCoroutine(DownloadHighScoresCoroutine(Difficulty.HARDCORE));
    }

    IEnumerator UploadNewHighscore(Score newScore)
    {
        UnityWebRequest www = UnityWebRequest.Get(webURL + privateCode[((int)newScore.difficulty)-1] + "/add/" + UnityWebRequest.EscapeURL(newScore.username) + "/" + newScore.round);
        Debug.Log(UnityWebRequest.EscapeURL(newScore.username) + "/" + ScoreToInt(newScore));
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Succesful");
            DownloadHighScores();
        }
        else
            print("Error Uploading: " + www.error);
        
    }

    IEnumerator DownloadHighScoresCoroutine(Difficulty diff)
    {
        Debug.Log("Downloading");
        UnityWebRequest www = UnityWebRequest.Get(webURL + publicCode[((int)diff) - 1] + "/pipe");
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Downloaded");
            downloaded[((int)diff) - 1] = true;
            ToggleHSButton(downloaded[0] && downloaded[1] && downloaded[2]);
            FormatHighScores(www.downloadHandler.text, diff);
            maxPageIndex[((int)diff) - 1] = (highScores[((int)diff)-1].Count - 1) / maxScoresPerPage;
        }
        else
            print("Error Downloading: " + www.error);
        
    }

    void FormatHighScores(string text, Difficulty diff)
    {
        highScores[((int)diff) - 1].Clear();
        string[] entries = text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < entries.Length; ++i)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            highScores[((int)diff) - 1].Add(new Score(entryInfo[0], diff, int.Parse(entryInfo[1])));
            Debug.Log(highScores[((int)diff) - 1].ElementAt<Score>(i).username + ": " + highScores[((int)diff) - 1].ElementAt<Score>(i).difficulty + " - " + highScores[((int)diff) - 1].ElementAt<Score>(i).round);
        }
    }

    public static void ToggleHSButton(bool loaded)
    {
        highScoresButton.GetComponentInChildren<Text>().text = loaded ? "High Scores" : "Loading...";
        highScoresButton.enabled = loaded;
    }

    public string ScoreToInt(Score score)
    {
        return "" + (int)score.difficulty + score.round;
    }

    public Score formatToScore(string name, string score)
    {
        return new Score(name, (Difficulty)int.Parse(score.Substring(0,1)), int.Parse(score.Substring(1)));
    }

    public void SelectEasy()
    {
        easyButton.interactable = false;
        hardButton.interactable = true;
        hardcoreButton.interactable = true;
        displayHighScores(Difficulty.EASY);
    }

    public void SelectHard()
    {
        easyButton.interactable = true;
        hardButton.interactable = false;
        hardcoreButton.interactable = true;
        displayHighScores(Difficulty.HARD);
    }

    public void SelectHardcore()
    {
        easyButton.interactable = true;
        hardButton.interactable = true;
        hardcoreButton.interactable = false;
        displayHighScores(Difficulty.HARDCORE);
    }

    public void PageDown()
    {
        if (easyButton.interactable == false && pageIndex[0] > 0)
            pageIndex[0] = pageIndex[0] - 1;
        else if (hardButton.interactable == false && pageIndex[1] > 0)
            pageIndex[1] = pageIndex[1] - 1;
        else if (hardcoreButton.interactable == false && pageIndex[2] > 0)
            pageIndex[2] = pageIndex[2] - 1;
        else
            pageDown.interactable = false;
    }

    public void PageUp()
    {
        if (easyButton.interactable == false && pageIndex[0] < maxPageIndex[0])
            pageIndex[0] = pageIndex[0] + 1;
        else if (hardButton.interactable == false && pageIndex[1] < maxPageIndex[1])
            pageIndex[1] = pageIndex[1] + 1;
        else if (hardcoreButton.interactable == false && pageIndex[2] < maxPageIndex[2])
            pageIndex[2] = pageIndex[2] + 1;
        else
            pageUp.interactable = false;
    }

}
public struct Score
{
    public string username;
    public Difficulty difficulty;
    public int round;

    public Score(string user, Difficulty diff, int ro)
    {
        username = user;
        difficulty = diff;
        round = ro;
    }
}