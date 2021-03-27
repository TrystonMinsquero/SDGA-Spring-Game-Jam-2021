using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyProjectile : MonoBehaviour
{   
    public void move(Vector3 startPos, Vector3 endPos, float moveTime) {
        transform.position = startPos;
        transform.DOMove(endPos, moveTime);
        StartCoroutine(DestroyObject(moveTime));
    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.gameObject.layer);
        if (col.gameObject.name == "Player Temp")
        {
            col.gameObject.GetComponent<Player>().takeDamage();
        }
        else if (col.gameObject.layer == 8) {
            DOTween.Kill(transform);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyObject(float timeUntil) {
        yield return new WaitForSeconds(timeUntil);
        DOTween.Kill(transform);
        Destroy(this.gameObject);
    }
}
