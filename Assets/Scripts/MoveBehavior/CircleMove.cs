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
    private bool _direction;
    private bool _turnObject;

    /* circle-equation
     * 
     *  x = cos(phi + c) * distance
     *  z = sin(phi + c) * distance
     *  
     *      phi = w * t = 2pi * f * t = 2pi / T * t
     *        phi ... angle
     *        w ... angular velocity
     *        f ... frequence
     *        T ... time of circulation
     *        t ... time
     *        _angle = phi + c
     */
    private float _w; // [w] = 1/s
    private float _c;
    private float _T; // [T] = 1/s
    private float _phi;
    private float _angle;
    private float _t;


    
    

    #endregion

    #region Interface Methods
    public void Init(GameObject objMov, GameObject objRef, float timeOfCirculation, bool direction, bool turnObject)
    {
        this._objMov = objMov;
        this._objRef = objRef;

        this._startVector_ObjMov = _objMov.transform.position;
        this._startVector_ObjMov_Orientation = _objMov.transform.rotation.eulerAngles;
        this._startVector_ObjRef = _objRef.transform.position;

        this._distanceVector = _startVector_ObjRef - _startVector_ObjMov;
        this._distance = _distanceVector.magnitude;

        this._w = Mathf.PI * 2 / timeOfCirculation;
        this._T = timeOfCirculation;
        this._c = Calculate_c();
        this._t = 0;

        this._direction = direction;
        this._turnObject = turnObject;

        #region Debug
        // Debug Output
        Debug.Log("--Init----------------------------");
        Debug.Log("Omega = " + _w);
        Debug.Log("d = " + _distance);
        Debug.Log("c = " + _c);
        Debug.Log("CounterTime = " + _t);
        Debug.Log("----------------------------------");
        #endregion
    }


    int i = 1; // debug index
    public void StartMovement()
    {
        Debug.Log("i = " + i++); // inkrement

        // calculate new parameter
        _t += Time.deltaTime;
        _phi = _t * _w;
        _phi = (_direction) ? _phi : (-1) * _phi;
        _angle = _phi + _c;



        // calculate new pose
        Vector3 newPos = NewCirclePosition();
        Vector3 newRot;// = NewOrientation4ObjectMove();

        if (_turnObject) newRot = NewOrientation4ObjectMove();
        else newRot = _startVector_ObjMov_Orientation;

        // set new pose
        _objMov.transform.SetPositionAndRotation(newPos, Quaternion.Euler(newRot));
        //_objMov.transform.SetPositionAndRotation(newPos, Quaternion.identity); // no turn of moving obj

    }

    public void StopMovement()
    {
        //_stopMovement = false;
    }
    #endregion


    #region more functions
    private Vector3 NewCirclePosition()
    {
        // calculate local
        float x = Mathf.Cos(_angle) * _distance;
        float y = _objRef.transform.InverseTransformPoint(_startVector_ObjMov).y;
        float z = Mathf.Sin(_angle) * _distance;

        Vector3 newPosLocal = new Vector3(x, y, z);

        // set global
        Vector3 newPosGlobal = _objRef.transform.TransformPoint(newPosLocal);

        #region Debug
        Debug.Log("t = " + _t + " = " + _t / Mathf.PI + " * pi");
        Debug.Log("Phi = w * t = " + _w + " * " + _t + " = " + _phi);
        Debug.Log("Local_V  = (" + newPosLocal.x + ", " + newPosLocal.y + ", " + newPosLocal.z + ")");
        Debug.Log("Global_V = (" + newPosGlobal.x + ", " + newPosGlobal.y + ", " + newPosGlobal.z + ")");
        Debug.Log("----------------------------------");
        #endregion

        return newPosGlobal;
    }

    /**
     * logic for the orientation of the moving object while turning around ref-object
     */
    private Vector3 NewOrientation4ObjectMove()
    {
        float rotX = _startVector_ObjMov_Orientation.x;
        float rotY = _startVector_ObjMov_Orientation.y - (_phi) * 180 / 3.1456f; // phi * 360° / (2*pi)
        float rotZ = _startVector_ObjMov_Orientation.z;
        return new Vector3(rotX, rotY, rotZ); 
    }

         
    /**
     * calclation relates to the local system of the ref-system
     */
    private float Calculate_c()
    {
        Vector3 v_obj_local = _objRef.transform.InverseTransformPoint(_startVector_ObjMov);
        float shift = Mathf.Atan2(v_obj_local.z, v_obj_local.x); // shift at time t=0;
        return shift;
    }
    #endregion
}
