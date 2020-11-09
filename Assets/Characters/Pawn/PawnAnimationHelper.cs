using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PawnAnimationHelper : MonoBehaviour
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
        set => _attackHasHit = value;
    }

    public void AttackHit()
    {
        AttackHasHit = true;
    }
}
