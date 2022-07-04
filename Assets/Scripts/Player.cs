using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    public Animator anim;
    public Transform point;
    private PlayerAudio playerAudio;

    public LayerMask enemyLayer;
    public float radius;

    private Health heathSystem;

    public float speed;
    public float jumpForce;
    private bool isJumping,doubleJump,isAttacking;

    private bool recovery;
    float recoveryCount=0f;

    private static Player instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance!=this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rig= GetComponent<Rigidbody2D>();
        playerAudio= GetComponent<PlayerAudio>();
        heathSystem= GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //se não preionar nada, retorna 0. se precinar direita 1 e esquerda -1
        float movement = Input.GetAxis("Horizontal");
        if (movement >0 &&!isJumping&&!isAttacking)
        {
            anim.SetInteger("Transition", 1);
            transform.eulerAngles=new Vector3(0,0,0);
        }
        else if (movement <0&&!isJumping && !isAttacking)
        {
            anim.SetInteger("Transition", 1);
            transform.eulerAngles=new Vector3(0,180,0);
        }
        else if(!isJumping&&!isAttacking)
        {
            anim.SetInteger("Transition", 0);
        }
            

        rig.velocity=new Vector2 (movement*speed, rig.velocity.y);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.X))
        {
            if(!isJumping)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if(doubleJump)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }

        }
    }

    void Attack()
    {
        if(Input.GetButtonDown("Fire1")||Input.GetKeyDown(KeyCode.Z))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);

            Collider2D hit = Physics2D.OverlapCircle(point.position, radius,enemyLayer);

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {
                //Debug.Log(hit.name);

                if(hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();
                }

                if (hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().OnHit();
                }
            }
            StartCoroutine(OnAttack());
        }

    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.33f);
        isAttacking=false;
    }

    
    public void OnHit()
    {
        recoveryCount += Time.deltaTime;

        if(recoveryCount >= 1f)
        {
            anim.SetTrigger("hit");
            heathSystem.health--;

            recoveryCount = 0f;
        }


        if(heathSystem.health <= 0&&!recovery)
        {
            recovery = true;
            anim.SetTrigger("die");
            //Destroy(gameObject,0.5f);
            //game over
            GameController.instance.ShowGameOver();
        }
    }

    public void OnTouch()
    {
        //heathSystem.health--;

        //if (heathSystem.health <= 0 && !recovery)
        //{
        //    recovery = true;
        //    anim.SetTrigger("die");
        //    //Destroy(gameObject, 0.5f);
        //    //game over
        //    GameController.instance.ShowGameOver();
        //}
        //else
        //{
        //    anim.SetTrigger("hit");
        //}
        OnHit();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(point.position,radius);
    }//desenha o ponto de ataque

    private void OnCollisionEnter2D(Collision2D collision)//verifica se ta pulando
    {
        if(collision.gameObject.layer==3)
        {
            isJumping = false;
            doubleJump = false;
        }

        if(collision.gameObject.layer==8)
        {
            PlayerPos.instance.CheckPoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==6)
        {
            OnTouch();
        }

        if(collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            GameController.instance.GetCoin();
            Destroy(collision.gameObject, 0.5f);
        }



        
    }
}
