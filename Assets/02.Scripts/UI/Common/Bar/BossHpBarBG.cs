using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarBG : BarUI
{
    private Enemy enemy;
    private Slider slider;
    private float currentValue;
    protected override void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        slider = GetComponent<Slider>();
        slider.maxValue = enemy.GetStatus().FinalValue.maxHp;
    }

    private void OnEnable()
    {
        slider.maxValue = enemy.GetStatus().FinalValue.maxHp;
        currentValue = enemy.GetStatus().CurrHp;
        slider.value = currentValue;
    }

    private void Update()
    {
        float targetValue = enemy.GetStatus().CurrHp;
        currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 2.0f); // 천천히 줄어들게 하기 위해 lerp 함수 사용
        slider.value = currentValue;
    }

}
