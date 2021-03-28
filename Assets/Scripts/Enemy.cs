using DG.Tweening;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyType type;
    public float maximumHP;
    public float updateRate = 0.1f;
    public float attackCooldown;
    public int difficulty;
    public float[] weaknesses; // damage multiplier based on attack type
    [Header("Draggables")]
    public GameObject rangedProjectile;
    public BoxCollider2D hitbox;
    public GameObject healthBarPrefab;
    public Transform healthBarSpot;
    public GameObject DeathParticle;
    
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private GameObject healthBar;
    private Vector3 healthBarPos;
    private float currentHP;
    private float lastAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player").transform;
        gameObject.GetComponent<AIDestinationSetter>().target = target;
        healthBarPos = healthBarSpot.position - transform.position;
        Destroy(healthBarSpot.gameObject);
        InvokeRepeating("AttackCheck",1.0f, updateRate);
        currentHP = maximumHP;
    }


    private void FixedUpdate()
    {
        if (healthBar != null)
            healthBar.transform.position = transform.position + healthBarPos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (type == EnemyType.CHARGE && col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().takeDamage();
        }
        else if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy") {
            DOTween.Kill(transform);
            if (type == EnemyType.CHARGE) {
                Vector3 targetPos = transform.position + (col.gameObject.transform.position - transform.position).normalized * -3f;
                transform.DOMove(targetPos, 2f);
            }
        }
    }

    private void AttackCheck() 
    {
        switch (type)
        {
            case EnemyType.MELEE:
                {
                    if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.position).magnitude < 0.8)
                    {
                        lastAttack = Time.time;
                        GameObject projectileClone = Instantiate(rangedProjectile, transform.position, Quaternion.identity) as GameObject;
                        Vector3 targetPos = transform.position + (target.position - transform.position).normalized * 1f;
                        projectileClone.GetComponent<EnemyProjectile>().move(transform, targetPos, 0.3f);
                    }
                    break;  
                }
            case EnemyType.CHARGE:
                {
                    if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.position).magnitude < 5)
                    {
                        lastAttack = Time.time;
                        Vector3 targetPos = transform.position + (target.position - transform.position).normalized * 15f;
                        transform.DOMove(targetPos, 3);
                    }
                    break;
                }
            case EnemyType.RANGED:
                {
                    if ((Time.time - lastAttack) > attackCooldown && (transform.position - target.position).magnitude < 15 && (transform.position - target.position).magnitude > 3)
                    {
                        lastAttack = Time.time;
                        GameObject projectileClone = Instantiate(rangedProjectile, transform.position, Quaternion.identity) as GameObject;
                        Vector3 targetPos = transform.position + (target.position - transform.position).normalized * 40f;
                        projectileClone.GetComponent<EnemyProjectile>().move(transform, targetPos, 10);
                    }
                    break;
                }
        }
    }

    public void takeDamage(Weapon_Type weapon, int damage)
    {
        DOTween.Kill(transform);
        currentHP -= damage * weaknesses[(int)weapon];

        //Check for death
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        //Manage health bar
        if (healthBar == null && healthBarPos != null)
        {
            healthBar = Instantiate(healthBarPrefab);
            healthBar.transform.position = transform.position + healthBarPos;
            healthBar.GetComponentInChildren<HealthBar>().SetHealth(currentHP / maximumHP);
        }
        else if(healthBar != null)
        {
            healthBar.GetComponentInChildren<HealthBar>().SetHealth(currentHP / maximumHP);
        }

        switch (weapon)
        {
            case Weapon_Type.SWORD:
                Vector3 targetPos = transform.position + (target.position - transform.position).normalized * -5f;
                transform.DOMove(targetPos, 2f);
                break;
            case Weapon_Type.BLUNT:
                Vector3 targetPos2 = transform.position + (target.position - transform.position).normalized * -10f;
                transform.DOMove(targetPos2, 2f);
                break;
            default:
                Vector3 targetPos3 = transform.position + (target.position - transform.position).normalized * -2f;
                transform.DOMove(targetPos3, 2f);
                break;
        }
    }

    public void Die()
    {
        GameObject deathParticle = Instantiate(DeathParticle, transform.position, Quaternion.identity) as GameObject;
        deathParticle.GetComponent<ParticleHandler>().emit(15, 1);
        LevelManager.RemoveEnemy(this);
        Destroy(healthBar);
        Destroy(gameObject);
    }
}

public enum EnemyType
{
    MELEE = 1,
    CHARGE = 2,
    RANGED = 3,
}
