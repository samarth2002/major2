using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerControls controls;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    

    float direction = 0;
    float voiceDir = 0f;
    public float speed = 400f;
    public float jumpForce = 5f;
    public float attackRange = 0.5f;
    bool isFacingRight = true;
    bool isGrounded;
    bool isAttacking1;
    int noOfJumps = 0;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;


    public Rigidbody2D playerRB;
    public Animator animator;
    public Transform groundCheck;
    public Transform attackPoint;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;


    private void configKeywordRecognizer()
    {
        actions.Add("right", Right);
        actions.Add("left", Left);
        actions.Add("jump", Jump);
        actions.Add("attack", Attack);
        actions.Add("stop", Stop);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void Awake(){
        controls = new PlayerControls();
        controls.Enable();

        controls.Land.Move.performed += ctx => {
            direction = ctx.ReadValue<float>();
        };

        controls.Land.Jump.performed += ctx => {
            Jump();
        };

        controls.Land.Attack.performed += ctx => {
            Attack();
        };
        configKeywordRecognizer();


    }


 

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        playerRB.velocity = new Vector2(direction * speed*Time.fixedDeltaTime, playerRB.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(direction));

        if(isFacingRight && direction < 0 || !isFacingRight && direction > 0){
            Flip();
        }
    }


    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);    
    }

    private void Attack()
    {
        animator.SetTrigger("attack1");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }
    private void Shoot()
    {
        Debug.Log("SHOOTING");
    }
    private void Right()
    {
        voiceDir = 1;
    }
    private void Left()
    {
        voiceDir = -1;
    }
    private void Stop()
    {
        voiceDir = 0;
    }


    void Flip(){
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x*-1, transform.localScale.y);
    }

    void Jump(){
        if(isGrounded){
            noOfJumps = 0;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            noOfJumps++;
        }else{
            if(noOfJumps == 1){
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                noOfJumps++;
            }
        }
    }
}
