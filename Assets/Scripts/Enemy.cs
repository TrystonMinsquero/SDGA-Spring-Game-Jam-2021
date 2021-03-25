using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public int[] weaknesses = { 0, 0, 0 }; // damage multiplier based on attack type
    public Rigidbody2D target;
    public bool enabled;
    public float moveSpeed = .1f;
    public double currentHP;
    public double maximumHP;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        
    }
}
