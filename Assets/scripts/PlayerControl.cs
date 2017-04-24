using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour {

    public enum FACEDIRECTION { FACELEFT = -1, FACERIGHT = 1};
    public FACEDIRECTION Facing = FACEDIRECTION.FACERIGHT;

    private Transform thisTransform = null;
    private Rigidbody2D thisBody = null;
    public float maxSpeed = 1f;
    public string horzAxis = "Horizontal";

    public string jumpButtom = "Jump";
    public float jumpPower = 600;
    private bool canJump = true;
    public float jumpTimeOut = 1f;

    public CircleCollider2D feetCollider = null;
    public bool isGrounded = false;
    public LayerMask groundLayer;

    void Awake()
    {
        thisTransform = GetComponent<Transform>();
        thisBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {

        // Update grounded status
        isGrounded = GetGrounded();

        float horz = CrossPlatformInputManager.GetAxis(horzAxis);
        thisBody.AddForce(Vector2.right * horz * maxSpeed);

        if (CrossPlatformInputManager.GetButton(jumpButtom))
            Jump();

        // Clamp velocity
        thisBody.velocity = new Vector2(Mathf.Clamp(thisBody.velocity.x, -maxSpeed, maxSpeed),
            Mathf.Clamp(thisBody.velocity.y, -maxSpeed, maxSpeed));

        //Flip direction if required
        if ((horz < 0f && Facing != FACEDIRECTION.FACELEFT) || (horz > 0f && Facing != FACEDIRECTION.FACERIGHT))
            FlipDirection();
	}

    // return bool - is player on ground?
    private bool GetGrounded()
    {
        Vector2 circleCenter = new Vector2(thisTransform.position.x, thisTransform.position.y) + feetCollider.offset;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(circleCenter, feetCollider.radius, groundLayer);
        if (hitColliders.Length > 0) return true;
        return false;
    }

    private void FlipDirection()
    {
        Facing = (FACEDIRECTION)((int)Facing * -1f);
        Vector3 LocalScale = thisTransform.localScale;
        LocalScale.x *= -1f;
        thisTransform.localScale = LocalScale;
    }

    // Engage jump
    private void Jump()
    {
        // if we are grounded, then jump
        if (!isGrounded || !canJump) return;

        // Jump
        thisBody.AddForce(Vector2.up * jumpPower);
        canJump = false;
        Invoke("ActivateJump", jumpTimeOut);
    }

    // Activates can jump variable after jump timeout 
    // Prevents double-jumps
    private void ActivateJump()
    {
        canJump = true;
    }
}
