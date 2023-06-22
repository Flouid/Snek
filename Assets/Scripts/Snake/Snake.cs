using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment snakeSegmentPrefab;

    private SnakeSegment _head, _tail;
    private GameController _game;
    private PlayerInputController _inputController;
    private Vector2 _currInput;
    private int _snakeLength;
    private float _clock, _snakeSpeed;

    // ---------------- initializers ----------------------------

    public void Init(Vector2 startPos, Vector2 startDir, int startLength, float snakeSpeed)
    {
        // assert that the snake has at least a head and a tail
        Assert.IsTrue(startLength > 1);
        _head = CreateSegment(null, null, startPos, startDir, _snakeLength);
        _tail = _head;
        _snakeLength = 1;
        
        // grow potentially several times depending on start config
        for (int segmentsLeft = startLength - 1; segmentsLeft > 0; --segmentsLeft) Grow();

        _snakeSpeed = snakeSpeed;
        _currInput = Vector2.zero;
    }

    // grow the snake by adding one new segment at the end
    public void Grow()
    {
        // spawn the new segment in the correct location
        Vector2 newPos = _tail.pos - _tail.dir;
        SnakeSegment newSegment = CreateSegment(_tail, null, newPos, _tail.dir, _snakeLength);
        // adjust the references to neighboring segments
        _tail.prev = newSegment;
        newSegment.next = _tail;
        _tail = newSegment;
        // track change to length
        ++_snakeLength;
        // configuration has changed, so update all of the snake segments
        UpdateSegments();
    }

    // factory method for segments
    SnakeSegment CreateSegment(SnakeSegment next, SnakeSegment prev, Vector2 startPos, Vector2 startDir, int index)
    {
        // instantiate a new segment
        var newSegmentObject = Instantiate(snakeSegmentPrefab);
        // name it according to the length of the snake at creation time
        newSegmentObject.name = $"Segment {index}";
        // get the controller for the new segment, initialize it and return
        SnakeSegment newSegment = newSegmentObject.GetComponent<SnakeSegment>();
        newSegment.Init(null, null, startPos, startDir, index);
        return newSegment;
    }

    // ------------------------ public helpers ---------------------------

    public Vector2 CurrentInput() { return _currInput; }
    public void SampleInput() { _currInput = _inputController.queuedInput; }

    // checks if a given position is currently occupied by the snake
    public bool IsOccupied(Vector2 pos)
    {
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            if (curr.pos == pos) return true;
        }
        return false;
    }

    // -------------------------- main update methods -----------------------------

    // perform one in-world move step, rotating and translating everything by a whole unit
    void MoveStep()
    {
        // walk down the snake starting from the head, moving each segment
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            // move the segment and propagate it's direction to the next one
            _currInput = curr.Move(_currInput);
        }
        UpdateSegments();
        SampleInput();
    }

    void UpdateSegments()
    {
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            curr.UpdateSegment();
            curr.ResetSprite();
        }
    }

    void AnimateSegments(float moveProgress)
    {
        Vector2 tempDir = _currInput;
        for (SnakeSegment curr = _head; curr != null; curr = curr.prev)
        {
            tempDir = curr.Animate(tempDir, moveProgress);
        }
    }

    // --------------------- lifecycle methods --------------------------------------

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
        _clock = 0.0f;
    }

    void Update()
    {
        _clock += Time.deltaTime;
        // (0, 1] interval representing how far through the move step it currently is
        float moveProgress = Mathf.Min(1, _clock * _snakeSpeed);
        // animate the sprites along each segment according to the move progress
        AnimateSegments(moveProgress);
    }

    void FixedUpdate()
    {
        // duration between move steps, one unit at a time
        float timeToMoveStep = 1 / _snakeSpeed;
        // if the move has finished, reset the clock and perform a movestep
        if (_clock > timeToMoveStep)
        {
            _clock = 0.0f;
            MoveStep();
        }
    }

}
