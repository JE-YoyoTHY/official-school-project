using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private GameObject player;
    private PlayerControlScript playerControlScript;
    private PlayerGroundTriggerScript playerGroundTriggerScript;

    #region States
    // states, 同時也是動畫的名稱
    public string currentState;
    public string STATE_IDLE { get; private set; } = "PlayerIdle";
    public string STATE_RUN { get; private set; } = "PlayerRun";
    public enum STATE_SHOOT_ENUM
    {
        // 上為0, 順時針轉依序為1, 2, 3...
        TOP = 0,
        TOP_RIGHT = 1,
        RIGHT = 2,
        BOTTOM_RIGHT = 3,
        BOTTOM = 4,
        BOTTOM_LEFT = 5,
        LEFT = 6,
        TOP_LEFT = 7,
    };
    public string STATE_JUMP { get; private set; } = "PlayerJump";
    public string STATE_FALL { get; private set; } = "PlayerFall";
    public string STATE_LAND { get; private set; } = "PlayerLand";

    private List<string> availableAnims = new List<string>()
    {
        "PlayerRun", "PlayerIdle", "PlayerJump", "PlayerFall", "PlayerLand"
    };
    #endregion

    private bool previousIsGrounded;
    private bool currentIsGrounded;
    private sbyte facingDir;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerControlScript = player.GetComponent<PlayerControlScript>();
        playerGroundTriggerScript = player.GetComponentInChildren<PlayerGroundTriggerScript>(false);  // false 代表只偵測active的子物件
        
    }

    void Update()
    {
        transform.position = GameObject.Find("Player").transform.position;
        facingDir = playerControlScript.moveKeyValue;
        flipPlayerSprite(facingDir);
        stateDetect();
    }
    public void changeState(string newState)
    {
        currentState = newState;
        if (availableAnims.Contains(newState))
        {
            animator.Play(currentState);
        }
    }

    public void stateDetect()
    {
        // run
        if (playerControlScript.isMoving == true && playerGroundTriggerScript.isGrounded == true)
        {
            changeState(STATE_RUN);
        }
        else if (playerControlScript.isJumping == true)
        {
            changeState(STATE_JUMP);  // 目前先這樣測試
        }
        else if (playerControlScript.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0 && playerGroundTriggerScript.isGrounded == false)
        {
            changeState(STATE_FALL);
        }
        else if (isLandNow() == true)
        {
            print("land");
            changeState(STATE_LAND);
        }
        else
        {
            changeState(STATE_IDLE);
        }

    }

    public void flipPlayerSprite(sbyte facingDir)
    {
        bool shouldFlip = false;
        if (facingDir == 0) { return; }
        if (facingDir == 1) { shouldFlip = false; }
        else if (facingDir == -1) {  shouldFlip = true; }

        spriteRenderer.flipX = shouldFlip;
        
    }

    public bool isLandNow()
    {
        previousIsGrounded = currentIsGrounded;
        currentIsGrounded = playerGroundTriggerScript.isGrounded;

        if (previousIsGrounded == false && currentIsGrounded == true)  // 代表剛落地
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
