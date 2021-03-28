using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject type3Projectile;
    private Rigidbody2D rb;
    public float[] weaknesses; // damage multiplier based on attack type
    public BoxCollider2D hitbox;
    public int type;
    public float currentHP;
    public float maximumHP;
    private float lastAttack;
    public float updateRate = 0.1f;
    public float attackCooldown;
    public Rigidbody2D target;
    public HealthBar healthbar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("AttackCheck",1.0f, updateRate);
        healthbar.gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            switch (type)
            {
                case 1:
                    if ((Time.time - lastAttack) > attackCooldown) {
                        lastAttack = Time.time;
                        col.gameObject.GetComponent<Player>().takeDamage();
                        Vector3 targetPos = transform.position + (col.gameObject.transform.position - transform.position).normalized * 2f;
                        transform.DOMove(targetPos, 2f);
                    }
                    break;
                case 2:
                    col.gameObject.GetComponent<Player>().takeDamage();
                    break;
            } 
        }
        else if (col.gameObject.tag == "Wall") {
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

        if (healthbar != null && currentHP != maximumHP && currentHP > 0)
        {
            healthbar.gameObject.SetActive(true);
            healthbar.SetHealth(currentHP / maximumHP);
        }
        
    }

    private void AttackCheck() 
    {
        switch (type)
        {
            case 2:
                if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.gameObject.transform.position).magnitude < 5) 
                {
                    lastAttack = Time.time;
                    Vector3 targetPos = transform.position + (target.gameObject.transform.position - transform.position).normalized * 15f;
                    transform.DOMove(targetPos, 3);
                }
                break;
            case 3:
                Quaternion rot = Quaternion.LookRotation(target.gameObject.transform.position - transform.position, transform.TransformDirection(Vector3.up));
                transform.rotation = new Quaternion(0,0, rot.z, rot.w);
                if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.gameObject.transform.position).magnitude < 15 && (transform.position - target.gameObject.transform.position).magnitude > 3 ) {
                    lastAttack = Time.time;
                    GameObject projectileClone = Instantiate(type3Projectile, transform.position, Quaternion.identity) as GameObject;
                    Vector3 targetPos = transform.position + (target.gameObject.transform.position - transform.position).normalized * 40f;
                    projectileClone.GetComponent<EnemyProjectile>().move(transform.position, targetPos, 10);
                }
                break;
        }
    }

    public void takeDamage(Weapon_Type weapon, int damage)
    {
        DOTween.Kill(transform);
        currentHP -= damage * weaknesses[(int)weapon];
        if (type < 3) {
            Vector3 targetPos = transform.position + (target.gameObject.transform.position - transform.position).normalized * -5f;
            transform.DOMove(targetPos, 2f);
        }
    }
}
