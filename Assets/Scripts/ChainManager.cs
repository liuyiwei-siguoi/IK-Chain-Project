
using UnityEngine;

public class ChainManager : MonoBehaviour
{
    [Header("骨骼链关节")]
    public Transform[] joints;

    [Header("骨骼连线")]
    private LineRenderer lineRenderer;

    void Start()
    {
        // 获取当前物体上的 LineRenderer
        lineRenderer = GetComponent<LineRenderer>();

        // 检查是否存在 LineRenderer
        if (lineRenderer == null)
        {
            Debug.LogError("没有找到 LineRenderer！");
            return;
        }

        // 设置连线数量
        if (joints == null || joints.Length < 2)
        {
            Debug.LogError("关节数量不足！");
            return;
        }

        lineRenderer.positionCount = joints.Length;

        // 初始化连线
        UpdateChain();
    }

    void Update()
    {
        UpdateChain();
    }

    // 更新骨骼连线
    public void UpdateChain()
    {
        if (lineRenderer == null || joints == null)
            return;

        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i] != null)
            {
                lineRenderer.SetPosition(
                    i,
                    joints[i].position
                );
            }
        }
    }
}
