using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; //Ссылка на объект (обычно персонаж игрока), за которым следит камера.
    public float height = 2.0f; // Ну и так понятно (высота камеры над целью(для еблана)).
    public float maxRotationSpeed = 0.7f; //Ну и так понятно (Максимальная скорость вращения камеры(для еблана)).
    public float fixedDistance = 5.0f; //Угу (Фиксированное расстояние между камерой и целью(для еблана)).
    public LayerMask playerLayer;

    private float mouseX, mouseY;
    private float currentRotation = 0.0f;

    void LateUpdate()
    {
        if (target == null)
            return;

        mouseX += Input.GetAxis("Mouse X") * maxRotationSpeed; //Переменные, в которых сохраняется ввод от мыши для управления вращением камеры.
        mouseY -= Input.GetAxis("Mouse Y") * maxRotationSpeed; //И это тоже.
        mouseY = Mathf.Clamp(mouseY, -65, 54); // Максимальный поврот камеры(Чтобы камера нахуй не кувыркалась).

        mouseX = mouseX % 360; //Я хз н овроде это ограничение градусов я не ебу почему и зачем и нахуйя это добавил и вроде это не огранечение это просто проверка...

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



