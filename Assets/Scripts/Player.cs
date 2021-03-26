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
        switch (weaponSelected)
        {
            case Weapon_Type.SWORD:
                Debug.Log("SWORD!");
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
        rb.position = rb.position + moveSpeed * controls.Gameplay.Movement.ReadValue<Vector2>() * Time.fixedDeltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}