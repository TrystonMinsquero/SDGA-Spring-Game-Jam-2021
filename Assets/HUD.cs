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
        ROUNDTEXT.color = ROUNDTEXT.color + Color.black;
        ROUNDTEXT.text = "Round " + round;
        ROUNDTEXT.gameObject.SetActive(true);
        lingerTime = Time.time + linger;
        ROUNDTEXT.DOFade(0, 3f);

    }

    static IEnumerator FadeText(Text text, float seconds = 1)
    {
        // loop over 1 second backwards
        for (float i = seconds; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }
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
