using UnityEngine;

public interface IMoveBehaviorStrategy{

    void Init(GameObject objMove, GameObject objRef, float time, bool direction, bool turnObject);
    void StartMovement();
    void StopMovement();
    void DoMovement();
    void ChangeDirection();


}
