using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Tao cộng thêm tên của thủ phạm (other.gameObject.name) để mày test cho sướng
        Debug.Log("Đang bị thiêu đốt bởi thằng: " + other.gameObject.name);
    }

    // Hàm này Unity tự động kích hoạt khi thằng đó đi ra khỏi vùng
    private void OnTriggerExit2D(Collider2D other) 
    {
        Debug.Log("Đã thoát");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Đang bị thiêu đốt bởi thằng: " + other.gameObject.name);
    }
}
