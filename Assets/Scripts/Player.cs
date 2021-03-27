using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Attributes")]
    public int max_health = 3;
    private int current_health;
    public int moveSpeed;
    public float attackDelay = 1f;
    public Weapon_Type weaponSelected;
    private float attackRange;

    [Header("Sword")]
    public int swordDamage = 30;
    [SerializeField]
    public float swordRange = .85f;
    [SerializeField]
    public float swordDistance = .5f;

    [Header("Blunt")]
    public int bluntDamage = 35;
    [SerializeField]
    public float bluntRange = .6f;
    [SerializeField]
    public float bluntDistance = .65f;

    [Header("Discus")]
    public int discusDamage = 25;
    public float discusRadius = 3f;
    public float discusDistance = 8;
    public float discusAcceleration = 10;

    private Discus disc = null;

    [Header("Globals")]
    public LayerMask enemyLayer;
    public Sprite discSprite;

    Transform attackPoint;
    Animator anim;

    Direction facing = Direction.DOWN;
    private bool moving;

    Rigidbody2D rb;
    Controls controls;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.Enable();

        //Attack Point Setup
        GameObject atkPoint = new GameObject("Attack Point");
        atkPoint.transform.parent = this.transform;
        attackPoint = atkPoint.transform;


        current_health = max_health;


        changeWeapon(0);
    }

    public void Attack()
    {
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                SwordAttack();
                break;
            case Weapon_Type.BLUNT:
                BluntAttack();
                break;
            case Weapon_Type.DISCUS:
                DiscusAttack();
                break;
        }
    }




    public void takeDamage()
    {
        current_health--;
        if (current_health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Death");
    }

    public void changeWeapon(int change)
    {
        weaponSelected += change;
        if ((int)weaponSelected > 2)
            weaponSelected = (Weapon_Type)0;
        else if ((int)weaponSelected < 0)
            weaponSelected = (Weapon_Type)2;
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                changeToSword();
                Debug.Log("changed to SWORD!");
                break;
            case Weapon_Type.BLUNT:
                changeToBlunt();
                Debug.Log("changed to BLUNT!");
                break;
            case Weapon_Type.DISCUS:
                changeToDiscus();
                Debug.Log("changed to DISCUS!");
                break;
        }
    }
    private void changeToSword()
    {
        weaponSelected = Weapon_Type.SWORD;
        attackRange = swordRange;
        float distance = swordDistance;
        switch (facing)
        {
            case Direction.UP:
                attackPoint.position = transform.position + new Vector3(0, distance, 0);
                break;
            case Direction.DOWN:
                attackPoint.position = transform.position + new Vector3(0, -distance, 0);
                break;
            case Direction.LEFT:
                attackPoint.position = transform.position + new Vector3(-distance, 0, 0);
                break;
            case Direction.RIGHT:
                attackPoint.position = transform.position + new Vector3(distance, 0, 0);
                break;
        }

        //update animation
    }

    private void changeToBlunt()
    {
        weaponSelected = Weapon_Type.BLUNT;
        attackRange = bluntRange;
        float distance = bluntDistance;
        switch (facing)
        {
            case Direction.UP:
                attackPoint.position = transform.position + new Vector3(0, distance, 0);
                break;
            case Direction.DOWN:
                attackPoint.position = transform.position + new Vector3(0, -distance, 0);
                break;
            case Direction.LEFT:
                attackPoint.position = transform.position + new Vector3(-distance, 0, 0);
                break;
            case Direction.RIGHT:
                attackPoint.position = transform.position + new Vector3(distance, 0, 0);
                break;
        }

        //update animation

    }

    private void changeToDiscus()
    {
        weaponSelected = Weapon_Type.DISCUS;
        attackRange = discusRadius;
        float distance = .5f;

        switch (facing)
        {
            case Direction.UP:
                attackPoint.position = transform.position + new Vector3(0, distance, 0);
                break;
            case Direction.DOWN:
                attackPoint.position = transform.position + new Vector3(0, -distance, 0);
                break;
            case Direction.LEFT:
                attackPoint.position = transform.position + new Vector3(-distance, 0, 0);
                break;
            case Direction.RIGHT:
                attackPoint.position = transform.position + new Vector3(distance, 0, 0);
                break;
        }

        //update animation
    }

    private void SwordAttack()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in collidersHit)
            if(enemy == enemy.GetComponent<Enemy>().hitbox)
                enemy.GetComponent<Enemy>().takeDamage(weaponSelected, swordDamage);
    }

    private void BluntAttack()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in collidersHit)
            if (enemy == enemy.GetComponent<Enemy>().hitbox)
                enemy.GetComponent<Enemy>().takeDamage(weaponSelected, bluntDamage);
    }
    
    private void DiscusAttack()
    {

        disc = Discus.create(this, facing);
        disc.throwDisc();
    }

    public void catchDiscus()
    {
        disc = null;
        changeWeapon(0);
    }

    private void Update()
    {

        if (controls.Gameplay.Attack.triggered && disc==null)
            Attack();

        if (controls.Gameplay.SwitchWeapon.triggered && disc==null)
        {
            changeWeapon((int)controls.Gameplay.SwitchWeapon.ReadValue<float>());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = controls.Gameplay.Movement.ReadValue<Vector2>();
        rb.position = rb.position + moveSpeed * direction * Time.fixedDeltaTime;
        moving = direction.magnitude > 0;
        rb.velocity = Vector2.zero;
        Direction dir = facing;

        //update direction
        if (direction.y > 0)
            facing = Direction.UP;
        if (direction.y < 0)
            facing = Direction.DOWN;
        if (direction.x < 0)
            facing = Direction.LEFT;
        if (direction.x > 0)
            facing = Direction.RIGHT;

        if (dir != facing)
            changeDirection();

        changeAnimationState();
    }



    private void changeDirection()
    {
        
        changeWeapon(0);

    }

    private void changeAnimationState()
    {
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                switch (facing)
                {
                    case Direction.UP:
                        break;
                    case Direction.DOWN:
                        break;
                    case Direction.LEFT:
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        if (moving) anim.Play("Player_Sun_Walk_Left");
                        break;
                    case Direction.RIGHT:
                        gameObject.transform.localScale = new Vector3(-1, 1, 1);
                        if (moving) anim.Play("Player_Sun_Walk_Left");
                        break;

                }
                break;
            case Weapon_Type.BLUNT:

                break;
            case Weapon_Type.DISCUS:

                break;
        }
    }

    private void showWeapon()
    {
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                changeToSword();
                Debug.Log("SWORD: " + (int)Weapon_Type.SWORD);
                break;
            case Weapon_Type.BLUNT:
                Debug.Log("BLUNT: " + (int)Weapon_Type.BLUNT);
                break;
            case Weapon_Type.DISCUS:
                Debug.Log("DISCUS: " + (int)Weapon_Type.DISCUS);
                break;
        }
    }

    private void showDirection()
    {
        switch (facing)
        {
            case Direction.UP:
                Debug.Log("UP");
                break;
            case Direction.DOWN:
                Debug.Log("DOWN");
                break;
            case Direction.LEFT:
                Debug.Log("LEFT");
                break;
            case Direction.RIGHT:
                Debug.Log("RIGHT");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
