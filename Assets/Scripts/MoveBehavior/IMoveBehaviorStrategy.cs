using UnityEngine;

public interface IMoveBehaviorStrategy{

    void Init(GameObject objMove, GameObject objRef, float speed, bool direction, bool turnObject);
    void StartMovement();
    void StopMovement();
}
