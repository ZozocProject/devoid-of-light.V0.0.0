using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; //������ �� ������ (������ �������� ������), �� ������� ������ ������.
    public float height = 2.0f; // �� � ��� ������� (������ ������ ��� �����(��� ������)).
    public float maxRotationSpeed = 0.7f; //�� � ��� ������� (������������ �������� �������� ������(��� ������)).
    public float fixedDistance = 5.0f; //��� (������������� ���������� ����� ������� � �����(��� ������)).
    public LayerMask playerLayer;

    private float mouseX, mouseY;
    private float currentRotation = 0.0f;

    void LateUpdate()
    {
        if (target == null)
            return;

        mouseX += Input.GetAxis("Mouse X") * maxRotationSpeed; //����������, � ������� ����������� ���� �� ���� ��� ���������� ��������� ������.
        mouseY -= Input.GetAxis("Mouse Y") * maxRotationSpeed; //� ��� ����.
        mouseY = Mathf.Clamp(mouseY, -65, 54); // ������������ ������ ������(����� ������ ����� �� �����������).

        mouseX = mouseX % 360; //� �� � ������ ��� ����������� �������� � �� ��� ������ � ����� � ������ ��� ������� � ����� ��� �� ����������� ��� ������ ��������...

        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, height, -fixedDistance);

        RaycastHit hit;
        int layerMask = LayerMask.GetMask("playerLayer");
        if (Physics.Linecast(target.position, desiredPosition, out hit, ~layerMask))
            {
            transform.position = hit.point;
            }
        else
        {
            transform.position = desiredPosition;
        }

        float targetRotation = Mathf.Atan2(transform.position.y - target.position.y, transform.position.x - target.position.x) * Mathf.Rad2Deg;

        currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * maxRotationSpeed);

        transform.rotation = Quaternion.Euler(mouseY, mouseX, currentRotation);

        transform.LookAt(target);
    }
}



