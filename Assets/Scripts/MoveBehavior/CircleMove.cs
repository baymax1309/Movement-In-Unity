using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour, IMoveBehaviorStrategy
{

    #region Datafield
    private Vector3 _startVector_ObjMov;
    private Vector3 _startVector_ObjMov_Orientation;
    private Vector3 _startVector_ObjRef;

    private GameObject _objMov;
    private GameObject _objRef;

    private float _distance; // [s] = >unity_unit<
    private Vector3 _distanceVector;

    private Rigidbody _rigidBody;

    private float _timeOfCirculation; // [t] = s
    private float _omega; // [w] = 1/s
    private float _timeStart;
    private float _timeCounter;
    private bool _stopMovement = true;
    private bool b_ready = false;

    #endregion

    #region Interface Methods
    public void Init(GameObject objMov, GameObject objRef, float timeOfCirculation)
    {
        this._objMov = objMov;
        this._objRef = objRef;

        this._startVector_ObjMov = _objMov.transform.position;
        this._startVector_ObjMov_Orientation = _objMov.transform.rotation.eulerAngles;
        this._startVector_ObjRef = _objRef.transform.position;

        this._distanceVector = _startVector_ObjRef - _startVector_ObjMov;
        this._distance = _distanceVector.magnitude;

        this._omega = Mathf.PI * 2 / timeOfCirculation;

        this._timeOfCirculation = timeOfCirculation;
        this._timeStart = CalculateTimeStart();
        this._timeCounter = _timeStart;

        
        Debug.Log("Omega = " + _omega);

        b_ready = true;
    }


    int i = 1;
    public void StartMovement()
    {
        //if (!b_ready) return;
        Debug.Log("i = " + i++);

        _timeCounter += Time.deltaTime;
        
        if (!_stopMovement) return;
        else
        {
            // calculate Pose
            Vector3 newPos = NewCirclePosition();
            Vector3 newRot = NewOrientation4ObjectMove();
            
            // set Pose
            _objMov.transform.SetPositionAndRotation(newPos, Quaternion.Euler(newRot));
        }
       
    }

    public void StopMovement()
    {
        _stopMovement = false;
    }

    #endregion


    
    private Vector3 NewCirclePosition()
    {

        float phi = (_timeStart - _c) * _omega;


        float x = -Mathf.Sin(phi) * _distance;
        float y = 0;// _objRef.transform.InverseTransformPoint(_startVector_ObjMov).y;
        float z = Mathf.Cos(phi) * _distance;

        Vector3 newPosLocal = new Vector3(x, y, z);
        Vector3 newPosGlobal = _objRef.transform.TransformPoint(newPosLocal);
        //newPosGlobal.y = _startVector_ObjMov.y; //stays constant 


        Debug.Log("StartTime = " + _timeStart);
        Debug.Log("t = " + _timeCounter + " = " + _timeCounter / Mathf.PI + " pi");
        Debug.Log("Phi = " + phi);
        Debug.Log("Local_V  = (" + newPosLocal.x + ", " + newPosLocal.y + ", " + newPosLocal.z + ")");
        Debug.Log("Global_V = (" + newPosGlobal.x + ", " + newPosGlobal.y + ", " + newPosGlobal.z + ")");
        


        return newPosGlobal;
    }

    private Vector3 NewOrientation4ObjectMove()
    {
        float rotX = _startVector_ObjMov_Orientation.x;
        float rotY = _startVector_ObjMov_Orientation.y - (int)((_timeCounter - _timeStart) * 180 / 3.1456f);
        float rotZ = _startVector_ObjMov_Orientation.z;
        return new Vector3(rotX, rotY, rotZ); 
    }




    private float _c;
   

    private float CalculateTimeStart()
    {

        // returns local vector in relation between ref- and move-obj in the system of ref-obj
        Vector3 v_obj_local = _objRef.transform.InverseTransformPoint(_startVector_ObjMov);
        Debug.Log("v_onj_local");
        Debug.Log(v_obj_local);

        float t = 0;

        float counter;
        float denominator;

        if (_distance == 0 || v_obj_local.z == 0)
        {
            t = (v_obj_local.x > 0) ? -Mathf.PI / 2 : +Mathf.PI / 2;
        }    
        else
        {
            counter = v_obj_local.x;
            denominator = v_obj_local.z;// * _omega;
            Debug.Log("counter = " + counter);
            Debug.Log("denominator = " + denominator);

            //t = Mathf.PI * 2 - Mathf.Atan2(counter , denominator);
            t = - Mathf.Atan2(counter , denominator) / _omega ;
        }


        Debug.Log("Start Time: t = " + t/(Mathf.PI) +" PI");










        _c = Mathf.Asin(_startVector_ObjMov.x / _distance) / _omega + t;
        _c *= (-1);
        _c = 0;


        return t;
    }
}
