using Unity.Netcode;
using UnityEngine;

public abstract class Ability : NetworkBehaviour
{
    public bool FiresOnComboStart;
    public bool FiresOnComboAdvance;
    public bool FiresOnComboComplete;
    public bool FiresOnComboFailed;

    public bool AttachToParent;

    public GameObject Owner;

    public virtual void ComboStart(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete) { }

    public virtual void ComboAdvanced(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete) { }

    public virtual void ComboComplete(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete) { }

    public virtual void ComboFailed(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
    {
        GameObject.Destroy(gameObject);
    }
}