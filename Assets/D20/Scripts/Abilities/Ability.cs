using Unity.Netcode;
using UnityEngine;

public abstract class Ability : NetworkBehaviour
{
    public bool FiresOnComboAdvance;
    public bool FiresOnComboComplete;
    public bool AttachToParent;
    public bool IsNetworkAbility;

    public virtual void ComboAdvanced(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete) { }
}