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
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().takeDamage();
            DOTween.Kill(transform);
            Destroy(this.gameObject);
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
