using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : FastSingleton<Character>
{
    [SerializeField] private Animator anim;
    [SerializeField] private float speed;

    private const float TIME_TO_FALL = 2f;  // Limit time if player not move --> fall
    private const int NUMBERS_OF_STEP_TO_INCREASE_DIFFICULTY = 50;  // every 50 points --> increase difficulty by decrease
                                                                    // time to fall
    private const float TIME_INCREASE_BY_DIFFICULTY = .1f;  // time decrease each 50 points
    private const float MIN_TIME_DIFFICULTY = .1f;  // min time decrease
    private const float MAX_TIME_DIFFICULTY = .5f;  // max time decrease
    private const float RANGE_DETECT = .3f;     // range detect ground, diamond, trap
    private const float TIME_COMBO = .5f;   // if time of tap screen < 0.5s --> increase combo
    private const int INCREASE_POINT = 2;   // if player eat diamond --> point per step multiply to increase point
    private const float TIMER_INCREASE_POINT = 5f;  // if player eat diamond --> have 5s to increase point
    private const float FALL_SPEED = 10f;    // fall speed when player lose
    
    public int score;
    public int combo;
    public float difficultyTime;    // total time decrease of difficulty
    public float timerForFall;  // timer check if player not move --> lose
    public int increasePoint;   // increase point when player eat diamond
    public float timerIncreasePoint;    // timer for increase point when player eat diamond

    private float interpolaForJump; // interpolate for player jump
    // start, end for player move
    private Vector3 start;  
    private Vector3 target;
    private bool isMoving;  // check if player is moving or not --> for next click
    private bool isLose;
    private int currentAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }
    /// <summary>
    /// OnInit reset all value when start game
    /// </summary>
    private void OnInit()
    {
        start = transform.position;
        target = transform.position;
        interpolaForJump = 0f;

        increasePoint = 1;
        timerIncreasePoint = 0;

        isLose = false;

        combo = 0;
        timerForFall = 0f;
        score = 0;
        difficultyTime = 0f;
    }
    private void Update() {
        // block when player lose or on setting
        if(isLose)
        {
            return;
        }
        if(GameplayManager.instance.onSetting)
        {
            return;
        }
        // if character is not moving --> can press left or right arrow to move character
        if(!isMoving)
        {
            timerForFall += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }
            ChangeAnim(AnimatorPlayer.idle);
        }
        
    }
    void FixedUpdate()
    {
        // block when player lose or on setting
        if(isLose)
        {
            transform.position += Vector3.down * Time.fixedDeltaTime * FALL_SPEED;
            return;
        }
        if(GameplayManager.instance.onSetting)
        {
            return;
        }

        // move character
        Move();

        // check if player collide
        CheckCollide();
        
        // check if player not in ground
        CheckOutGround();

        // every 50 points --> increase difficulty by decrease time for fall
        if(score >= NUMBERS_OF_STEP_TO_INCREASE_DIFFICULTY)
        {
            difficultyTime = Mathf.Clamp((int)(score / NUMBERS_OF_STEP_TO_INCREASE_DIFFICULTY) 
                * TIME_INCREASE_BY_DIFFICULTY, MIN_TIME_DIFFICULTY, MAX_TIME_DIFFICULTY);
        }
        // check if the standing time exceeds the allowed time --> lose
        if(timerForFall > (TIME_TO_FALL - difficultyTime))
        {
            Lose();
        }
        // check if time move > time combo --> add score to player and reset combo
        if(timerForFall > TIME_COMBO)
        {
            if(combo > 10)
            {
                score += combo;
            }
            combo = 0;
        }
        // timer for increase point --> increase point when player in time
        timerIncreasePoint -= Time.fixedDeltaTime;
        if(timerIncreasePoint > 0f)
        {
            increasePoint = INCREASE_POINT;
        }
        else
        {
            increasePoint = 1;
        }
    }
    /// <summary>
    /// Move character between start point and end point by using 2 vector3.lerp
    /// to create player jump
    /// </summary>
    private void Move()
    {
        // create control point
        Vector3 middle_point = start + (target - start)/2 + Vector3.up * 1.5f;
        // move player by 2 vector3.lerp
        if (interpolaForJump < 1.0f) {
            isMoving = true;

            interpolaForJump += speed * Time.fixedDeltaTime;

            Vector3 m1 = Vector3.Lerp(start, middle_point, interpolaForJump);
            Vector3 m2 = Vector3.Lerp(middle_point, target, interpolaForJump);
            transform.position = Vector3.Lerp(m1, m2, interpolaForJump);
        }
        else
        {
            isMoving = false;
        }
    }
    /// <summary>
    /// Move character to left when tap left button in screen
    /// </summary>
    public void MoveLeft()
    {
        // check not moving --> can tap
        if(!isMoving)
        {
            // set start and end point
            target = transform.position + Vector3.left;
            start = transform.position;
            interpolaForJump = 0f;

            // spawn ground front
            GameplayManager.instance.SpawnGround();

            // reset time for fall
            timerForFall = 0f;

            score += 1 * increasePoint;
            ChangeAnim(AnimatorPlayer.move);
            Invoke(nameof(ResetAnim), .3f);

            // rotate character
            transform.eulerAngles = new Vector3(0f, -90f, 0f);

            // check if time tap < time combo --> increase combo
            if(timerForFall < TIME_COMBO)
            {
                combo += 1;
            }
        }
    }
    /// <summary>
    /// Move character to right when tap left button in screen
    /// </summary>
    public void MoveRight()
    {
        // check not moving --> can tap
        if(!isMoving)
        {
            // set start and end point
            target = transform.position + Vector3.forward;
            start = transform.position;
            interpolaForJump = 0f;

            // spawn ground front
            GameplayManager.instance.SpawnGround();

            // reset time for fall
            timerForFall = 0f;

            score += 1 * increasePoint;
            ChangeAnim(AnimatorPlayer.move);
            Invoke(nameof(ResetAnim), .3f);

            // rotate character
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            
            // check if time tap < time combo --> increase combo
            if(timerForFall < TIME_COMBO)
            {
                combo += 1;
            }
        }
    }
    /// <summary>
    /// Use raycast from character to down --> to check if have ground below character or not --> if not --> lose
    /// </summary>
    private void CheckOutGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 3f))
        {

        }
        else
        {
            Lose();
        }
    }
    /// <summary>
    /// User overlapsphere to check if player hit diamond or water
    /// </summary>
    private void CheckCollide()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * 0.5f, RANGE_DETECT);
        foreach(Collider c in colliders)
        {
            if(c.tag == "diamond")
            {
                // set time increase point and return diamond to pool
                DiamondPooler.instance.BackToPool(c.gameObject);
                timerIncreasePoint = TIMER_INCREASE_POINT;
            }
            if(c.tag == "water")
            {
                Lose();
            }
        }
    }
    private void ChangeAnim(AnimatorPlayer newAnim)
    {
        // if next anim  >< current anim --> set animation to idle --> set new animation
        if(currentAnim != (int)newAnim)
        {
            ResetAnim();
            anim.SetInteger("move", (int)newAnim);
        }
    }
    private void ResetAnim()
    {
        anim.SetInteger("move", (int)AnimatorPlayer.idle);
    }
    /// <summary>
    /// If player lose --> add score before go to lose screen
    /// play animation fall
    /// </summary>
    private void Lose()
    {
        if(combo >= 10)
        {
            score += combo;
        }
        ChangeAnim(AnimatorPlayer.fall);
        isLose = true;
        GameplayManager.instance.OnLose();
    }
    public float GetMaxTime()
    {
        return TIME_TO_FALL;
    }
    public float GetTimeIncrease()
    {
        return TIMER_INCREASE_POINT;
    }
}
