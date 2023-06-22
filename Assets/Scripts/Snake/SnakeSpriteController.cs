using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpriteController : MonoBehaviour
{
    [SerializeField] private Sprite head, body, tail, turnRight, turnLeft;

    private SpriteRenderer _spriteRenderer;
    private Transform _trans;

    public void SetSprite(SegmentType type)
    {
        switch (type)
        {
            case (SegmentType.Head): _spriteRenderer.sprite = head; return;
            case (SegmentType.Body): _spriteRenderer.sprite = body; return;
            case (SegmentType.Tail): _spriteRenderer.sprite = tail; return;
            case (SegmentType.TurnRight): _spriteRenderer.sprite = turnRight; return;
            case (SegmentType.TurnLeft): _spriteRenderer.sprite = turnLeft; return;
            case (SegmentType.None): Debug.Log("attempted to set no sprite, doing nothing"); return;
        }
    }

    public void ResetSpriteTransform()
    {
        _trans.localPosition = Vector3.zero;
        _trans.localRotation = Quaternion.identity;   
    }

    // translate sprite to a given position relative to the parent segment
    public void TranslateSprite(Vector2 pos)
    {
        _trans.position = new Vector3(pos.x, pos.y, 0.0f);
    }

    // rotate a sprite to a given angle relative to the set angle
    public void RotateSprite(float angle)
    {
        _trans.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trans = GetComponent<Transform>();
    }
}
