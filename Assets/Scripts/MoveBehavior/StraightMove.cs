using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StraightMove: IMoveBehaviorStrategy {



    private Vector3 _startVector;
    private Vector3 _refVector;

    private GameObject objMov;
    private GameObject objRef;

    private Rigidbody _rigidBody;

    private float _time = 1;
    private float _speed = 0;
    private Vector3 _v_connect;
    private float _distance;
    private bool _direction;
    private bool _stopMovement;

    #region Interface Methods
    public void Init(GameObject objMov, GameObject objRef, float time2destination, bool direction, bool turnObject)
    {
        Debug.Log("Init");
        

        this._direction = direction;
        this._time = time2destination;

        this._startVector = objMov.transform.position;
        this._refVector = objRef.transform.position;

        this._v_connect = (_direction) ? (_refVector - _startVector) : (_startVector - _refVector);

        this._distance = _v_connect.magnitude;

        this._speed = this._distance / this._time;
        
        this._stopMovement = true;
        
        AddRigidbody(objMov); // Movement control over rigidbody        
          


    }

    public void ChangeDirection()
    {
        StopMovement();
        this._direction = !_direction;
        this._v_connect = (_direction) ? (_v_connect) : -(_v_connect);
        StartMovement();
    }

    public void StartMovement()
    {
        _stopMovement = false;
    }

    public void StopMovement()
    {
        _stopMovement = true;
        Debug.Log("Stooooooooooooooooooooop");
        _rigidBody.velocity = Vector3.zero;
        Debug.Log(_rigidBody.velocity);
    }

    public void DoMovement()
    {
        if (_stopMovement) return;

        if (_rigidBody.velocity.magnitude != 0)
        {
            return;
        }
        else
        {
            Debug.Log("StartMovement - StraightMove");
            Debug.Log("speed = " + _speed);
            Debug.Log(_v_connect);
            Debug.Log(_v_connect.normalized);
            _rigidBody.velocity = _v_connect.normalized * this._speed;
        }
        
    }

    #endregion


    private void AddRigidbody(GameObject obj)
    {
        if(obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            Debug.Log("Rigidbody already exists");
        }
        _rigidBody = obj.GetComponent<Rigidbody>();
        _rigidBody.velocity = Vector3.zero;
    }
}



    