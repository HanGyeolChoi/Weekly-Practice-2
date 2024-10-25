using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketControllerC : MonoBehaviour
{
    private EnergySystemC _energySystem;
    private RocketMovementC _rocketMovement;
    
    private bool _isMoving;
    private float _movementDirection;
    
    private readonly float ENERGY_TURN = 0.5f;
    private readonly float ENERGY_BURST = 2f;

    [SerializeField] private ObjectPool objPool;
    [SerializeField] private SecondObjectPool objPool2;
    [SerializeField] private ThirdObjectPool objPool3;
    private Queue<GameObject> objQueue;

    private Queue<GameObject> objQueue3;
    private void Awake()
    {
        _energySystem = GetComponent<EnergySystemC>();
        _rocketMovement = GetComponent<RocketMovementC>();
        objQueue = new Queue<GameObject>();
        objQueue3 = new Queue<GameObject>();
    }
    
    private void FixedUpdate()
    {
        if (!_isMoving) return;
        
        if(!_energySystem.UseEnergy(Time.fixedDeltaTime * ENERGY_TURN)) return;
        
        _rocketMovement.ApplyMovement(_movementDirection);
    }

    // OnMove 구현
    // private void OnMove...
    private void OnMove(InputValue value)
    {
        float dir = value.Get<float>();
        _isMoving = Mathf.Abs(dir) >= 0.1f;
        _movementDirection = dir;
    }


    // OnBoost 구현
    // private void OnBoost...
    private void OnBoost(InputValue value)
    {
        if (!_energySystem.UseEnergy(ENERGY_BURST)) return;
        _rocketMovement.ApplyBoost();
    }

    private void OnSpace(InputValue value)
    {
        GameObject obj = objPool.GetObject();
        GameObject obj2 = objPool2.GetObject();
        GameObject obj3 = objPool3.GetObject();
        Debug.Log("Space!");
        objQueue.Enqueue(obj);
        objQueue3.Enqueue(obj3);
    }

    private void OnRelease(InputValue value)
    {
        if (objQueue.TryDequeue(out GameObject obj))
        {
            objPool.ReleaseObject(obj);
        }
        objPool2.ReleaseObject();
        if (objQueue3.TryDequeue(out GameObject obj3))
        {
            objPool3.ReleaseObject(obj3);
        }
    }
}