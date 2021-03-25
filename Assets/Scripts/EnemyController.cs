using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GridController gridController;
    public float moveSpeed;

    public List<GameObject> enemies;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gridController.curFlowField == null) { return; }
        foreach (GameObject enemy in enemies)
        {
            Cell nodeBelow = gridController.curFlowField.GetCellFromWorldPos(enemy.transform.position);
            Vector3 moveDirection = new Vector3(nodeBelow.bestDirection.Vector.x, nodeBelow.bestDirection.Vector.y, 0);
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.velocity = moveDirection * moveSpeed;
        }
    }
}
