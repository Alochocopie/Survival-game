using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementt : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(moveX, moveY);

        direction = direction.normalized;

        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Debug.Log("lenght of current vector: " + direction.magnitude);
        
    }
}
