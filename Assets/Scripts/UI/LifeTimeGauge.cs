using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;  // 수명 표시할 슬라이더
    private void Awake()
    {
        slider = GetComponent<Slider>();  
    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();  // 시작할 때 Player 찾기
        player.onLifeTimeChange += Refresh;          // 델리게이트에 함수 등록
    }

    private void Refresh(float ratio)  // UI 표시 갱신해야 할 때 실행되는 함수 / ratio = 현재 수명/ 최대 수명
    {
        slider.value = ratio;        // 비율에 맞게 표시
    }
}
