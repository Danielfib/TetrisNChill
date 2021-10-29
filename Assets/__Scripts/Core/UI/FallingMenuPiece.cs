using UnityEngine;
using DG.Tweening;

public class FallingMenuPiece : MonoBehaviour
{
    private void Start()
    {
        Fall();
    }

    private void Fall()
    {
        var dist = transform.position.y.Difference(-5);
        transform.DOMoveY(-5, dist).SetEase(Ease.Linear).OnComplete(() => {
            var r = Random.Range(0, 360);
            var randomRot = r - (r % 90);
            transform.localEulerAngles = Vector3.forward * randomRot;
            transform.position = new Vector3(transform.position.x, 21f, transform.position.z);
            Fall();
        });
    }
}
