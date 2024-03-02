using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    private PlayerMovement playerMovement;


    void Awake(){
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        animator.SetTrigger("hit");


        if(currentHealth <= 0){
           
            Die();
        }
    }

    void Die(){
        Debug.Log("died");
        animator.SetTrigger("dead");
        if(rigidbody2D)
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        if(playerMovement != null){
            playerMovement.enabled = false;
        }
        if (GetComponentInParent<EnemyPatrol>() != null)
            GetComponentInParent<EnemyPatrol>().enabled = false;

        if (GetComponent<MeleeEnemy>() != null)
            GetComponent<MeleeEnemy>().enabled = false;

        if (boxCollider2D){
            boxCollider2D.enabled = false;
        }

        this.enabled = false;
    }

}
