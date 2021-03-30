using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighScores : MonoBehaviour
{
    const string privateCode = "aA3-WA7yKUqiewP48VBaeA5GORZ0h0h0WSb8dh8BkU3A";
    const string publicCode = "5f978c2beb371809c4b44a91";
    const string webURL = "http://dreamlo.com/lb/";

    private List<Score> highScores;
    public bool highscoresLoaded;

    public int pageIndex = 0;
    public int maxPageIndex;
    public int maxScoresPerPage = 10;


    public void Awake()
    {

        //Instaitate and add score
        highScores = new List<Score>();

        if(DataHandler.playerName != null)
        {
            AddNewHighScore(new Score(DataHandler.playerName, DataHandler.startingDifficulty, DataHandler.round));
        }


        //prevPage.interactable = false;
        

    }

    public void displayHighScores()
    {
        //Switch Canvases
        //victoryCanvas.SetActive(false);
        //highScoreCanvas.SetActive(true);


        //Clear text boxes
        for(int j = 0; j < maxScoresPerPage; j++)
        {
            //userTextBoxes[j].text = "";
            //timeTextBoxes[j].text = "";
        }


        //Print to Screen
        for (int i = 0; i < maxScoresPerPage; ++i)
        {
            if(i + (10 * pageIndex) < highScores.Count)
            {
            }
        }

    }

    public void AddNewHighScore(Score newScore)
    {
        StartCoroutine(UploadNewHighscore(newScore));
    }
    public void DownloadHighScores()
    {
        Debug.Log("Intiate Download");
        StartCoroutine(DownloadHighScoresCoroutine());
    }

    IEnumerator UploadNewHighscore(Score newScore)
    {
        UnityWebRequest www = UnityWebRequest.Get(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(newScore.username) + "/" + ScoreToInt(newScore));
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Succesful");
            DownloadHighScores();
        }
        else
            print("Error Uploading: " + www.error);
        
    }

    IEnumerator DownloadHighScoresCoroutine()
    {
        Debug.Log("Downloading");
        UnityWebRequest www = UnityWebRequest.Get(webURL + publicCode + "/pipe");
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Downloaded");
            Debug.Log(www.downloadHandler.text);
            highscoresLoaded = true;
            FormatHighScores(www.downloadHandler.text);
            maxPageIndex = (highScores.Count - 1) / maxScoresPerPage;
        }
        else
            print("Error Downloading: " + www.error);
        
    }

    void FormatHighScores(string text)
    {
        highScores.Clear();
        string[] entries = text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < entries.Length; ++i)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            highScores.Add(formatToScore(entryInfo[0],entryInfo[1]));
            Debug.Log(highScores.ElementAt<Score>(i).username + ": " + highScores.ElementAt<Score>(i).round);
        }
    }

    public string ScoreToInt(Score score)
    {
        return "" + (int)DataHandler.startingDifficulty + DataHandler.round;
    }

    public Score formatToScore(string name, string score)
    {
        return new Score(name, (Difficulty)int.Parse(score.Substring(0,1)), int.Parse(score.Substring(1)));
    }

}
public struct Score
{
    public string username;
    Difficulty difficulty;
    public int round;

    public Score(string user, Difficulty diff, int ro)
    {
        username = user;
        difficulty = diff;
        round = ro;
    }
}