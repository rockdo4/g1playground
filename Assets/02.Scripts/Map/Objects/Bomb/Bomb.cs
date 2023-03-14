using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private string effect1Name = "Fire_small";
    [SerializeField] private string effect2Name = "Fire_explosion_earth";
    [SerializeField] private GameObject bomb;
    [SerializeField] private float delay = 3f;

    private float timer = 0f;
    private bool isTriggered = false;
    private GameObject effect1;
    private GameObject effect2;


    // Start is called before the first frame update
    void Start()
    {
        TileColorManager.instance.ToInvisibleMaterial(bomb);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            timer += Time.deltaTime;

            //set effect position
            var effectPos = bomb.transform.position;
            effect1.transform.position = new Vector3(effectPos.x, effectPos.y, effectPos.z - 1f);

            //explode in delay
            if (timer >= delay)
            {
                timer = 0f;
                isTriggered = false;

                //set explosion effect
                effect2 = GameManager.instance.effectManager.GetEffect(effect2Name);
                effect2.transform.position = effectPos;
                float destroyTime = effect2.GetComponent<ParticleSystem>().main.duration;
                GameManager.instance.effectManager.ReturnEffectOnTime(effect2Name, effect2, destroyTime);

                bomb.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<ObjectMass>() && !isTriggered) 
        {
            //reveal bomb
            TileColorManager.instance.ToOriginalMaterial(bomb);

            isTriggered = true;

            //set fire effect
            effect1 = GameManager.instance.effectManager.GetEffect(effect1Name);
            GameManager.instance.effectManager.ReturnEffectOnTime(effect1Name, effect1, delay);   
        }
    }
}
