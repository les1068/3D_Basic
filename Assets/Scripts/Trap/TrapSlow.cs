using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSlow : TrapBase
{
    public float SlowDuration = 5.0f;   // 슬로우 되는시간
    protected override void OnTrapActivate(GameObject target)
    {
        base.OnTrapActivate(target);

        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.SetHalfSpeed();  // 속도 절반으로 줄이기
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                StartCoroutine(RestoreSpeed(player));
            }
        }
    }
    IEnumerator RestoreSpeed(Player player)   // 속도 정상으로 돌리는 코루틴  / player : 대상 플레이어
    {
        yield return new WaitForSeconds(SlowDuration);
        player.ResetMoveSpeed();
    }
}
