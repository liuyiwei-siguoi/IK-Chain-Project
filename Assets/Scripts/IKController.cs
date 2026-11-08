
using UnityEngine;

public class IKController : MonoBehaviour
{
    [Header("IK算法")]
    public FABRIKSolver ikSolver;

    [Header("摄像机")]
    public Camera mainCamera;

    [Header("拖拽参数")]
    public float pickRadius = 0.3f;

    private bool isDragging = false;

    void Start()
    {
        if (ikSolver == null)
            ikSolver = GetComponent<FABRIKSolver>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (ikSolver == null || mainCamera == null)
            return;

        // 获取鼠标对应的世界坐标
        Vector3 mousePosition =
            Input.mousePosition;

        mousePosition.z =
            -mainCamera.transform.position.z;

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 mouseWorld =
            new Vector2(worldPosition.x, worldPosition.y);

        // 获取末端关节
        int lastIndex =
            ikSolver.chainManager.joints.Length - 1;

        Transform endJoint =
            ikSolver.chainManager.joints[lastIndex];

        // 鼠标按下时，检查是否点击末端关节
        if (Input.GetMouseButtonDown(0))
        {
            float distance =
                Vector2.Distance(mouseWorld, endJoint.position);

            if (distance <= pickRadius)
            {
                isDragging = true;
            }
        }

        // 拖动过程中更新目标位置
        if (isDragging && Input.GetMouseButton(0))
        {
            ikSolver.targetPosition = mouseWorld;
        }

        // 松开鼠标，结束拖拽
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
