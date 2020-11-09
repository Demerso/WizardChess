using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimationHelper : MonoBehaviour
{
    private bool _attackHasHit = false;

    public bool AttackHasHit
    {
        get
        {
            if (!_attackHasHit)
                return false;
            _attackHasHit = false;
            return true;
        }
    }

    public void AttackHit()
    {
        _attackHasHit = true;
    }
}
