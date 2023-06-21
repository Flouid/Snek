using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpriteController : MonoBehaviour
{
    [SerializeField] private Sprite head, body, tail, turn;

    private float _angle;
    private SpriteRenderer _spriteRenderer;
    private Transform _trans;

    public void SetSprite(SegmentType type)
    {
        switch (type)
        {
            case (SegmentType.Head): _spriteRenderer.sprite = head; return;
            case (SegmentType.Body): _spriteRenderer.sprite = body; return;
            case (SegmentType.Tail): _spriteRenderer.sprite = tail; return;
            case (SegmentType.Turn): _spriteRenderer.sprite = turn; return;
            case (SegmentType.None): Debug.Log("attempted to set no sprite, doing nothing"); return;
        }
    }

    public void ResetSpritePos() { _trans.localPosition = Vector3.zero; }

    // translate sprite to a given position relative to the parent segment
    public void TranslateSprite(Vector2 pos)
    {
        _trans.localPosition = new Vector3(pos.x, pos.y, 0.0f);
    }

    // snap a sprite to the next rotation
    public void SnapRotateSprite(float angle)
    {
        float angleSum = _angle + angle;
        _angle = angleSum > 0 ? (angleSum % 360) : (angleSum + 360) % 360;
        _trans.localRotation = Quaternion.Euler(0, 0, _angle);
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trans = GetComponent<Transform>();
        _angle = 0.0f;
    }
}
