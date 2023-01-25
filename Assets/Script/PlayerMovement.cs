using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 16f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GameObject end;

    private bool canShine = true;
    private bool isShine = false;
    private float shineCooldown = 7f;

    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        // 
        pos = gameObject.transform.position;
    }

    private bool isGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(isShine) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if(Input.GetButtonDown("Jump") && isGrounded()){
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if(canShine && Input.GetKeyDown(KeyCode.LeftShift)){
            StartCoroutine(Shine());
        }
    }

    private IEnumerator Shine(){
        canShine = false;
        isShine = true;
        transform.gameObject.AddComponent<LightGenerator>();
        float gravity = rb.gravityScale;
        Vector2 velocity = rb.velocity;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0,0);
        yield return new WaitForSeconds(shineCooldown);
        yield return new WaitForSeconds(1f);
        isShine = false;
        canShine = true;
        rb.gravityScale = gravity;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D c) {
        if(c.gameObject.name == "door"){
            end.SetActive(true);
        }

        if(c.gameObject.tag == "spike"){
            gameObject.transform.position = pos;
        }
    }
}
