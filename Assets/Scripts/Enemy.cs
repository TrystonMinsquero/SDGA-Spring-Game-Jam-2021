using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if (col.gameObject.name == "Player Temp")
        {
            switch (type)
            {
                case 1:
                    if ((Time.time - lastAttack) > attackCooldown) {
                        lastAttack = Time.time;
                        col.gameObject.GetComponent<Player>().takeDamage();
                        Vector3 moveDir = transform.position - col.gameObject.transform.position;
                        rb.AddForce(moveDir * forceVal);
                    }
                    break;
                case 2:
                    Debug.Log("hit playa");
                    col.gameObject.GetComponent<Player>().takeDamage();
                    break;
            } 
        }
        else if (col.gameObject.layer == 8) {
            DOTween.Kill(transform);
        }
    }

    void FixedUpdate()
    {
        if (currentHP <= 0)
        {
            DOTween.Kill(transform);
            Destroy(this.gameObject);
        }
        AttackCheck();
    }

    void AttackCheck() 
    {
        switch (type)
        {
            case 2:
                if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.gameObject.transform.position).magnitude < 5) 
                {
                    lastAttack = Time.time;
                    Vector2 targetPos = transform.position + (target.gameObject.transform.position - transform.position).normalized * 15f;
                    transform.DOMove(targetPos, 3);
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
