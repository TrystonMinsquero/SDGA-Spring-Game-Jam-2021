using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed;

    Rigidbody2D rb;
    Controls controls;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.Enable();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = rb.position + moveSpeed * controls.Gameplay.Movement.ReadValue<Vector2>() * Time.fixedDeltaTime;
    }
}