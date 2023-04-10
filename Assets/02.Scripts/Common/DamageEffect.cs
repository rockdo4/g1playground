using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    private TextMeshPro text;
    private float timer = 0f;
    private float oriFontSize;
    private Renderer textRenderer;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        oriFontSize = text.fontSize;
        textRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        timer += dt;
        transform.position = transform.position + new Vector3(0f, dt, 0f);
        if (timer > 1f)
        {
            timer = 0f;
            GameManager.instance.effectManager.ReturnEffect("Damage", gameObject);
        }
    }

    public void OnDamage(Vector3 pos, int damage, Color color, bool isCritical)
    {
        transform.position = pos + new Vector3(0f, 0f, -20f);
        text.text = damage == 0 ? "¹æ¾î" : damage.ToString();
        text.color = color;
        if (isCritical)
        {
            text.fontStyle = FontStyles.Bold;
            text.fontSize = oriFontSize * 1.4f;
            text.outlineWidth = 0.3f;
        }
        else
        {
            text.fontStyle = FontStyles.Normal;
            text.fontSize = oriFontSize;
            text.outlineWidth = 0f;
        }
        textRenderer.material.renderQueue = 3004;
    }
}
