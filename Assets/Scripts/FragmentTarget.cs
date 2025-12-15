using System.Collections;
using UnityEngine;

public class FragmentTarget : MonoBehaviour
{
    // [!!!] VA EN IMAGE TARGET [!!!]
    // Esta clase 

    private bool collected = false;

    public void OnTargetRecognized()
    {
        if (collected) return;

        collected = true;

        GameManager.Instance.FragmentCollected();
        gameObject.SetActive(false);
    }
}
