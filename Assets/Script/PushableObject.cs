using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableObject : MonoBehaviour
{
    [Header("Physics Settings")]
    [Tooltip("How much force is applied when this object is shot.")]
    public float pushForce = 2.0f;

    private Rigidbody rb;

    // We use Awake() because it is called before Start(). It's the safest place for physics setup.
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // This public function is called by our Weapon script.
    public void GetPushed(Vector3 hitDirection)
    {
        // When a force is applied, the Rigidbody will wake up with all the correct
        // physics settings and will behave realistically.
        rb.AddForce(hitDirection * pushForce, ForceMode.Impulse);
    }
}