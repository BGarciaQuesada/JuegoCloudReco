using System.Collections;
using UnityEngine;
using Vuforia;

public class FragmentTarget : MonoBehaviour
{
    // [!!!] VA EN IMAGE TARGET [!!!]
    // Esta clase 

    private bool collected = false;
    bool canCollect = false;
    ObserverBehaviour observer;

    void Awake()
    {
        observer = GetComponentInParent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnTargetStatusChanged;
        StartCoroutine(EnableCollectionDelay());
    }

    void OnDestroy()
    {
        observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (!canCollect || collected) return;

        if (status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED)
        {
            collected = true;
            GameManager.Instance.FragmentCollected();
        }
    }

    // Esta corutina solo es para que espere un ratito a que se descargue el asset y no explote inmediatamente
    IEnumerator EnableCollectionDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canCollect = true;
    }
}
