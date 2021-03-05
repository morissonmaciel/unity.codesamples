using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Third Person Framework/Characters/Traversal/Climbable")]
[RequireComponent(typeof(BoxCollider))]
public class TPS_Climbable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var isPlayer = other.gameObject.tag == "Player";

        if (isPlayer)
        {
            var climber = other.gameObject.GetComponent<TPS_ClimberCharacter>();
            climber.SetIsClimbing(true, this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var isPlayer = other.gameObject.tag == "Player";

        if (isPlayer)
        {
            var climber = other.gameObject.GetComponent<TPS_ClimberCharacter>();
            climber.SetIsClimbing(false, null);
        }
    }

}
