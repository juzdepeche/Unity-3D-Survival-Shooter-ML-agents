using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class PlayerAgent : Agent
{
    [SerializeField] private EnemySpawnerManager _enemySpawnerManager;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerShooting _playerShooting;
    [SerializeField] private PlayerInputs _playerInputs;
    [SerializeField] private bool _heuristicAimbot = false;
    private Ray _shootRay = new Ray();

    private int _floorMask;
    private int _shootableMask;

    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Floor");
        _shootableMask = LayerMask.GetMask("Shootable");
    }

    void FixedUpdate()
    {
        AddReward(0.01f / (float) (_enemySpawnerManager.EnemyCount == 0 ? 1 : _enemySpawnerManager.EnemyCount));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        ActionSegment<float> continuousActions = actions.ContinuousActions;
        ActionSegment<int> discreteActions = actions.DiscreteActions;

        float movementHorizontal = continuousActions[0];
        float movementVertical = continuousActions[1];

        _playerMovement.Move(movementHorizontal, movementVertical);

        float rotationX = continuousActions[2];
        float rotationZ = continuousActions[3];

        _playerMovement.Turning(rotationX, rotationZ);

        if (discreteActions[0] == 1)
        {
            _playerShooting.Shoot();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = _playerInputs.HorizontalAxis;
        continuousActions[1] = _playerInputs.VerticalAxis;

        if (_heuristicAimbot)
        {
            HeuristicAimbot(continuousActions, discreteActions);
        }
        else
        {
            HeuristicAim(continuousActions, discreteActions);
        }
    }

    private void HeuristicAimbot(ActionSegment<float> continuousActions, ActionSegment<int> discreteActions)
    {
        Vector3 closestEnemy = _enemySpawnerManager.GetClosestEnemyPositionFromPlayer();
        if (closestEnemy != Vector3.zero)
        {
            Vector3 direction = closestEnemy - transform.position;
            direction.y = 0f;
            direction.Normalize();

            continuousActions[2] = direction.x;
            continuousActions[3] = direction.z;

            discreteActions[0] = 1;

            _shootRay.origin = transform.position;
            _shootRay.direction = transform.forward;

            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }
    }

    private void HeuristicAim(ActionSegment<float> continuousActions, ActionSegment<int> discreteActions)
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 playerToMouse = Vector3.zero;
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 100f, _floorMask))
        {
            playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            playerToMouse.Normalize();
        }

        continuousActions[2] = playerToMouse.x;
        continuousActions[3] = playerToMouse.z;

        discreteActions[0] = _playerInputs.IsShooting ? 1 : 0;
    }
}
