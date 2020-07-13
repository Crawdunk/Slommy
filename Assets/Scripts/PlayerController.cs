using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Literally way too much stuff
    public float speed;
    public float jumpForce;
    float moveInput;
    //Still way too much
    private Rigidbody2D rb;

    bool facingRight = true;

    bool isGrounded;
    public Transform groundCheck;
    public Transform gmTrans;
    public float checkRadius;
    public LayerMask whatIsGround;
    //Even More
    private int extraJumps;
    public int extraJumpsValue;

    bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlidingSpeed;

    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    bool tripleJump;
    //Jesus why
    private Animator anim;
    private GameManager gm;

    public AudioSource jumpNoise;
    public AudioSource hitNoise;

    void Start()
    {
        extraJumpsValue = 1; //Sets the extra jumps value to 1 for a double jump
        extraJumps = extraJumpsValue; //Sets extraJumps to the value set by the dev.
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); //Grabs the animator and rigidbody components of the player
        gm = GameObject.FindWithTag("GM").GetComponent<GameManager>(); //Find GameManager
        transform.position = gmTrans.position; //Sets the initital spawn of the player before a checkpoint is met.
        tripleJump = false; //Sets tripleJump to false.
    }

    void Update()
    {
        

        //Move and Facing
        float input = Input.GetAxisRaw("Horizontal"); //Sets the default movement keys for Horizontal to move the x axis and sets it as a float.
        rb.velocity = new Vector2(input * speed, rb.velocity.y); //Take input and times it by dev set speed.

        if (facingRight == false && input > 0)
        {
            Flip();
        } else if (facingRight == true && input < 0)
        {
            Flip(); //Calls the flip function to find whether or not you are facing right or left.
        }

        //isGrounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround); //Creates a radius circle to check if it overlaps with a collider, if it does, it finds "whatIsGround" and declares that you are grouned based on the position of the empty game object groundCheck on the player.

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue; // If isGrounded is true, you're extra jumps are reset to the default value.
            anim.SetBool("isJumping", false); //The isJumping bool for the animator is set to false so you play the idle/running animation.
        } else
        {
            anim.SetBool ("isJumping", true); //If isGrounded is false play the jumping animation.
        }

        if (tripleJump == true)
        {
            extraJumpsValue = 2; //If tripleJump is true, set the default value of extraJumps to 2 so you jump 3 times.
        }

        //WallSlide, WallJump, and isTouchingFront
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround); //Similar to isGrounded, make an OverlapCircle to check if you are colliding with anything in front of you, that is considered ground.

        if (isTouchingFront == true && isGrounded == false && input != 0) //If you are touching a wall but not the ground, and you ARE moving, wallSliding = true/
        {
            wallSliding = true;
        } else
        {
            wallSliding = false; //If you arent you are not wallsliding.
        }

        if (wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue)); //if wallSliding do the math to determine how fast you are going with the variable wallSlidingSpeed.
            anim.SetBool("isJumping", true); //Set the Jumping animation to play
        }

        if (Input.GetKeyDown(KeyCode.W) && wallSliding == true)
        {
            wallJumping = true; //If you hit the jump key while wallSliding, you are now wallJumping
            Invoke("SetWallJumpingToFalse", wallJumpTime); //invoke the SetWallJumpingToFalse function and make it wait by a dev set time.
        }

        if (wallJumping == true)
        {
            rb.velocity = new Vector2(xWallForce * -input, yWallForce); //If walljumping is true create a new vector2 that multiplies your xWallForce by -input to see which direction you are going, and the yWallForce to see how high you go. These are dev set.
        }

        //Jumps and Movement
        if(Input.GetKeyDown(KeyCode.W) && extraJumps > 0) //If you hit jump and you're extra jumps are greater than 0
        {
            jumpNoise.Play(); //Play the jumpNoise audio.
            anim.SetTrigger("takeOff"); //Play the takeOff animation.
            rb.velocity = Vector2.up * jumpForce; //Make a Vector2.Up that is multiplied by the force of your jump (dev set)
            extraJumps--; //Subtract from your total of extraJumps.

        } else if (Input.GetKeyDown(KeyCode.W) && extraJumps == 0 && isGrounded == true) //If you hit jump and you have 0 extra jumps, and you are grounded
        {
            anim.SetTrigger("takeOff"); //Play the takeoff animation
            rb.velocity = Vector2.up * jumpForce; //jumpforce
        }

        if (input == 0) //if input does not have a value aka you arent pressing anything.
        {
            anim.SetBool("isRunning", false); //Set the isRunning animation bool to false
        } else
        {
            anim.SetBool("isRunning", true); //If it does set the isRunning animation bool to true and play the animation.
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler; //I had to look this one up and it sort of confuses me.
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false; //Simple function that sets wallJumping to false to be used with Invoke.
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Spikes") //If you collide with an object with the spikes tag.
        {
            hitNoise.Play(); //play the hitNoise audio
            transform.position = gm.lastCheckpointPos; //Reset your position back to the lastCheckpointPos of the gameManager
            gm.lives--; //subtract from your players lives.
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Upgrade")) //If you collide with the Upgrade trigger, it sets triple jump to true and destroys the upgrade object.
        {
            tripleJump = true;
            Destroy(col.gameObject);
        }
    }
}
