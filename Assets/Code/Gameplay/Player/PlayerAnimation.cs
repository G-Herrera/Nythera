using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMovement(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void UpdateGrounded(bool isGrounded)
    {
        animator.SetBool("IsGrounded", isGrounded);
    }
}
