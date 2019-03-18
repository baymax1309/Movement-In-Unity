﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StraightMove: IMoveBehaviorStrategy {



    private Vector3 _startVector;
    private Vector3 _refVector;

    private GameObject objMov;
    private GameObject objRef;

    private Rigidbody _rigidBody;

    private float _speed = 1;
    private Vector3 _direction;

    #region Interface Methods
    public void Init(GameObject objMov, GameObject objRef, float time2destination)
    {

        this.objMov = objMov;
        this.objRef = objRef;

        _startVector = objMov.transform.position;
        _refVector = objRef.transform.position;

        AddRigidbody(objMov); // Movement control over rigidbody        
    }

    public void StartMovement()
    {
        if (_rigidBody.velocity.magnitude != 0)
        {
            return;
        }
        else
        {
            Debug.Log("StartMovement - StraightMove");
            _direction = (_refVector - _startVector).normalized;
            _rigidBody.velocity = _direction * this._speed;
        }
        
    }

    public void StopMovement()
    {
        _rigidBody.velocity = Vector3.zero;
    }
    #endregion


    private void AddRigidbody(GameObject obj)
    {
        obj.AddComponent<Rigidbody>().useGravity = false;
        _rigidBody = obj.GetComponent<Rigidbody>(); 
    }
}



    