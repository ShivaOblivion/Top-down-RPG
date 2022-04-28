using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Player_actions player_actions;

    public SpriteRenderer playerSprite;
    private Rigidbody2D rb;

    public AnimationCurve jumpCurve;        //Courbe de progression du saut

    public float jumpDuration;
    public float moveSpeed;
    public float sprintAddedSpeed;          //Vitesse ajouté lors du sprint

    private bool isJumping;
    private bool isSprinting = false;
    private Vector3 groundPosition;

    private void Awake()
    {
        player_actions = new Player_actions();
    }

    private void OnEnable()
    {
        player_actions.Enable();
    }

    private void OnDisable()
    {
        player_actions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVec = player_actions.Basic.Movements.ReadValue<Vector2>();
        inputVec.Normalize();

        rb.velocity = inputVec * moveSpeed;
    }

    public void OnJump()
    {
        if (!isJumping)
        {
            StartCoroutine(JumpCo(0.5f, 0.0f));         //Le deuxième paramètre n'est pas utilisé, le premier détermine la hauteur du saut.
        }
    }

    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;
        groundPosition = playerSprite.transform.localPosition;

        float startTime = Time.time;
        

        while(isJumping)
        {
            float jumpCompletionRatio = (Time.time - startTime) / jumpDuration;
            jumpCompletionRatio = Mathf.Clamp01(jumpCompletionRatio);

            playerSprite.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletionRatio)*jumpHeightScale;
            playerSprite.transform.localPosition = groundPosition + Vector3.up * jumpCurve.Evaluate(jumpCompletionRatio) * playerSprite.size.y;

            if (jumpCompletionRatio == 1.0f) break;

            yield return null;
        }

        playerSprite.transform.localScale = Vector3.one;
        playerSprite.transform.localPosition = groundPosition;
        isJumping = false;
    }

    public void OnSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            moveSpeed -= sprintAddedSpeed;
        }
        else
        {
            isSprinting = true;
            moveSpeed += sprintAddedSpeed;
        }
    }

    /*public void OnMovements(InputValue input)                 //Old way we did it
    {
        Vector2 inputVec = input.Get<Vector2>();
        inputVec.Normalize();   //Fixe la vitesse de déplacement en diagonal
        movingVec = inputVec;

        if (!isSprinting) rb.velocity = inputVec * moveSpeed;
        else rb.velocity = inputVec * (moveSpeed + sprintAddedSpeed);
    }*/

    /*public void OnSprint()                                //Old way we did it
    {
        if (isSprinting)
        {
            isSprinting = false;
            rb.velocity -= movingVec * (sprintAddedSpeed);
        } else
        {
            isSprinting = true;
            rb.velocity += movingVec * (sprintAddedSpeed);
        }

    }*/
}
