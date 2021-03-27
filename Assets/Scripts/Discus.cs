using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discus : MonoBehaviour
{
    Player player;
    Direction directionThrown;

    Rigidbody2D rb;
    ConstantForce2D force;

    public static Discus create(Player player, Direction directionThrown)
    {

        GameObject discus = Instantiate(player.discusPrefab ,player.transform, true);

        discus.transform.position = player.transform.position;

        discus.GetComponent<Discus>().player = player;
        discus.GetComponent<Discus>().directionThrown = directionThrown;
        discus.GetComponent<Discus>().rb = discus.GetComponent<Rigidbody2D>();
        discus.GetComponent<Discus>().force = discus.GetComponent<ConstantForce2D>();


        return discus.GetComponent<Discus>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(Weapon_Type.DISCUS, player.discusDamage);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            rb.velocity = Vector2.zero;
        }
    }

}

