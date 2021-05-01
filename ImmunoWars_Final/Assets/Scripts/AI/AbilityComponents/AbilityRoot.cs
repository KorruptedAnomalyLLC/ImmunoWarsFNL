﻿///
///This script controlls all of the unit's Abilities
///

using UnityEngine;

public class AbilityRoot : MonoBehaviour
{
    private LocalBlackboard _localBlackboard;

    public void Setup(LocalBlackboard localBlackboard)
    {
        _localBlackboard = localBlackboard;


        if(TryGetComponent(out MultiplyAbility temp))
        {
            temp.Setup(_localBlackboard);
        }
        if(TryGetComponent(out Collection temp2))
        {
            temp2.Setup(_localBlackboard);
        }
    }

    public void _update()
    {

    }
}
