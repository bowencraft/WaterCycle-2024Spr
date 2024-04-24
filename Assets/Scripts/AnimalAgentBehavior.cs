using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AnimalAgentBehaavior : MonoBehaviour
{
    [HideInInspector]
    public float moveSpeed = 1f;

    public BoxCollider hangOutArea;
    
    [HideInInspector]
    public Vector3 hangOutAreaMin; // 挂出区域的最小边界

    [HideInInspector]
    public Vector3 hangOutAreaMax; // 挂出区域的最大边界

    [HideInInspector]
    public float hangOutWaitTime = 2f; // 停顿时间
    
    // ensure prewalk doesn't trigger when changing direction
    Vector3 _previousMovement;
    public NavMeshAgent navMeshAgent;
    
    private void Awake()
    {
        if (navMeshAgent == null) navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
    }

    void Start()
    {
        
        if (hangOutArea != null) {
            hangOutArea.enabled = true;
            hangOutAreaMin = hangOutArea.bounds.min; 
            hangOutAreaMax = hangOutArea.bounds.max;
        }
        
        SelectRandomTargetPosition(20);
        StartHangingOut();
        
        
    }

    
    public Vector3 _targetPosition;
    // public bool _isMovingToTarget = false;


    public bool _isHangingOut = true;
    // public float _hangOutTimer = 0f;

    private bool _reachedTarget = false;
    
    void Update()
    {
        navMeshAgent.speed = moveSpeed;
        
        if (_isHangingOut)
        {
            if (!ReachedTarget())
            {
                // _characterBehavior.IsPendingTowardsTarget = true;
                MoveTowardsTarget();
                // _reachedTarget = false;
            }
            else
            {
                // if (!_reachedTarget)
                // {
                    StopHangingOut();
                    print("Stopped animation");
                    
                    // SetTargetPosition(transform.position);
                    
                    _reachedTarget = true;
                    // StartCoolDown();

                    SelectRandomTargetPosition(20);
                    StartHangingOut();
                // }

            }
        }
    }
    
    IEnumerator StartCoolDown()
    {
        print("Arrived target, start cooldown");
        yield return new WaitForSeconds(hangOutWaitTime);
        SelectRandomTargetPosition(20);
        StartHangingOut();
    }


    public void StartHangingOut()
    {
        // Debug.Log("Start hanging out!");
        
        // Debug.Log("Set iswalking true");
        // animator.SetBool("isWalking", true);
        _isHangingOut = true;
        _reachedTarget = false;
        // SelectRandomTargetPosition();
        // _hangOutTimer = hangOutWaitTime;
    }

    public void StopHangingOut()
    {
        _isHangingOut = false;
        
        // Debug.Log("Set iswalking false");
        // animator.SetBool("isWalking", false);
    }
    public void SelectRandomTargetPosition(float radius)
    {
        bool isPathValid = false;
        int i = 0;
        while (!isPathValid && i < 5)
        {
            i++;
            // 生成一个随机角度
            float randomAngle = Random.Range(0, 360);
            // 计算新的目标位置
            Vector3 direction = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle));
            Vector3 targetPosition = transform.position + direction * radius;
            // 使用射线检测来确定新位置是否有效
            RaycastHit hit;
            if (Physics.Raycast(targetPosition + Vector3.up * 50, Vector3.down, out hit, 100))
            {
                targetPosition = hit.point;
                isPathValid = SetTargetPosition(targetPosition);
            }
        }

        if (!isPathValid)
        {
            Debug.Log("No valid path found!");
            isPathValid = SetTargetPosition(transform.position);
        }
    }

    public bool SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(_targetPosition, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(_targetPosition);
            // _isMovingToTarget = true;
            return true;
        }
        return false;
    }
    //
    // public void MoveToPosition(Vector3 position)
    // {
    //     _targetPosition = position;
    //     _isMovingToTarget = true;
    // }

    private float _stuckCount = 0;
    private bool ReachedTarget()
    {
        
        if (navMeshAgent.velocity == new Vector3(0,0,0))
        {
            if (_stuckCount < 0.5f)
            {
                _stuckCount += Time.deltaTime;
            }
            else
            {
                // animator.SetBool("isWalking", false);
                _stuckCount = 0;
                return true;
            }
            // _isMovingToTarget = false;
        } else 
        if (Vector3.Distance(transform.position, _targetPosition) < 1f)
        {
            return true;
        }

        return false;
    }
    
    private void MoveTowardsTarget()
    {
        // if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        // {
        //     animator.SetBool("isWalking", false);
        //     // _isMovingToTarget = false;
        //     return;
        // }

        // animator.SetBool("isWalking", true);

        // Vector3 movementDirection = (_targetPosition - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

        //     
        // if (navMeshAgent.velocity.x < 0)
        // {
        //     _leafValue = 1;
        //     _visual.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
        // }
        // else if (navMeshAgent.velocity.x > 0)
        // {
        //     _leafValue = 0;
        //     _visual.localScale = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
        // }
    }

}
