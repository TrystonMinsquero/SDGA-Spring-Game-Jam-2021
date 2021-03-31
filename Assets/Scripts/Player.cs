using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class Player : MonoBehaviour
{

    [Header("Attributes")]
    public int max_health = 3;
    public int current_health;
    public int moveSpeed;
    public float attackDelay = 1f;
    public Weapon_Type weaponSelected;
    public float stunDelay;
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
    public GameObject discusPrefab;
    public int discusDamage = 25;
    public float discusRadius = 3f;
    public float discusDistance = 8;
    public float discusAcceleration = 10;

    private Discus disc = null;

    [Header("Globals")]
    public LayerMask enemyLayer;
    public Light2D glowLight;
    [ColorUsageAttribute(true, true)]
    public Color sunModeBloom;
    [ColorUsageAttribute(true, true)]
    public Color moonModeBloom;
    [ColorUsageAttribute(true, true)]
    public Color starModeBloom;

    Transform attackPoint;
    Animator anim;

    Direction facing = Direction.DOWN;
    private bool moving;
    private float timeForStun;
    private float attackTime;

    Material material;
    Rigidbody2D rb;
    Controls controls;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        material = this.GetComponent<SpriteRenderer>().material;
        controls = new Controls();
        controls.Enable();

        //Attack Point Setup
        GameObject atkPoint = new GameObject("Attack Point");
        atkPoint.transform.parent = this.transform;
        attackPoint = atkPoint.transform;


        current_health = max_health;

        changeWeapon(0);
        HUD.updateHearts(current_health);
    }

    public void Attack(Direction attackDirection)
    {
        attackTime = Time.time + attackDelay;
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                SwordAttack(attackDirection);
                break;
            case Weapon_Type.BLUNT:
                BluntAttack(attackDirection);
                break;
            case Weapon_Type.DISCUS:
                DiscusAttack(attackDirection);
                break;
        }

        playAttackAnimation(attackDirection);
    }


    public void takeDamage()
    {
        if(Time.time > timeForStun)
        {
            StartCoroutine(Flash(stunDelay));
            current_health--;
            HUD.updateHearts(current_health);
            if (current_health <= 0)
                Die();
            timeForStun = Time.time + stunDelay;
        }
    }

    private IEnumerator Flash(float duration, float stunInterval = .2f)
    {
        float timeUntilFlash = Time.time;
        for(float i = duration; i >= 0; i -= Time.deltaTime)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return null;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void Die()
    {
        LevelManager.End();
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
                break;
            case Weapon_Type.BLUNT:
                changeToBlunt();
                break;
            case Weapon_Type.DISCUS:
                changeToDiscus();
                break;
        }

        glowLight.color = material.color;
    }
    private void changeToSword()
    {
        material.color = sunModeBloom;
        weaponSelected = Weapon_Type.SWORD;
        attackRange = swordRange;
        
        //update animation
    }

    private void changeToBlunt()
    {
        material.color = moonModeBloom;
        weaponSelected = Weapon_Type.BLUNT;
        attackRange = bluntRange;

        //update animation

    }

    private void changeToDiscus()
    {
        material.color = starModeBloom;
        weaponSelected = Weapon_Type.DISCUS;
        attackRange = discusRadius;

        //update animation
    }

    private void SwordAttack(Direction attackDirection)
    {

        float distance = swordDistance;
        switch (attackDirection)
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
        
        //Play Attack animation

        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in collidersHit)
            if (enemy == enemy.GetComponent<Enemy>().hitbox)
                enemy.GetComponent<Enemy>().takeDamage(weaponSelected, swordDamage);
    }

    private void BluntAttack(Direction attackDirection)
    {
        float distance = bluntDistance;
        switch (attackDirection)
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

        //Play Moon Attack Animation

        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in collidersHit)
            if (enemy == enemy.GetComponent<Enemy>().hitbox)
                enemy.GetComponent<Enemy>().takeDamage(weaponSelected, bluntDamage);
    }
    
    private void DiscusAttack(Direction attackDirection)
    {

        float distance = .5f;

        switch (attackDirection)
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

        //Play Star Attack animation

        disc = Discus.create(this, attackDirection);
        disc.throwDisc();
    }

    public void catchDiscus()
    {
        disc = null;
        changeWeapon(0);
    }

    private void Flash()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
    }

    private void Update()
    {

        if (Time.time > attackTime && disc == null && controls.Gameplay.Attack.ReadValue<Vector2>() != Vector2.zero)
        {
            Vector2 direction = controls.Gameplay.Attack.ReadValue<Vector2>();
            if (direction.y > 0)
                Attack(Direction.UP);
            else if (direction.y < 0)
                Attack(Direction.DOWN);
            else if (direction.x < 0)
                Attack(Direction.LEFT);
            else if (direction.x > 0)
                Attack(Direction.RIGHT);
        }

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
        else if (direction.y < 0)
            facing = Direction.DOWN;
        else if (direction.x < 0)
            facing = Direction.LEFT;
        else if (direction.x > 0)
            facing = Direction.RIGHT;

        if (dir != facing)
            changeDirection();

        changeAnimationState();
    }

    public void addHealth(int change)
    {
        current_health += change;
    }

    public void fillHealth()
    {
        current_health = max_health;
    }

    private void changeDirection()
    {
        
        changeWeapon(0);

    }

    private void playAttackAnimation(Direction attackDirection)
    {
        string state = "";
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                state += "Sun";
                break;
            case Weapon_Type.BLUNT:
                state += "Moon";
                break;
            case Weapon_Type.DISCUS:
                state += "Star";
                break;
        }
        switch (attackDirection)
        {
            case Direction.UP:
                state += "Up";
                break;
            case Direction.DOWN:
                state += "Down";
                break;
            case Direction.LEFT:
                state += "Left";
                break;
            case Direction.RIGHT:
                state += "Right";
                break;
        }

        anim.Play(state + "Attack");
    }

    private void changeAnimationState()
    {
        string state = "";
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                state += "Sun";
                break;
            case Weapon_Type.BLUNT:
                state += "Moon";
                break;
            case Weapon_Type.DISCUS:
                state += "Star";
                break;
        }

        switch (facing)
        {
            case Direction.UP:
                state += "Up";
                break;
            case Direction.DOWN:
                state += "Down";
                break;
            case Direction.LEFT:
                state += "Left";
                break;
            case Direction.RIGHT:
                state += "Right";
                break;
        }

        state += moving ? "Moving" : "Idle";

        anim.Play(state);
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
