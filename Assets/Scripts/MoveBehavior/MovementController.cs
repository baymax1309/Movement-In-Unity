using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour {

    #region Inspector
    private  GameObject gObj;

    [SerializeField]
    private  GameObject gRef;

    [SerializeField]
    [Tooltip("is the time in sec to pass the given distance. 'StraightMove' time for the distance. 'CircleMove' time for one turn")]
    private float _speed = 10;

    [Header("Initialization - Settings")]
    [SerializeField]
    private MoveBehaviorEnum _behave = MoveBehaviorEnum.Circle;
    [SerializeField]
    [Tooltip("direction of the move-obj. 'StraightMove': true -> toward ref-obj. 'CircleMove': true -> math postivie around ref-obj.")]
    private bool _direction = true;

    [SerializeField]
    [Tooltip("only for 'CircleMove': true -> face everytime to ref-obj; false -> mov-obj keeps start orientation")]
    private bool _turnObject = false;    
    #endregion

    // Strategy Pattern
    private IMoveBehaviorStrategy MoveBehavior;

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


        MoveBehavior = SetStrategyByDropDown();
        MoveBehavior.Init(gObj, gRef, _speed, _direction, _turnObject);
    }
	
	// Update every frame and calculate new position for the moving obj
	void Update () {
        MoveBehavior.StartMovement();
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
}
