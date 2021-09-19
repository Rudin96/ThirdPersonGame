using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float rotationXAxis = 0.0f;
    private float rotationYAxis = 0.0f;
    private float velocityX = 0.0f;
    private float velocityY = 0.0f;

    public Transform target;
    public float xSpeed = 20.0f;
    public float ySpeed = 20.0f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float distance = 10.0f;
    public float distanceMin = 10f;
    public float distanceMax = 10f;
    public Vector3 CameraRotOffset;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationXAxis = angles.x;
        rotationYAxis = angles.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(0))
            {
                SetRotationVelocity();
                if (Input.GetMouseButton(1))
                    RotateTarget();
            } else if(Input.GetMouseButton(1))
            {
                SetRotationVelocity();
                RotateTarget();
            } else
            {
                velocityX = 0;
                velocityY = 0;
            }

            rotationYAxis += velocityX;
            rotationXAxis += velocityY;
            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            
            Vector3 negDistance = new Vector3(0f, 0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation * Quaternion.Euler(CameraRotOffset);
            transform.position = position;

            RaycastHit _hitInfo;
            if (Physics.Linecast(target.position, transform.position, out _hitInfo))
                transform.position = _hitInfo.point;
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private void SetRotationVelocity()
    {
        velocityX = xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
        velocityY = ySpeed * Input.GetAxis("Mouse Y") * distance * 0.02f;
    }

    private void RotateTarget()
    {
        target.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
