using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    [SerializeField] private float fov = 45f;
    [SerializeField] private float radius = 10f;
    private float alertThreshold;

    private Animator animator;
    private static readonly int blinking = Animator.StringToHash("isBlinking");

    private void Start()
    {
        animator = GetComponent<Animator>();
        // FOV를 라디안으로 변환하고 코사인 값을 계산
        alertThreshold = Mathf.Cos(fov * Mathf.Deg2Rad / 2f);
    }

    private void Update()
    {
        CheckAlert();
    }

    private void CheckAlert()
    {
        // 'Aestroid' 레이어에 있는 모든 오브젝트를 탐색
        int layerMask = LayerMask.GetMask("Aestroid");
        var hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.up, 0f, layerMask);
        
        bool needAlert = false;
        foreach (var hit in hits)
        {
            // 목표물까지의 방향 계산
            Vector2 directionToTarget = (hit.transform.position - transform.position).normalized;
            // 'transform.up' 방향과의 내적을 이용해 시야각 검사
            float cos = Vector2.Dot(directionToTarget, transform.up.normalized);
            if (cos >= alertThreshold)
            {
                needAlert = true;
                break;
            }
        }
        // 애니메이션 상태 업데이트
        animator.SetBool(blinking, needAlert);
    }
}