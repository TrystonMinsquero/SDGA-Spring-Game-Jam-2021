using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyProjectile : MonoBehaviour
{   
    public void move(Transform startEnemy, Vector3 endPos, float moveTime) {
        transform.position = startEnemy.position;
        transform.DOMove(endPos, moveTime);
        transform.up = -1* (startEnemy.position - endPos).normalized;
        StartCoroutine(DestroyObject(moveTime));
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().takeDamage();
        }
        if (col.gameObject.tag == "Wall") {
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
