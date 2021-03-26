using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discus : MonoBehaviour
{
    Player player;
    Direction directionThrown;

    Rigidbody2D rb;
    ConstantForce2D force;

    public float enemyHitDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public static Discus create(Player player, Direction directionThrown)
    {
        GameObject disc = new GameObject("Disc");
        disc.AddComponent<SpriteRenderer>();
        disc.GetComponent<SpriteRenderer>().sprite = player.discSprite;
        disc.GetComponent<SpriteRenderer>().enabled = true;
        disc.AddComponent<Rigidbody2D>();
        disc.GetComponent<Rigidbody2D>().gravityScale = 0;
        disc.AddComponent<ConstantForce2D>();
        disc.AddComponent<Discus>();
        disc.GetComponent<Discus>().player = player;
        disc.GetComponent<Discus>().directionThrown = directionThrown;
        disc.GetComponent<Discus>().rb = disc.GetComponent<Rigidbody2D>();
        disc.GetComponent<Discus>().force = disc.GetComponent<ConstantForce2D>();

        disc.transform.position = player.transform.position;

        return disc.GetComponent<Discus>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, player.discusRadius, player.enemyLayer);
        Dictionary<Enemy, float> enemyHitTimes = new Dictionary<Enemy, float>();

        foreach (Collider2D enemy in collidersHit)
            if (enemy == enemy.GetComponent<Enemy>().hitbox)
            {
                if(enemyHitTimes.ContainsKey(enemy.GetComponent<Enemy>()))
                {
                    if(enemyHitTimes[enemy.GetComponent<Enemy>()] > Time.time)
                    {
                        enemy.GetComponent<Enemy>().takeDamage(Weapon_Type.DISCUS, player.discusDamage);
                        enemyHitTimes[enemy.GetComponent<Enemy>()] = Time.time + enemyHitDelay;
                    }
                }
                else
                {
                    enemy.GetComponent<Enemy>().takeDamage(Weapon_Type.DISCUS, player.discusDamage);
                    enemyHitTimes[enemy.GetComponent<Enemy>()] = Time.time + enemyHitDelay;
                }
            }

        FixPosition();
    }

    public void throwDisc()
    {

        float initVelocity = Mathf.Sqrt(2 * player.discusDistance * player.discusAcceleration);
        switch (directionThrown)
        {
            case Direction.UP:
                rb.velocity = Vector2.up * initVelocity;
                force.force = Vector2.down * player.discusAcceleration;
                break;
            case Direction.DOWN:
                rb.velocity = Vector2.down * initVelocity;
                force.force = Vector2.up * player.discusAcceleration;
                break;
            case Direction.LEFT:
                rb.velocity = Vector2.left * initVelocity;
                force.force = Vector2.right * player.discusAcceleration;
                break;
            case Direction.RIGHT:
                rb.velocity = Vector2.right * initVelocity;
                force.force = Vector2.left * player.discusAcceleration;
                break;
        }
    }

    private void catchDiscus()
    {
        player.catchDiscus();
        Destroy(this.gameObject);
    }


    private void FixPosition()
    {
        switch (directionThrown)
        {
            case Direction.UP:
                if (transform.position.y < player.transform.position.y)
                    catchDiscus();
                transform.position = new Vector3(player.transform.position.x, transform.position.y);
                break;
            case Direction.DOWN:
                if (transform.position.y > player.transform.position.y)
                    catchDiscus();
                transform.position = new Vector3(player.transform.position.x, transform.position.y);
                break;
            case Direction.LEFT:
                if (transform.position.x > player.transform.position.x)
                    catchDiscus();
                transform.position = new Vector3(transform.position.x, player.transform.position.y);
                break;
            case Direction.RIGHT:
                if (transform.position.x < player.transform.position.x)
                    catchDiscus();
                transform.position = new Vector3(transform.position.x, player.transform.position.y);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (transform != null)
            Gizmos.DrawWireSphere(transform.position, player.discusRadius);
    }
}
