using UnityEngine;

public class RookAnimationHelper : MonoBehaviour
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
        SoundPlayer.sp.Play("RooksAttack");
        _attackHasHit = true;
    }
}
