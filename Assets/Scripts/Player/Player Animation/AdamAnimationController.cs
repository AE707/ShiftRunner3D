using UnityEngine;

public class AdamAnimationController : MonoBehaviour
{

    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Speed up the animation to make walk look like run
       /* if (animator != null)
        {
            animator.speed = 1f; // Try values: 1.5f, 2f, 2.5f, 3f
        }*/
    }

    void Update()
    {
        if (Input.GetKey("space"))
        {
            animator.SetBool("Jumpi", true);
        }
        if (!Input.GetKey("space"))
        {
            animator.SetBool("Jumpi", false);
        }
    }
}
