using UnityEngine;

public interface IMoveBehaviorStrategy{

    void Init(GameObject objMove, GameObject objRef, float speed);
    void StartMovement();
    void StopMovement();
}
