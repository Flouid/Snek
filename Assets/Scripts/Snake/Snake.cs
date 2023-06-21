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
    private int _snakeLength;
    private float _clock, _snakeSpeed;

    public void Init(GridPosition startPos, int startLength, float snakeSpeed)
    {
        // assert that the snake can have at least a head and a tail
        Assert.IsTrue(startLength > 1);
        _head = CreateSegment(null, null, startPos, _snakeLength);
        _tail = _head;
        _snakeLength = 1;
        
        // grow potentially several times depending on start config
        for (int segmentsLeft = startLength - 1; segmentsLeft > 0; --segmentsLeft) Grow();

        _snakeSpeed = snakeSpeed;
    }

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
        float timeToMoveStep = 1 / _snakeSpeed;

        if (_clock >= timeToMoveStep)
        {
            MoveStep();
        }
    }

    void MoveStep()
    {
        Debug.Log("executing move step");

        _clock = 0.0f;
        _head.pos.dir = _inputController.queuedInput;
        Direction currDir, nextDir;

        // walk down the snake and move each segment in the direction they are facing
        for (SnakeSegment curr = _head; curr != null; curr = curr.next)
        {
            currDir = curr.pos.dir;
        }
    }

    void Grow()
    {
        // spawn the new segment in the correct location
        SnakeSegment newSegment = CreateSegment(_tail, null, _tail.pos.Behind(), _snakeLength);
        // adjust the references to neighboring segments
        _tail.prev = newSegment;
        newSegment.next = _tail;
        _tail = newSegment;
        // track change to length
        ++_snakeLength;
    }

    SnakeSegment CreateSegment(SnakeSegment next, SnakeSegment prev, GridPosition startPos, int index)
    {
        // instantiate a new segment
        var newSegmentObject = Instantiate(snakeSegmentPrefab);
        // name it according to the length of the snake at creation time
        newSegmentObject.name = $"Segment {index}";
        // get the controller for the new segment, initialize it and return
        SnakeSegment newSegment = newSegmentObject.GetComponent<SnakeSegment>();
        newSegment.Init(null, null, startPos, index);
        return newSegment;
    }

}
