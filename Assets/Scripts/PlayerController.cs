using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Transform _playerPos;
    public Camera Cam;

    [Header("Movement Properties")]
    public float MovementSpeed = 20.0f;
    public float JumpStrength = 200f;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        HandlePlayerMovement();
    }

    void HandlePlayerMovement()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            _body.AddForce(transform.forward * MovementSpeed * Input.GetAxis("Vertical"), ForceMode.Acceleration);

            _body.AddForce(transform.right * MovementSpeed * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }

        if(Input.GetMouseButton(0) && Input.GetMouseButton(1))
            _body.AddForce(transform.forward * MovementSpeed, ForceMode.Acceleration);

        if (Input.GetButtonDown("Jump"))
        {
            _body.AddForce(new Vector3(0f, JumpStrength * 1000f, 0f), ForceMode.Force);
        }
    }
}
