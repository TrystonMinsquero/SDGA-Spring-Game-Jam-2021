using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discus : MonoBehaviour
{
    Player player;
    Direction directionThrown;

    Rigidbody2D rb;
    ConstantForce2D force;
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

        foreach (Collider2D enemy in collidersHit)
            if (enemy == enemy.GetComponent<Enemy>().hitbox)
                enemy.GetComponent<Enemy>().takeDamage(Weapon_Type.DISCUS, player.discusDamage);

        FixPosition();
    }

    public void throwDisc()
    {

        float acceleration = Mathf.Pow(player.discusInitVelocity, 2) / (2 * player.discusDistance);
        switch (directionThrown)
        {
            case Direction.UP:
                rb.velocity = Vector2.up * player.discusInitVelocity;
                force.force = Vector2.down * acceleration;
                break;
            case Direction.DOWN:
                rb.velocity = Vector2.down * player.discusInitVelocity;
                force.force = Vector2.up * acceleration;
                break;
            case Direction.LEFT:
                rb.velocity = Vector2.left * player.discusInitVelocity;
                force.force = Vector2.right * acceleration;
                break;
            case Direction.RIGHT:
                rb.velocity = Vector2.right * player.discusInitVelocity;
                force.force = Vector2.left * acceleration;
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
                if (transform.position.y <= player.transform.position.y)
                    catchDiscus();
                transform.position = new Vector3(player.transform.position.x, transform.position.y);
                break;
            case Direction.DOWN:
                if (transform.position.y >= player.transform.position.y)
                    catchDiscus();
                transform.position = new Vector3(player.transform.position.x, transform.position.y);
                break;
            case Direction.LEFT:
                if (transform.position.x >= player.transform.position.x)
                    catchDiscus();
                transform.position = new Vector3(transform.position.x, player.transform.position.y);
                break;
            case Direction.RIGHT:
                if (transform.position.x <= player.transform.position.x)
                    catchDiscus();
                transform.position = new Vector3(transform.position.x, player.transform.position.y);
                break;
        }
    }
}
