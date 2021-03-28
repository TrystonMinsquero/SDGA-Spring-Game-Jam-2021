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

    public static Sprite HEART;
    public static Sprite BROKENHEART;

    private static Image[] hearts;

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
