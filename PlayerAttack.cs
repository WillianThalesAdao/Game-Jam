using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

//Esse script serve para o ataque
public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    public Animator camAnim;
    public Animator playerAnim;
    public float attackRangerX;
    public float attackRangerY;

    public class Dano
    {
        int danoLevel = 3;
    }
                                        //script modificado por outro
    private void Awake()
    {
        camAnim = GameObject.Find("MainCamera").GetComponent<Animator>();
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
    }
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                //voce pode atacar
                camAnim.SetTrigger("shake");
                playerAnim.SetTrigger("attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangerX, attackRangerY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++) {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamege(damage);
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3 (attackRangerX, attackRangerY, 1));
    }
}

