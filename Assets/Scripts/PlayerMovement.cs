using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    // public float rotateSpeed = 180f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        transform.LookAt(new Vector3(playerInput.rotate.x, transform.position.y, playerInput.rotate.z));
    }

    private void Move()
    {
        var pos = playerRigidbody.position;
        pos += playerInput.move * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(pos);
    }
}