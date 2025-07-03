using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Animator animator;

    public Vector2 moveDirection = Vector2.right;
    private Vector2 moveVelocity;

    public AppleController script_applecontroller;

    public List<Vector3> positionHistory = new List<Vector3>();
    public List<Transform> bodyParts = new List<Transform>();
    private float distanceBetween = 0.4f;

    public GameObject bodyPrefab;

    //点数を表示
    public int score = 0;
    public TextMeshProUGUI scoreText;

    //食い物をもらったら一定時間に無敵(食い物をもらうと即死を解決するため)
    private float invincibletime = 0;
    private bool invincibleact = false;

    private bool is_gameover = false;

    public AudioSource catchsound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!is_gameover)
        {
            Move();
            StorePosition();
            MoveBody();
            UpdateAnimation();
            CheckOutOfBounds();
            Invincible();
            UpdateScoreUI();
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2.down)
            moveDirection = Vector2.up;
        if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2.up)
            moveDirection = Vector2.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2.right)
            moveDirection = Vector2.left;
        if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2.left)
            moveDirection = Vector2.right;

        moveVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = moveVelocity;
    }

    void UpdateAnimation()
    {
        if (moveDirection == Vector2.right) animator.Play("Walk_right");
        else if (moveDirection == Vector2.left) animator.Play("Walk_left");
        else if (moveDirection == Vector2.up) animator.Play("Walk_up");
        else if (moveDirection == Vector2.down) animator.Play("Walk_down");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            script_applecontroller.Apple_Initialize();
            moveSpeed += 0.1f;
            Grow();
            score += 10;
            invincibleact = true;
            catchsound.Play();
        }
        if (other.CompareTag("Body") && !invincibleact) 
        {
            GameOver();
        }
    }

    void StorePosition()
    {
        // 移動したときだけ記録
        if (positionHistory.Count == 0 || Vector3.Distance(transform.position, positionHistory[0]) > 0.05f)
        {
            positionHistory.Insert(0, transform.position);
        }

        // Body 全体が通過した位置まで履歴を保持
        float maxNeededDistance = distanceBetween * (bodyParts.Count + 1);
        float totalDistance = 0f;

        for (int i = 0; i < positionHistory.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(positionHistory[i], positionHistory[i + 1]);
            if (totalDistance > maxNeededDistance)
            {
                // 不要な位置履歴を削除
                positionHistory.RemoveRange(i + 1, positionHistory.Count - (i + 1));
                break;
            }
        }
    }

    void MoveBody()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            float targetDistance = distanceBetween * (i + 1);
            float distanceSoFar = 0f;

            for (int j = 0; j < positionHistory.Count - 1; j++)
            {
                float segmentDistance = Vector3.Distance(positionHistory[j], positionHistory[j + 1]);
                distanceSoFar += segmentDistance;

                if (distanceSoFar >= targetDistance)
                {
                    Vector3 segmentDirection = -(positionHistory[j + 1] - positionHistory[j]).normalized;
                    bodyParts[i].position = positionHistory[j + 1];

                    // 向きを更新
                    Body bodyScript = bodyParts[i].GetComponent<Body>();
                    if (bodyScript != null)
                    {
                        bodyScript.SetDirection(segmentDirection);
                    }
                    break;
                }
            }
        }
    }

    public void Grow()
    {
        GameObject newPart = Instantiate(bodyPrefab, transform.position, Quaternion.identity);
        Body bodyScript = newPart.GetComponent<Body>();
        if (bodyScript != null)
        {
            bodyScript.SetDirection(moveDirection);
        }
        bodyParts.Add(newPart.transform);
    }

    void CheckOutOfBounds()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // ビューポートの範囲外（0〜1の範囲）に出たらゲームオーバー
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        is_gameover = true;
        SceneManager.LoadScene("ResultScene");
    }
    void Invincible()
    {
        if (invincibleact)
        {
            invincibletime += Time.deltaTime;
            if (invincibletime >= 0.5f)
            {
                invincibleact = false;
                invincibletime = 0;
            }
        }
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
