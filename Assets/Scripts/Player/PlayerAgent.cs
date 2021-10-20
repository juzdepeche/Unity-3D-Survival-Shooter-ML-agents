using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class PlayerAgent : Agent
{
    public Action OnEpisodeBeginEvent;
    [SerializeField] private EnemySpawnerManager _enemySpawnerManager;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerShooting _playerShooting;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerInputs _playerInputs;

    [SerializeField] private bool _heuristicAimbot = false;

    private int _floorMask;

    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Floor");
    }

    void FixedUpdate()
    {
        if (_enemySpawnerManager == null)
            return;
            
        AddReward(0.01f / (float) (_enemySpawnerManager.EnemyCount == 0 ? 1 : _enemySpawnerManager.EnemyCount));
    }

    public override void OnEpisodeBegin()
    {
        OnEpisodeBeginEvent?.Invoke();
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

    public void ResetPlayer()
    {
        transform.localPosition = Vector3.zero;
        _playerHealth.ResetHealth();
    }

    private void HeuristicAimbot(ActionSegment<float> continuousActions, ActionSegment<int> discreteActions)
    {
        if (_enemySpawnerManager == null)
        {
            Debug.LogWarning("This agent has no EnemySpawnerManager reference to aimbot. Are you in a training environment?");
            discreteActions[0] = 0;
            return;
        }
        
        Vector3 closestEnemy = _enemySpawnerManager.GetClosestEnemyPositionFromPlayer();
        if (closestEnemy != Vector3.zero)
        {
            Vector3 direction = closestEnemy - transform.localPosition;
            direction.y = 0f;
            direction.Normalize();

            continuousActions[2] = direction.x;
            continuousActions[3] = direction.z;

            discreteActions[0] = 1;

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
            playerToMouse = floorHit.point - transform.localPosition;
            playerToMouse.y = 0f;
            playerToMouse.Normalize();
        }

        continuousActions[2] = playerToMouse.x;
        continuousActions[3] = playerToMouse.z;

        discreteActions[0] = _playerInputs.IsShooting ? 1 : 0;
    }
}
