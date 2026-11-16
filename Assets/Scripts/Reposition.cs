using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float diffX = playerPos.x - myPos.x;
        float diffY = playerPos.y - myPos.y;

        float dirX = diffX < 0 ? -1 : 1;
        float dirY = diffY < 0 ? -1 : 1;

        diffX = Mathf.Abs(diffX);
        diffY = Mathf.Abs(diffY);

        if (transform.CompareTag("Ground"))
        {
            if (diffX > diffY)
            {
                transform.Translate(Vector3.right * dirX * 40);
            }
            else if (diffX < diffY)
            {
                transform.Translate(Vector3.up * dirY * 40);
            }
        }
        else if (transform.CompareTag("Enemy"))
        {
            if (coll.enabled)
            {
                Vector3 diffVec = playerPos - myPos;
                Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0f);
                transform.Translate(diffVec * 2 + randomOffset);
            }
        }
    }
}
