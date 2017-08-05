using UnityEngine;

public class TweenUIPosition : Tween<Vector2>
{
    [SerializeField]
    private RectTransform targetTransform;
    [SerializeField]
    private bool relative = true;

    private Vector2 OriginalPosition { get; set; }

    private void Awake()
    {
        if (targetTransform == null)
            targetTransform = GetComponent<RectTransform>();

        OriginalPosition = targetTransform.anchoredPosition;
    }

    public override Vector2 Lerp(Vector2 from, Vector2 to, float t)
    {
        return Vector2.Lerp(from, to, t);
    }

    public override void SetValue(Vector2 value)
    {
        if (relative)
        {
            value.x += OriginalPosition.x;
            value.y += OriginalPosition.y;
        }

        targetTransform.anchoredPosition = value;
    }
}