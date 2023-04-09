using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarFR : BarUI
{
    private Enemy enemy;
    private Slider slider;
    protected override void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        slider = GetComponent<Slider>();
        slider.maxValue = enemy.GetStatus().FinalValue.maxHp;
    }

    private void OnEnable()
    {
        slider.maxValue = enemy.GetStatus().FinalValue.maxHp;
    }

    private void Update()
    {
        slider.value = enemy.GetStatus().CurrHp;
    }

}
