using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        hearts = Hearts.GetComponentsInChildren<Image>();
    }

    public void Start()
    {
        HEART = heart;
        BROKENHEART = brokenHeart;
        ENEMIESTEXT = enemies;
        ROUNDTEXT = round;
        ROUNDTEXT.gameObject.SetActive(false);
        HUD.updateHearts(3);
    }

    public static void updateEnemies(int numEnemies)
    {
        ENEMIESTEXT.text = "Enemies: " + numEnemies;
    }

    public static void displayRound(int round, float linger = 3)
    {
        ROUNDTEXT.color = ROUNDTEXT.color + Color.black;
        ROUNDTEXT.text = "Round " + round;
        ROUNDTEXT.gameObject.SetActive(true);
        lingerTime = Time.time + linger;
        ROUNDTEXT.DOFade(0, 3f);

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
