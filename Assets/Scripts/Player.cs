using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed;
    public Weapon_Type weaponSelected;


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
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position = rb.position + moveSpeed * controls.Gameplay.Movement.ReadValue<Vector2>() * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}