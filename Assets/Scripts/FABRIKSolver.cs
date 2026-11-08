
using UnityEngine;

public class FABRIKSolver : MonoBehaviour
{
    [Header("骨骼链管理器")]
    public ChainManager chainManager;

    [Header("IK目标位置")]
    public Vector2 targetPosition = new Vector2(2f, 2f);

    [Header("算法参数")]
    public int maxIterations = 10;
    public float tolerance = 0.01f;

    private float[] lengths;
    private Vector2[] positions;
    private float totalLength;
    private bool initialized;

    void Start()
    {
        if (chainManager == null)
            chainManager = GetComponent<ChainManager>();

        if (chainManager == null || chainManager.joints == null
            || chainManager.joints.Length < 2)
        {
            Debug.LogError("骨骼链配置错误！");
            return;
        }

        int count = chainManager.joints.Length;

        lengths = new float[count - 1];
        positions = new Vector2[count];

        totalLength = 0f;

        // 记录每段骨骼的长度
        for (int i = 0; i < count - 1; i++)
        {
            Vector2 a = chainManager.joints[i].position;
            Vector2 b = chainManager.joints[i + 1].position;

            lengths[i] = Vector2.Distance(a, b);
            totalLength += lengths[i];
        }

        initialized = true;
        SolveIK();
    }

    void Update()
    {
        if (initialized)
            SolveIK();
    }

    public void SolveIK()
    {
        int count = chainManager.joints.Length;

        Vector2 rootPosition =
            chainManager.joints[0].position;

        // 获取当前所有关节的位置
        for (int i = 0; i < count; i++)
        {
            positions[i] = chainManager.joints[i].position;
        }

        // 判断目标是否超出骨骼链的最大长度
        if (Vector2.Distance(rootPosition, targetPosition)
            >= totalLength)
        {
            Vector2 direction =
                (targetPosition - rootPosition).normalized;

            for (int i = 1; i < count; i++)
            {
                positions[i] =
                    positions[i - 1] + direction * lengths[i - 1];
            }
        }
        else
        {
            // FABRIK 反向与正向迭代
            for (int iteration = 0;
                 iteration < maxIterations;
                 iteration++)
            {
                // Backward Pass：反向调整
                positions[count - 1] = targetPosition;

                for (int i = count - 2; i >= 0; i--)
                {
                    Vector2 direction =
                        (positions[i] - positions[i + 1]).normalized;

                    positions[i] =
                        positions[i + 1] + direction * lengths[i];
                }

                // Forward Pass：正向调整
                positions[0] = rootPosition;

                for (int i = 1; i < count; i++)
                {
                    Vector2 direction =
                        (positions[i] - positions[i - 1]).normalized;

                    positions[i] =
                        positions[i - 1] + direction * lengths[i - 1];
                }

                // 判断是否达到精度要求
                if (Vector2.Distance(
                    positions[count - 1], targetPosition) < tolerance)
                {
                    break;
                }
            }
        }

        // 更新关节位置
        for (int i = 0; i < count; i++)
        {
            Vector3 oldPosition =
                chainManager.joints[i].position;

            chainManager.joints[i].position =
                new Vector3(
                    positions[i].x,
                    positions[i].y,
                    oldPosition.z
                );
        }

        // 更新骨骼连线
        chainManager.UpdateChain();
    }
}
