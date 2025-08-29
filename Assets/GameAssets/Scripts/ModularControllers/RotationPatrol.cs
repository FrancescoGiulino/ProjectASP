using UnityEngine;

public class RotationPatrol : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationAngle = 45f;

    private float startY;
    private bool rotatingRight = true;
    private bool active = true;
    private float angle;

    void Start()
    {
        startY = transform.eulerAngles.y;
    }

    void Update() { PatrolRotation(); }

    private void PatrolRotation()
    {
        if (!active) return;
        angle = Mathf.DeltaAngle(startY, transform.eulerAngles.y);

        if (rotatingRight)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            if (angle >= rotationAngle) rotatingRight = false;
        }
        else
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            if (angle <= -rotationAngle) rotatingRight = true;
        }
    }

    public void AimAt(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, GetRotationSpeed() * Time.deltaTime);
    }

    public float GetRotationSpeed() => rotationSpeed;
    public void SetActive(bool _active) => active = _active;
}