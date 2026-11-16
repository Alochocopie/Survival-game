using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items; // Mảng chứa toàn bộ thẻ bài Item (có sẵn trong Panel)

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next(); // Xáo trộn thẻ
        rect.localScale = Vector3.one; // Bật UI
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        GameManager.instance.Stop(); // Dừng game
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero; // Tắt UI
        GameManager.instance.Resume(); // Tiếp tục game
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1. Tắt hết toàn bộ item
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        // 2. Thuật toán Random 3 số không trùng nhau
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2]) break;
        }

        // 3. Bật 3 item đã bốc thăm
        for (int i = 0; i < ran.Length; i++) {
            Item ranItem = items[ran[i]];

            // CHỐT HẠ: Nếu Item này đã max cấp (level bằng với độ dài mảng damages)
            if (ranItem.level == ranItem.data.damages.Length) {
                // Ép hiển thị Item Hồi Máu (giả sử item Hồi máu nằm ở vị trí cuối cùng trong mảng items)
                items[items.Length - 1].gameObject.SetActive(true);
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}