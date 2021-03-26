using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public float[] weaknesses; // damage multiplier based on attack type
    public BoxCollider2D hitbox;
    public int type;
    public float currentHP;
    public float maximumHP;
    public float forceVal = 3000f;
    private float lastAttack;
    public float attackCooldown;
    public Rigidbody2D target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player Temp" && (Time.time - lastAttack) > attackCooldown)
        {
            switch (type)
            {
                case 1:
                    lastAttack = Time.time;
                    col.gameObject.GetComponent<Player>().takeDamage();
                    Vector3 moveDir = transform.position - col.gameObject.transform.position;
                    rb.AddForce(moveDir * forceVal);
                    break;
                case 2:
                    col.gameObject.GetComponent<Player>().takeDamage();
                    break;
            } 
        }
    }

    void FixedUpdate()
    {
        if (currentHP <= 0)
        {
            Destroy(this.gameObject);
        }
        switch (type)
        {
            case 2:
                if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.gameObject.transform.position).magnitude < 5) 
                {
                    lastAttack = Time.time;
                    Debug.Log("attack");
                    Vector3 moveDir = transform.position - target.gameObject.transform.position;
                    rb.AddForce(moveDir * -1 * forceVal);
                }
                break;

        }
    }

    public void takeDamage(Weapon_Type weapon, int damage)
    {
        currentHP -= damage * weaknesses[(int)weapon];
        Vector3 moveDir = transform.position - target.gameObject.transform.position;
        rb.AddForce(moveDir * forceVal * 3);
    }
}
