using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Camera _cam;
    private Transform _rotator;

    private float _cameraLength;

    private Transform _adsPoint;

    public float MovementSpeed = 5f;
    public float JumpStrength = 200f;

    public float AdsSpeed = 10f;
    public float CameraBoomLength = 10f;
    public float CamMaxRotDegrees = 90f;
    public Vector3 BoomAddVector;
    public Vector3 BoomAddRotation;
    private Vector3 _camDirRaw;
    private Vector3 _camDirNormalized;
    private Vector3 _adsPos;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();

        _cam = GetComponentInChildren<Camera>();

        _cameraLength = (_cam.transform.position - transform.position).magnitude;

        _cam.transform.position = transform.position + ((transform.forward * -1) * CameraBoomLength);

        //_body.drag = 15f;
        //_body.mass = 10f;

        Debug.Log(-CamMaxRotDegrees);

        _rotator = transform.Find("CameraRotator");
    }

    // Update is called once per frame
    void Update()
    {
        SetupCameraPositions();
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            _body.AddForce(transform.forward * MovementSpeed * Input.GetAxis("Vertical"), ForceMode.Acceleration);

            _body.AddForce(transform.right * MovementSpeed * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }

        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _body.transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f));

            if(_rotator.transform.eulerAngles.x < CamMaxRotDegrees && (360f - _rotator.transform.eulerAngles.x) < CamMaxRotDegrees)
            _rotator.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0f, 0f));
        }

        if(Input.GetButtonDown("Jump"))
        {
            _body.AddForce(new Vector3(0f, JumpStrength * 1000f, 0f), ForceMode.Force);
        }

        //CheckCameraCollision();

        if(Input.GetButtonDown("Fire2"))
        {
            StartADS();
        }

        if(Input.GetButtonUp("Fire2"))
        {
            StopADS();
        }
    }

    private void StartADS()
    {
        _adsPos = transform.position + (_camDirNormalized * (_camDirRaw.magnitude / 2));

        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _adsPos, AdsSpeed);
    }

    private void StopADS()
    {
        _adsPos = transform.position + (_camDirNormalized * (_camDirRaw.magnitude * 2));

        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _adsPos, AdsSpeed);
    }

    private void SetupCameraPositions()
    {
        _camDirRaw = (_cam.transform.position - transform.position);
        _camDirNormalized = _camDirRaw.normalized;
    }

    private void CheckCameraCollision()
    {
        RaycastHit hitInfo;
        Vector3 _endDir = _camDirNormalized * _camDirRaw.magnitude;

        if (Physics.Raycast(transform.position, _endDir, out hitInfo, _camDirRaw.magnitude, 3))
        {
            Debug.DrawLine(hitInfo.point, hitInfo.point + (hitInfo.normal * 2), Color.blue);
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, Vector3.RotateTowards(hitInfo.point + (hitInfo.normal * 2), transform.position, 0f, 0f), AdsSpeed);
        } else if(_cam.transform.position != transform.position + (_camDirNormalized * _cameraLength))
        {
            Debug.DrawLine(transform.position, transform.position + (_camDirNormalized * _cameraLength), Color.green);
            Vector3 _rotDir = Vector3.RotateTowards(hitInfo.point + (hitInfo.normal * 2), transform.position, 0f, 0f);
            _cam.transform.Rotate(_rotDir);
        }

        //_rotator.transform.Rotate(BoomAddRotation);
    }
}
