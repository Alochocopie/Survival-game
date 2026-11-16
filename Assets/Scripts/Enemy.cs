using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxhealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isAlive;
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    WaitForFixedUpdate wait;
    Collider2D coll;
    // SpriteRenderer sp;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
        // sp = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isAlive) return;
        sprite.flipX = target.position.x < rigid.position.x;

    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxhealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxhealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) return;

        health -= collision.GetComponent<Bullet>().damage;



        if (health > 0)
        {
            anim.SetTrigger("Hit");
            StartCoroutine(KnockBack());
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            DisableEnemy();
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }

    }
    public void Dead()
    {
        gameObject.SetActive(false);
    }

    public void DisableEnemy()
    {
        isAlive = false;
        coll.enabled = false;
        rigid.simulated = false;
        sprite.sortingOrder = 1;
        anim.SetBool("Dead", true);
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }


}
