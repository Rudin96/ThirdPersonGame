using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Transform _playerPos;
    private float _camAngleX;
    private float _camAngleY;

    public Camera Cam;

    [Header("Movement Properties")]
    public float MovementSpeed = 20.0f;
    public float JumpStrength = 200f;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandlePlayerMovement();
        SetupCameraPositions();
    }

    void SetupCameraPositions()
    {
        _playerPos = transform;
        _camAngleX = Input.GetAxis("Mouse Y");
        _camAngleY = Input.GetAxis("Mouse X");
        if(Cam != null)
        {
            Cam.transform.position = _playerPos.position;
            Cam.transform.rotation = _playerPos.rotation;
        }
    }

    void HandlePlayerMovement()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            _body.AddForce(transform.forward * MovementSpeed * Input.GetAxis("Vertical"), ForceMode.Acceleration);

            _body.AddForce(transform.right * MovementSpeed * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f));
        }

        if (Input.GetButtonDown("Jump"))
        {
            _body.AddForce(new Vector3(0f, JumpStrength * 1000f, 0f), ForceMode.Force);
        }
    }
}
