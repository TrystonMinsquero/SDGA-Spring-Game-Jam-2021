using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public GameObject Hearts;

    public Sprite heart;
    public Sprite brokenHeart;
    public Text enemies;
    public Text round;

    private static Sprite HEART;
    private static Sprite BROKENHEART;
    private static Text ENEMIESTEXT;
    private static Text ROUNDTEXT;

    private static Image[] hearts;
    private static float lingerTime;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

    }

    public void Start()
    {
        hearts = Hearts.GetComponentsInChildren<Image>();
        HEART = heart;
        BROKENHEART = brokenHeart;
        ENEMIESTEXT = enemies;
        ROUNDTEXT = round;
        ROUNDTEXT.gameObject.SetActive(false);
    }

    public static void updateEnemies(int numEnemies)
    {
        ENEMIESTEXT.text = "Enemies: " + numEnemies;
    }

    public static void displayRound(int round, float linger = 3)
    {
        ROUNDTEXT.text = "Round " + round;
        ROUNDTEXT.gameObject.SetActive(true);
        lingerTime = Time.time + linger;
        

    }

    private void Update()
    {
        if (Time.time > lingerTime)
            ROUNDTEXT.gameObject.SetActive(false);
    }

    private static void hideRound()
    {
        ROUNDTEXT.gameObject.SetActive(false);
    }

    public static void updateHearts(int health)
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = HEART;
            else
                hearts[i].sprite = BROKENHEART;
        }
    }
}
