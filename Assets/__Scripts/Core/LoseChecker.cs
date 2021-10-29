using UnityEngine;

public class LoseChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("perdeu");
    }
}
