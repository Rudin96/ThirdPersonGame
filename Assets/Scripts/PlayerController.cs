using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Camera _cam;
    private Transform _rotator;
    private Vector3 _baseCameraPos;
    private Vector3 _currentCameraPos;
    private float _currentBoomLength;

    [Header("Movement Properties")]
    public float MovementSpeed = 20.0f;
    public float JumpStrength = 200f;

    [Header("Camera Properties")]
    public float TargetBoomLength = 20.0f;
    public float AdsBoomLength = 10.0f;
    public float CameraFOV = 75f;
    public Vector3 BoomAddVector = new Vector3(0f, 10f, 0f);
    public Vector3 BoomAddRotation = new Vector3(20f, 0f, 0f);
    public float AdsSpeed = 10f;
    public Vector3 AdsPos;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();

        _cam = GetComponentInChildren<Camera>();

        _rotator = transform.Find("CameraRotator");

        _cam.fieldOfView = CameraFOV;

        _currentBoomLength = TargetBoomLength;
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            _body.AddForce(transform.forward * MovementSpeed * Input.GetAxis("Vertical"), ForceMode.Acceleration);

            _body.AddForce(transform.right * MovementSpeed * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }

        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _body.transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f));
            _rotator.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0f, 0f));
        }

        if(Input.GetButtonDown("Jump"))
        {
            _body.AddForce(new Vector3(0f, JumpStrength * 1000f, 0f), ForceMode.Force);
        }

        SetupCameraPositions();
        CheckCameraCollision();

        if(Input.GetAxis("Fire2") > 0)
        {
            SetCameraAds(_currentBoomLength, AdsBoomLength, AdsSpeed);
        } else
        {
            SetCameraAds(_currentBoomLength, TargetBoomLength, AdsSpeed);
        }
    }

    void SetCameraAds(float start, float end, float seconds)
    {
        _currentBoomLength = Mathf.Lerp(start, end, seconds * Time.deltaTime);
    }

    void CheckCameraCollision()
    {
        RaycastHit _hitInfo;
        if (Physics.Linecast(_rotator.transform.position, _baseCameraPos, out _hitInfo, 3))
        {
            _currentCameraPos = _hitInfo.point;
            _cam.transform.position = _currentCameraPos;
        } else
        {
            _cam.transform.position = _baseCameraPos;
        }
    }

    void SetupCameraPositions()
    {
        _baseCameraPos = _rotator.transform.position + ((_rotator.transform.forward * -1) * _currentBoomLength) + BoomAddVector;
        _cam.transform.localRotation = Quaternion.Euler(BoomAddRotation);
    }
}
