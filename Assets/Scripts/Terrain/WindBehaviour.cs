using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 push;
    private PlayerControl playerControl;
    List<StatusEffect> statusEffects;

    private void Start()
    {
        playerControl = GameObject.Find("Ifer").GetComponent<PlayerControl>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !StatusEffect.IsStatusEffectApplied<Immovable>(playerControl.GetStatusEffects()))
        {
            if (!gameObject.GetComponent<Knockback>())
            {
                gameObject.AddComponent<Knockback>();
                gameObject.GetComponent<Knockback>().PassData(playerControl.GetRigidbody(), push);
                StatusEffect.AddUniqueStatusEffect<Knockback>(GetComponent<Knockback>(), playerControl.GetStatusEffects());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StatusEffect.RemoveStatusEffect<Knockback>(playerControl.GetStatusEffects());
        }
    }
}
