﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class DaughterActions : MonoBehaviour {

    public Vector3 jump;
    public float jumpForce = 2.0f;
    public float gravity = 10.0f;
    public bool isGrounded;
    public float speed = 200.0f;

    Rigidbody2D rb;

    private Player player;
    private Vector3 moveDirection = Vector3.zero;


    private void Awake()
    {
        player = ReInput.players.GetPlayer(0); // get the player by id
    }
    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        jump = new Vector3(0.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update() {
        PlayerMovement();
        UsePowers();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void PlayerMovement() {
        if (player.GetAxis("Move Horizontal") != 0.0f)
        {
            if (player.GetAxis("Move Horizontal") < 0.0f)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;

            } else if (player.GetAxis("Move Horizontal") > 0.0f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }

        if (player.GetButtonDown("Jump"))
        {
            if (isGrounded == true)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isGrounded = false;
                Debug.Log("Jump!");
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
    }
        
    private void UsePowers()
    {
    if (player.GetButtonDown("Powers"))
        {
        Debug.Log("Powers!");
        }
    }
}
