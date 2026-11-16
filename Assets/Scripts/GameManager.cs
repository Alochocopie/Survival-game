using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    public PoolManager poolManager;

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("Player Info")]
    public int level;
    public int kill;
    public int exp;
    public float health;
    public float maxHealth = 100f;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 270, 360, 450, 600 };

    public bool isLive; // Công tắc sinh tử
    public LevelUp uiLevelUp;

    public Result uiResult;

    void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        health = maxHealth;
        uiLevelUp.Select(0);
        isLive = true;
        AudioManager.instance.PlayBgm(true);

    }

    void Update()
    {
        if (!isLive) return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive) return;
        exp++;
        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // Ngừng đọng thời gian thực tế của Unity
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // Khôi phục thời gian
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        // Ngừng các logic game
        yield return new WaitForSeconds(0.3f); // Đợi 0.5 giây cho animation bia mộ
        uiResult.gameObject.SetActive(true);
        uiResult.Lose(); // Hiển thị bảng Game Over
        Stop(); // Dừng hoàn toàn thời gian game
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);

    }

    public void GameRetry()
    {
        Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        SceneManager.LoadScene(0);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        foreach (Transform child in poolManager.transform)
        {
            if (child.gameObject.activeSelf && child.CompareTag("Enemy"))
            {
                Enemy enemyScript = child.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.DisableEnemy(); // Quái vẫn nổ máu, vẫn giãy chết, nhưng CPU thì nhàn tênh!
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop(); // Dừng game
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // Gọi hàm hiển thị UI chiến thắng

    }
}
