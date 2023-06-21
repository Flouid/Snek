using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpriteController : MonoBehaviour
{
    [SerializeField] private Sprite head, body, tail, turn;

    private float angle;
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

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trans = GetComponent<Transform>();
        angle = 0.0f;
    }

    [ContextMenu("rotate 90 degrees")]
    void Rotate()
    {
        angle += 90.0f;
        _trans.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
