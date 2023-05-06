using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    bool oneTimeUse = true;
    [SerializeField]
    int addUses = 999;

    public void Interact()
    {
        PlayerManager.AddUsesToAbilities(addUses);
        if (oneTimeUse)
            Destroy(gameObject);
    }
}
