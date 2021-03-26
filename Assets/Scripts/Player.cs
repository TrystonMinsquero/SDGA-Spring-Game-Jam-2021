using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Attributes")]
    public int max_health = 3;
    public int moveSpeed;
    [SerializeField]
    public float attackRange;
    public Weapon_Type weaponSelected;

    [Header("Globals")]
    public LayerMask enemyLayer;

    public Transform attackPoint;

    private int health;
    private Direction facing;

    Rigidbody2D rb;
    Controls controls;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.Enable();
    }

    public void Attack()
    {
        switch (weaponSelected)
        {

            case Weapon_Type.SWORD:
                Debug.Log("SWORD!");
                SwordAttack();
                break;
            case Weapon_Type.BLUNT:
                Debug.Log("BLUNT!");
                break;
            case Weapon_Type.DISCUS:
                Debug.Log("DISCUS!");
                break;
        }
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
                Debug.Log("changed to BLUNT!");
                break;
            case Weapon_Type.DISCUS:
                Debug.Log("changed to DISCUS!");
                break;
        }
    }
    private void changeToSword()
    {
        weaponSelected = Weapon_Type.SWORD;
        attackRange = 1;
        switch (facing)
        {
            case Direction.UP:
                attackPoint.position = transform.position + new Vector3(0,.5f,0);
                break;
            case Direction.DOWN:
                attackPoint.position = transform.position + new Vector3(0, -.5f, 0);
                break;
            case Direction.LEFT:
                attackPoint.position = transform.position + new Vector3(-.5f, 0, 0);
                break;
            case Direction.RIGHT:
                attackPoint.position = transform.position + new Vector3(.5f, 0, 0);
                break;
        }
    }

    private void SwordAttack()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in collidersHit)
        {
            Debug.Log("Hit Something!");
        }
    }

    private void BluntAttack()
    {
        Debug.Log("Attack with Blunt");
    }
    
    private void DiscusAttack()
    {
        Debug.Log("Attack with Discus");
    }

    private void Update()
    {

        if (controls.Gameplay.Attack.triggered)
            Attack();

        if (controls.Gameplay.SwitchWeapon.triggered)
            changeWeapon((int)controls.Gameplay.SwitchWeapon.ReadValue<float>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = controls.Gameplay.Movement.ReadValue<Vector2>();
        rb.position = rb.position + moveSpeed * direction * Time.fixedDeltaTime;

        Direction dir = facing;
        //update direction
        if (direction.y > 0)
            facing = Direction.UP;
        if (direction.y < 0)
            facing = Direction.DOWN;
        if (direction.x > 0)
            facing = Direction.RIGHT;
        if (direction.x < 0)
            facing = Direction.LEFT;

        if (dir != facing)
            showDirection();
            
        changeDirection();
    }



    private void changeDirection()
    {
        
        changeToSword();

        //change animation

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


enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
