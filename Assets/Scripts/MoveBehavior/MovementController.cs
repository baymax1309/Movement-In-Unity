using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour {

    private  GameObject gObj;

    [SerializeField]
    private  GameObject gRef;

    [SerializeField]
    [Tooltip("is the time in sec to pass the given distance. 'StraightMove' time for the distance. 'CircleMove' time for one turn")]
    private float speed = 10;

    private IMoveBehaviorStrategy MoveBehavior;




    private void OnEnable()
    {
        MoveBehavior = new CircleMove();
        MoveBehavior.Init(gObj, gRef, speed);
    }

    private void Awake()
    {
        gObj = this.gameObject;
        
    }

    // Use this for initialization
    void Start () {

        

	}
	
	// Update is called once per frame
	void Update () {
        MoveBehavior.StartMovement();
    }
}
