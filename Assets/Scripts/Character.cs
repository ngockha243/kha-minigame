using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : FastSingleton<Character>
{
    [SerializeField] private Animator anim;
    [SerializeField] private float speed;
    public int score;
    

    public const float TIME_TO_FALL = 2f;
    private const int NUMBERS_OF_STEP_TO_INCREASE_DIFFICULTY = 50;
    private const float TIME_INCREASE_BY_DIFFICULTY = .1f;
    private const float MIN_TIME_DIFFICULTY = .1f;
    private const float MAX_TIME_DIFFICULTY = .5f;
    private const float RANGE_DETECT = .3f;
    private const float TIME_COMBO = .5f;
    private const int INCREASE_POINT = 2;
    private const float TIMER_INCREASE_POINT = 5f;

    
    public int combo;
    public float difficultyTime;
    public float timerForFall;
    public int increasePoint;
    private float timerIncreasePoint;
    private float interpolaForJump;
    private Vector3 target;
    private Vector3 start;
    private bool isMoving;
    private bool isLose;
    private int currentAnim;
    public float GetMaxTime()
    {
        return TIME_TO_FALL;
    }
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }
    private void OnInit()
    {
        increasePoint = 1;
        timerIncreasePoint = 0;
        combo = 0;
        start = transform.position;
        target = transform.position;
        interpolaForJump = 0f;
        timerForFall = 0f;
        isLose = false;
        score = 0;
        difficultyTime = 0f;
    }
    private void Update() {
        if(isLose)
        {
            return;
        }
        
        if(GameplayManager.instance.onSetting)
        {
            return;
        }
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
    public void MoveLeft()
    {
        if(!isMoving)
        {
            target = transform.position + Vector3.left;
            start = transform.position;
            interpolaForJump = 0f;
            GameplayManager.instance.SpawnGround();
            timerForFall = 0f;
            score += 1 * increasePoint;
            ChangeAnim(AnimatorPlayer.move);
            Invoke(nameof(ResetAnim), .3f);
            transform.eulerAngles = new Vector3(0f, -90f, 0f);

            if(timerForFall < TIME_COMBO)
            {
                combo += 1;
            }
        }
    }
    public void MoveRight()
    {
        if(!isMoving)
        {
            target = transform.position + Vector3.forward;
            start = transform.position;
            interpolaForJump = 0f;
            GameplayManager.instance.SpawnGround();
            timerForFall = 0f;
            score += 1 * increasePoint;
            ChangeAnim(AnimatorPlayer.move);
            Invoke(nameof(ResetAnim), .3f);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            
            if(timerForFall < TIME_COMBO)
            {
                combo += 1;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(isLose)
        {
            return;
        }
        if(GameplayManager.instance.onSetting)
        {
            return;
        }
        Move();

        CheckCollide();
        
        CheckOutGround();

        if(score >= NUMBERS_OF_STEP_TO_INCREASE_DIFFICULTY)
        {
            difficultyTime = Mathf.Clamp((int)(score / NUMBERS_OF_STEP_TO_INCREASE_DIFFICULTY) 
                * TIME_INCREASE_BY_DIFFICULTY, MIN_TIME_DIFFICULTY, MAX_TIME_DIFFICULTY);
        }
        if(timerForFall > (TIME_TO_FALL - difficultyTime))
        {
            Lose();
        }
        if(timerForFall > TIME_COMBO)
        {
            if(combo > 10)
            {
                score += combo;
            }
            combo = 0;
        }
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
    private void Move()
    {
        Vector3 middle_point = start + (target - start)/2 + Vector3.up * 1.5f;
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
    private void CheckCollide()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * 0.5f, RANGE_DETECT);
        foreach(Collider c in colliders)
        {
            if(c.tag == "diamond")
            {
                DiamondPooler.instance.BackToPool(c.gameObject);
                timerIncreasePoint = TIMER_INCREASE_POINT;
            }
            if(c.tag == "water")
            {
                Lose();
            }
        }
        // if(colliders.Length == 0)
        // {
        //     Lose();
        // }
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
    private void Lose()
    {
        isLose = true;
        GameplayManager.instance.OnLose();
    }
}
