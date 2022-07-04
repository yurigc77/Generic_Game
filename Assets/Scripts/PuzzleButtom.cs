using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtom : MonoBehaviour
{
    private Animator anim;
    public Animator barrier;

    private void Start()
    {
        anim=GetComponent<Animator>();
    }

    void OnPressed()
    {
        anim.SetBool("isPressed", true);
        barrier.SetBool("isPressed", true);
    }

    void OnExit()
    {
        anim.SetBool("isPressed", false);
        barrier.SetBool("isPressed", false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Stone"))
        {
            OnPressed();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stone"))
        {
            OnExit();
        }
    }
}
