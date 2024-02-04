using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public bool FiresOnComboAdvance;
    public bool FiresOnComboComplete;
    public bool AttachToParent;

    public virtual void ComboAdvanced(AbilityControler ac, int roll, Transform target, Canvas canvas) { }
    public virtual void ComboComplete(AbilityControler ac, int roll, Transform target, Canvas canvas) { }
}