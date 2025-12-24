using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public interface ComboListener
{
    void OnComboStart();

    void OnComboStageAdvance(int roll);

    void OnComboComplete(int total);

    void OnComboFail(int roll);
}

public class ComboController : MonoBehaviour
{
    public float MaxComboStageTime = 10f;
    public ComboListener ComboListener {  get; set; }

    private D20Controller D20Controller;
    private D20FaceEmissionControl FaceEmissionControl;

    private ComboDefinition ActiveCombo;

    private int ComboStage = 0;
    private string debugState = "None";
    private List<int> RolledValues;

    private float innerTimer = 0f;

    private string comboName = "None";

    void Start()
    {
        D20Controller = GetComponent<D20Controller>();
        FaceEmissionControl = GetComponent<D20FaceEmissionControl>();
    }

    private void Update()
    {
        if(innerTimer > MaxComboStageTime)
        {
            innerTimer = 0f;
            ClearState();
            D20Controller.PlaySoundRandomPitch("cough");
        }

        if (ActiveCombo != null)
        {
            innerTimer += Time.deltaTime;
        }
    }

    public void StartCombo(string name)
    {
        FailCombo(-1);
        ActiveCombo = ComboConfig.GetComboDefinitionByName(name);
        FaceEmissionControl.HighlightFaces(GetCurrentStageValues().ToArray());
        comboName = name;

        ComboListener.OnComboStart();

        debugState = "Doing " + name;
    }

    public bool ValidateRoll(int roll)
    {
        if (ActiveCombo == null) return false;

        if (GetCurrentStageValues().Contains(roll))
        {
            AdvanceStage(roll);
            return true;
        }
        else
        {
            FailCombo(roll);
            return false;
        }
    }

    private void AdvanceStage(int roll)
    {
        innerTimer= 0f;
        RolledValues.Add(roll);
        ComboListener.OnComboStageAdvance(roll);
        EventBroker<OnHitValueEvent>.PublishEvent(new(roll.ToString(), "green"));
        debugState = "Cleared Stage " + ComboStage;

        if (++ComboStage == ActiveCombo.GetStageCount())
        {
            ComboListener.OnComboComplete(RolledValues.Sum());
            EventBroker<OnHitValueEvent>.PublishEvent(new("Total: " + RolledValues.Sum(), "#FFD400"));
            debugState = "Done!";
            ClearState();
            return;
        }
        FaceEmissionControl.HighlightFaces(GetCurrentStageValues().ToArray());
    }

    private void FailCombo(int roll)
    {
        ClearState();
        if (roll > 0)
        {
            EventBroker<OnHitValueEvent>.PublishEvent(new(roll.ToString(), "#A05C24"));
            ComboListener.OnComboFail(roll);
        }
        debugState = "Failed";
    }

    private void ClearState()
    {
        ActiveCombo = null;
        ComboStage = 0;
        innerTimer = 0f;
        RolledValues = new List<int>();
        FaceEmissionControl.ClearFaceHighlight();
    }

    private List<int> GetCurrentStageValues()
    {
        return ActiveCombo.GetStage(ComboStage).AllowedValues;
    }

    public override string ToString()
    {
        return "> ComboController:\n  State:\t\t\t" + debugState;
    }
}
