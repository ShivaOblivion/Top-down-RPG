using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Player_actions player_actions;

    public SpriteRenderer playerSprite;
    private Rigidbody2D rb;

    public AnimationCurve jumpCurve;

    public float moveSpeed;

    private bool isJumping;

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
        if (player_actions.Basic.Jump.triggered) Jump(1.0f, 0.0f);

    }

    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        if (!isJumping)
        {
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale));
        }
    }

    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;

        float startTime = Time.time;
        float jumpDuration = 2;

        while(isJumping)
        {
            float jumpCompletionRatio = (Time.time - startTime) / jumpDuration;
            jumpCompletionRatio = Mathf.Clamp01(jumpCompletionRatio);

            playerSprite.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletionRatio)*jumpHeightScale;

            if (jumpCompletionRatio == 1.0f) break;

            yield return null;
        }

        playerSprite.transform.localScale = Vector3.one;
        isJumping = false;
    }

    public void OnJumpAlt()
    {
        Debug.Log("And so he jumped");
    }

    public void OnMovements(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        rb.velocity = inputVec * moveSpeed;

        Debug.Log("And so he moved");
    }
}
