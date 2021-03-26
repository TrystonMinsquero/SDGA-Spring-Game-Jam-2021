using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool attackDebounce;
    public BoxCollider2D weaponCollider;
    public float[] weaknesses = { 0, 0, 0 }; // damage multiplier based on attack type
    public Rigidbody2D target;
    public int type; // 1 = melee 2 = ranged
    public float currentHP;
    public float maximumHP;
    public float moveVal = 0.1f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        attackDebounce = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player Temp" && !attackDebounce) {
            
        }
    }

    IEnumerator Sleep(float time) {
        yield return new WaitForSeconds(time);
    }

    public void takeDamage(Weapon_Type weapon, int damage)
    {
        switch(weapon)
        {
            case Weapon_Type.SWORD:
                currentHP -= damage * weaknesses[0];
                break;
            case Weapon_Type.BLUNT:
                currentHP -= damage * weaknesses[1];
                break;
            case Weapon_Type.DISCUS:
                currentHP -= damage * weaknesses[2];
                break;
        }
    }
}
