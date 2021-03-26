using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public float[] weaknesses; // damage multiplier based on attack type
    public BoxCollider2D hitbox;
    public int type; // 1 = melee 2 = ranged
    public float currentHP;
    public float maximumHP;
    public float forceVal = 2000f;
    private float lastAttack;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Player Temp" && (Time.time - lastAttack) > 2) {
            lastAttack = Time.time;
            col.gameObject.GetComponent<Player>().takeDamage();
            Vector3 moveDir = transform.position - col.gameObject.transform.position;
            rb.AddForce(moveDir * 2000f);
        }
    }

    public void takeDamage(Weapon_Type weapon, int damage)
    {
        currentHP -= damage * weaknesses[(int)weapon];
    }
}
