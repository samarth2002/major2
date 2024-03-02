using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    public float attackCooldown;
    public float range;
    public int damage;

   

    [Header("Collider Parameters")]
    public float colliderDistance;
    public BoxCollider2D boxCollider;



    [Header("Player Layer")]

    private float coolDownTimer = Mathf.Infinity;
    public LayerMask playerLayer;


    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;





    private void Awake(){
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    void Update()
    {
        coolDownTimer += Time.deltaTime;
        if(PlayerInSight()){
            if(coolDownTimer >= attackCooldown){
                coolDownTimer = 0;
                anim.SetTrigger("attack");
            }
        }

        if(enemyPatrol != null){
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight(){
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center+ transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x*range, boxCollider.bounds.size.y,boxCollider.bounds.size.z)
        , 0, Vector2.left, 0, playerLayer);

        if(hit.collider != null){
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    private void DamagePlayer(){
        if(PlayerInSight()){
            playerHealth.TakeDamage(damage);
        }
    }
}
