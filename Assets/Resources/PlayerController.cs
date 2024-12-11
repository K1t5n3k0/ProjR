using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 1.���������ƶ������Ƚ�����
 * 2.������������û�й���
 * 3.���л��������ƶ����� ������һЩ���������ֵ�ܵ�
 * 4.��ԾĿǰ�Ქ3�ζ��������ⲻ����д�ο��ƣ�������ڲ���
 */
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;  //���Rigidbody
    private Collider2D playerFeet;
    private Animator anim;

    public string keyRight = "d";
    public string keyLeft = "l";
    public string keyJump = "space";
    public string keyRun = "left shift";

    private bool moveEnabled = true;

    private float speed = 0.7f;
    private float runningSpeed = 3.0f;
    public float jumpForce = 4.0f;
    public Transform groundCheck;//����
    public LayerMask ground;//����

    public bool isGound, isJump;//�ж��Ƿ��ڵ��桢�Ƿ��Ѿ���Ծ��
    private bool isIdle;

    bool jumpPressed;
    int jumpCount;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFeet = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(keyJump) && jumpCount > 0)
        {
            jumpPressed = true;
        }

        GroundMovement();
        Jump();
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        //isGound = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);//��������Ƿ��ڵ���
        isGound = playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));

        
    }

    void GroundMovement()//�ƶ�����
    {
        if (moveEnabled)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            
            if (Input.GetKey(keyRight) || Input.GetKey(keyLeft))
            {
                isIdle = false;
            }
            else
            {
                isIdle = true;
            }

            float targetSpeed = Input.GetKey(keyRun) ? runningSpeed : speed;
            
            float moveForce = horizontalMove * targetSpeed;
            rb.velocity = new Vector2(moveForce, rb.velocity.y);

            if(horizontalMove != 0)
            {
                transform.localScale = new Vector3(horizontalMove, 1, 1);
            }
        }
    }

    void Jump()
    {
        if(isGound)
        {
            moveEnabled = true;
            jumpCount = 2;
            isJump = false;
        }

        if (jumpPressed)
        {
            moveEnabled = false;
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && isJump)//������
        {
            moveEnabled = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));//ȡ����ֵ��ֻ�����ٶȴ�С
        anim.SetBool("isIdle", isIdle);
        //print(rb.velocity.x);

        if(isGound) 
        {
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
        }
        else if (!isGound && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (!isGound && rb.velocity.y < 0 )
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }
}