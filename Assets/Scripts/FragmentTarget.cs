using System.Collections;
using UnityEngine;

public class FragmentTarget : MonoBehaviour
{
    private bool collected = false;

    public void OnTargetRecognized()
    {
        if (collected) return;

        collected = true;
        StartCoroutine(CollectFragment());
    }

    IEnumerator CollectFragment()
    {
        // mini animación simple de ir para arriba
        Vector3 startPos = transform.position;
        Vector3 endPos = GameManager.Instance.fragmentCounterText.transform.position;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        GameManager.Instance.FragmentCollected();
        gameObject.SetActive(false);
    }
}
