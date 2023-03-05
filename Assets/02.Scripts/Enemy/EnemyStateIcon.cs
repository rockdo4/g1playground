using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyController;

public class EnemyStateIcon : MonoBehaviour
{
    private Image curIcon;
    public Sprite idle;
    public Sprite patrol;
    public Sprite chase;
    public Sprite attack;
    public Sprite takeDamage;
    public Sprite die;

    private EnemyState currentState;
    private EnemyController controller;
    void Start()
    {
        curIcon = GetComponent<Image>();
        controller = GetComponentInParent<EnemyController>();
        currentState = controller.State;
    }

    public void ChangeIcon(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                curIcon.sprite = idle;
                break;
            case EnemyState.Patrol:
                curIcon.sprite = patrol;
                break;
            case EnemyState.Chase:
                curIcon.sprite = chase;
                break;
            case EnemyState.Attack:
                curIcon.sprite = attack;
                break;
            case EnemyState.TakeDamage:
                curIcon.sprite = takeDamage;
                break;
            case EnemyState.Die:
                curIcon.sprite = die;
                break;
        }
    }
}
