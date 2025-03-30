using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformGlowLineStatic : MonoBehaviour
{
    public Color glowColor = Color.green;
    public float glowWidth = 0.2f;
    public float glowYOffset = 0.05f;

    void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        float width = col.size.x;

        GameObject glowObj = new GameObject("GlowLine");
        glowObj.transform.SetParent(transform);
        glowObj.transform.localPosition = Vector3.zero;

        LineRenderer line = glowObj.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.positionCount = 2;
        line.numCapVertices = 10;

        float y = -col.size.y / 2f - glowYOffset;

        line.SetPosition(0, new Vector3(-width / 2f, y, 0));
        line.SetPosition(1, new Vector3(width / 2f, y, 0));

        line.startWidth = glowWidth;
        line.endWidth = glowWidth;

        // Создаём градиент: середина яркая, края прозрачные
        Gradient gradient = new Gradient();

        glowColor.a = 1f; // гарантируем видимость

        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(glowColor, 0f),
                new GradientColorKey(glowColor, 0.5f),
                new GradientColorKey(glowColor, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(1f, 0.5f),
                new GradientAlphaKey(0f, 1f)
            }
        );

        line.colorGradient = gradient;
    }
}