using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool attackDebounce;
    public Rigidbody2D rb;
    public float[] weaknesses = new float[] {0,0,0}; // damage multiplier based on attack type
    public BoxCollider2D hitbox;
    public int type; // 1 = melee 2 = ranged
    public float currentHP;
    public float maximumHP;
    public float forceVal = 2000f;
    private int attackCooldown = 2;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        attackDebounce = false;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.name == "Player Temp" && !attackDebounce) {
            attackDebounce = true;
            Vector3 moveDir = transform.position - col.gameObject.transform.position;
            rb.AddForce(moveDir * 2000f);
            Debug.Log("attack");
            StartCoroutine(Sleep());
            attackDebounce = false;
        }
    }

    IEnumerator Sleep() {
        yield return new WaitForSeconds(5);
    }

    public void takeDamage(Weapon_Type weapon, int damage)
    {
        currentHP -= damage * weaknesses[(int)weapon];
    }
}
