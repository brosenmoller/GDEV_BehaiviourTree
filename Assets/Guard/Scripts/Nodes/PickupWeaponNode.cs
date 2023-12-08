using BehaiviourTree;
using System;
using UnityEngine;

public class PickupWeaponNode : Node
{
    private readonly Transform weaponHolder;
    private Transform weaponPickup;

    public PickupWeaponNode(Transform weaponHolder) 
    {
        this.weaponHolder = weaponHolder;
    }

    public override void OnEnter()
    {
        weaponPickup = Array.Find(
            blackboard.GetVariable<Transform[]>(VariableNames.VISIBLE_TARGETS_TransformArray),
            element => element.gameObject.CompareTag("Weapon")
        );

        weaponPickup.SetParent(weaponHolder);
        weaponPickup.localPosition = new Vector3(0, 0, 0.47f);
        weaponPickup.localRotation = Quaternion.identity;

        blackboard.SetVariable(VariableNames.HELT_WEAPON_GameObject, weaponPickup.gameObject);
        blackboard.SetVariable(VariableNames.SEARCHING_FOR_WEAPON_Bool, false);

        Status = NodeStatus.Success;
    }
}

