using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isFront;
    private bool isDead;
    
    public Transform point;
    public Transform behind;

    private Vector2 direction;
    private Animator anim;

    public bool isRight;
    public float stopDistance;

    public float speed;
    public float maxVision;
    public int health;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();

        if (isRight)//vira pra direita
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else//vira pra esquerda
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    // Update is called once per frame
    void Update()
    {
  
        
    }
    private void FixedUpdate()
    {
        GetPlayer();

        OnMove();
    }

    void OnMove()
    {

        if (isFront && !isDead)
        {
            anim.SetInteger("transition", 1);

            if (isRight)//vira pra direita
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else//vira pra esquerda
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig.velocity = new Vector2(-speed, rig.velocity.y);
            }
        }

    }

    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        if(hit.collider!=null && !isDead)//se ver algo
        {
            //Debug.Log(hit.transform.name);
            if(hit.transform.CompareTag("Player"))//se esse algo for o player
            {
                //Debug.Log("to te vendo em");
                isFront=true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);
               
                if(distance<=stopDistance)//distancia para atacar
                {
                    isFront = false;
                    rig.velocity = Vector2.zero;

                    anim.SetInteger("transition", 2);

                    hit.transform.GetComponent<Player>().OnHit();
                }
            }
            else
             {
                 isFront=false;
                 rig.velocity = Vector2.zero;

                 anim.SetInteger("transition", 0);
             }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision-maxVision/2);
        
        if(behindHit.collider!=null && !isDead)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                //player ta atras
                isRight=!isRight;
                isFront=true;
            }
        }

    }

    public void OnHit()
    {
        health--;

        if (health <= 0)
        {
            isDead = true;
            speed = 0;
            anim.SetTrigger("die");
            Destroy(gameObject, 0.5f);
        }
        else
        {
            anim.SetTrigger("hit");
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(point.position,direction * maxVision);
        Gizmos.DrawRay(behind.position, -direction * (maxVision-maxVision/2));
    }//desenha o ponto de ataque
}
