using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{
    public Rigidbody2D bulletPrefab;

    public Transform currentGun;

    public float walkSpeed = 1f;
    public float maxVelocity = 6f;
    public float rocketVelocity = 5f;

    public int wormID;

    WormStats wormStats;
    SpriteRenderer spriteRenderer;
    Camera mainCamera;
    Vector3 difference;

    public bool IsTurn
    {
        get
        {
            return GameManager.Instance.IsMyTurn(wormID);
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wormStats = GetComponent<WormStats>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        if(!IsTurn)
        {
            return;
        }

        RotateGun();
        Movement();
    }

    void RotateGun()
    {
        Vector3 difference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        currentGun.rotation = Quaternion.Euler(0, 0, rotZ + 180f);
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if(horizontal == 0)
        {
            currentGun.gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody2D bullet = Instantiate(bulletPrefab, currentGun.position, currentGun.rotation);
                bullet.AddForce(-currentGun.right * rocketVelocity, ForceMode2D.Impulse);

                if(IsTurn)
                {
                    GameManager.Instance.NextTurn();
                }
            }
        }
        else
        {
            currentGun.gameObject.SetActive(false);
            transform.position += Vector3.right * horizontal * Time.deltaTime * walkSpeed;

            spriteRenderer.flipX = horizontal > 0f;
        }
    }
    public void OnCollisionEnter2D(Collision2D objectCollider)
    {
        if(objectCollider.transform.tag == "Bullet" && wormID != GameManager.currentWorm + 1)
        {
            wormStats.ChangeHealth(-10);
            if(IsTurn)
            {
               GameManager.Instance.NextTurn();
            }
        }
    }


}
