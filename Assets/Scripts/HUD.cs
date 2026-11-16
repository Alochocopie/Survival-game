using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    private Text myText;
    private Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                // Logic cập nhật thanh kinh nghiệm
                // Tính tỷ lệ phần trăm kinh nghiệm hiện có
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];

                // Cập nhật giá trị cho Slider (từ 0 đến 1)
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                // Logic cập nhật level
                myText.text = string.Format("Level {0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                // Logic cập nhật số kill
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // Logic cập nhật thời gian
                // 1. Tính thời gian còn lại (Tổng thời gian trừ đi thời gian đã trôi qua)
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

                // 2. Chuyển đổi sang số nguyên để lấy phút và giây
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);

                // 3. Hiển thị định dạng 'Phút:Giây' (Luôn có 2 chữ số: D2)
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                // Logic cập nhật thanh máu
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;

                // Cập nhật giá trị cho Slider (từ 0 đến 1)
                mySlider.value = curHealth / maxHealth;
                break;
                
        }
    }
}
