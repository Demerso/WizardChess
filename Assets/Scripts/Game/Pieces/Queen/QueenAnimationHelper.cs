using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAnimationHelper : MonoBehaviour
{
    private bool _castAttack = false;

    public bool ShouldCastAttack
    {
        get
        {
            if (!_castAttack) return false;
            _castAttack = false;
            return true;
        }
    }

    public void CastAttack()
    {
        SoundPlayer.sp.Play("QueenAttack");

        _castAttack = true;
    }
    
}
