using System.Collections;
using UnityEngine;

public class TransfomTools : MonoBehaviour {
    public delegate void TransfomCallBack();
    public Coroutine moveToPositionByCoroutine(Transform transform, Vector3 destroyPosition, float moveSpeed)
        => StartCoroutine(MoveToPosition(transform, destroyPosition, moveSpeed, -1));
    public Coroutine moveToPositionByCoroutine(Transform transform, Vector3 destroyPosition, float moveSpeed, int inTime, TransfomCallBack callBack)
        => StartCoroutine(moveToPositionAndCallBack(transform, destroyPosition, moveSpeed, inTime, callBack));

    public void stopCoroutine(Coroutine c)
        => StopCoroutine(c);
    public IEnumerator MoveToPosition(Transform transform, Vector3 targetPosition, float moveSpeed, int inTime) {
        Collider2D collider = transform.gameObject.GetComponent<Collider2D>();
        Rigidbody2D rb = transform.gameObject.GetComponent<Rigidbody2D>();
        if (collider != null)
            collider.enabled = false;
        if (rb != null)
            rb.gravityScale = 0;
        float timer = 0;
        while (transform != null && transform.position != targetPosition) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            if (inTime != -1 && timer > inTime) {
                transform.position = targetPosition;
                yield break;
            }
            yield return 0;
        }
        if (collider != null)
            collider.enabled = true;
        if (rb != null)
            rb.gravityScale = 1;
    }
    public IEnumerator moveToPositionAndCallBack(Transform transform, Vector3 targetPosition, float moveSpeed, int inTime, TransfomCallBack callBack) {
        yield return StartCoroutine(MoveToPosition(transform, targetPosition, moveSpeed, inTime));
        if (callBack != null)
            callBack();
    }
}
