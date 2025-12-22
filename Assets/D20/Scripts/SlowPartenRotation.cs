using UnityEngine;

public class SlowPartenRotation : MonoBehaviour
{
    public float slowFactor = 0.3333f;
    private Quaternion lastPartenRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPartenRotation = transform.parent.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var parentInverseRotation = Quaternion.Inverse(transform.parent.localRotation);
        var currentLocalRotation = transform.localRotation;

        //Rewind parent rotation
        transform.localRotation = parentInverseRotation * lastPartenRotation * transform.localRotation;

        //Apply scaled rotation
        transform.localRotation = Quaternion.Lerp(currentLocalRotation, transform.localRotation, slowFactor);

        lastPartenRotation = transform.parent.rotation;
    }
}
