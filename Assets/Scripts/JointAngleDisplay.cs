
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class JointAngleDisplay : MonoBehaviour
{
    [Header("骨骼链管理器")]
    public ChainManager chainManager;

    [Header("角度显示文本")]
    public Text angleText;

    void LateUpdate()
    {
        if (chainManager == null ||
            chainManager.joints == null ||
            angleText == null)
        {
            return;
        }

        Transform[] joints = chainManager.joints;

        StringBuilder display = new StringBuilder();

        display.AppendLine("=== IK Joint Angles ===");
        display.AppendLine("");

        // 计算每段骨骼相对于水平轴的角度
        for (int i = 0; i < joints.Length - 1; i++)
        {
            Vector2 direction =
                joints[i + 1].position - joints[i].position;

            float angle =
                Mathf.Atan2(direction.y, direction.x)
                * Mathf.Rad2Deg;

            display.AppendLine(
                "Bone " + i + ": " +
                angle.ToString("F1") + "°"
            );
        }

        display.AppendLine("");
        display.AppendLine("=== Joint Angles ===");

        // 计算中间关节的内部夹角
        for (int i = 1; i < joints.Length - 1; i++)
        {
            Vector2 previous =
                joints[i - 1].position - joints[i].position;

            Vector2 next =
                joints[i + 1].position - joints[i].position;

            float angle = Vector2.Angle(previous, next);

            display.AppendLine(
                "Joint " + i + ": " +
                angle.ToString("F1") + "°"
            );
        }

        angleText.text = display.ToString();
    }
}
