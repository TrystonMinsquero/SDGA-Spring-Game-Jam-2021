using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool attackDebounce;
    public BoxCollider2D weaponCollider;
    public int[] weaknesses = { 0, 0, 0 }; // damage multiplier based on attack type
    public Rigidbody2D target;
    public int type; // 1 = melee 2 = ranged
    public double currentHP;
    public double maximumHP;
    public float moveVal = 0.1f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        attackDebounce = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player Temp" && !attackDebounce) {
            transform.position = Vector3.MoveTowards(transform.position, col.gameObject.transform.position, moveVal);
            attackDebounce = true;
            Sleep(2);
            attackDebounce = false;
        }
    }

    IEnumerator Sleep(float time) {
        yield return new WaitForSeconds(time);
    }
}
