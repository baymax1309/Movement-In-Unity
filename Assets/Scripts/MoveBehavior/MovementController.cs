using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class Loop
{
    public bool _activate_Loop = false;
    [Header("Change Direction")]
    [Tooltip("enable the feature to switch direction")]
    public bool _enable;
    [Tooltip("data in %. 0.5 -> when Time = 3, turn in 1.5")]
    [Range(0,10)]
    public float _time_percent = 1;
    
}


public class MovementController : MonoBehaviour {

    #region Inspector
    private GameObject gObj;

    [SerializeField]
    private GameObject gRef;

    [SerializeField]
    [Tooltip("is the time in sec to pass the given distance. 'StraightMove' time for the distance. 'CircleMove' time for one turn")]
    private float _time = 10;

    [Header("Initialization - Settings")]
    [SerializeField]
    private MoveBehaviorEnum _behave = MoveBehaviorEnum.Circle;
    [SerializeField]
    [Tooltip("direction of the move-obj. 'StraightMove': true -> toward ref-obj. 'CircleMove': true -> math postivie around ref-obj.")]
    private bool _direction = true;

    [SerializeField]
    [Tooltip("only for 'CircleMove': true -> face everytime to ref-obj; false -> mov-obj keeps start orientation")]
    private bool _turnObject = false;

    [SerializeField]
    [Tooltip("Switch direction after the half of the time")]
    private Loop _loop;
    


    #endregion

    // Strategy Pattern
    private IMoveBehaviorStrategy MoveBehaviour;

    private enum MoveBehaviorEnum
    {
        Straight_Line,
        Circle
    };
    

    private void Awake()
    {
        gObj = this.gameObject;
    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        MoveBehaviour = SetStrategyByDropDown();
        MoveBehaviour.Init(gObj, gRef, _time, _direction, _turnObject);
        MoveBehaviour.StartMovement();

        if (_loop._activate_Loop)
        {
            if (_loop._enable)
            {
                InvokeRepeating("InvokeTurnAround", _time * _loop._time_percent, _time * _loop._time_percent);
            }
            else
            {
                // do nothing
            }
        } 
        else Invoke("Stop", _time);
    }

    // Update every frame and calculate new position for the moving obj
    void Update () {
        MoveBehaviour.DoMovement();
    }

    /**
     * init the right strategy (selected by dropdown in the inspector)
     */
    private IMoveBehaviorStrategy SetStrategyByDropDown()
    {
        IMoveBehaviorStrategy strategy = null;
        switch (_behave)
        {
            case MoveBehaviorEnum.Circle:
                strategy = new CircleMove();
                break;

            case MoveBehaviorEnum.Straight_Line:
                strategy = new StraightMove();
                break;

            default:
                Debug.LogError("Something went wrong with the strategy enum");
                break;
        }
        return strategy;
    }

    private void InvokeTurnAround()
    {
        /*_direction = !_direction;
        //MoveBehaviour = new CircleMove();
        MoveBehaviour.Init(gObj, gRef, _time, _direction, _turnObject);
        MoveBehaviour.StartMovement();*/
        MoveBehaviour.ChangeDirection();
    }

    private void Stop()
    {
        MoveBehaviour.StopMovement();
    }
}
