using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class ImageEntry
{
    public int Number;
    public GameObject transitionCanvas;
}

public class TransitionSample : MonoBehaviour
{
    public List<ImageEntry> imageEntries;
    public float duration = 1.0f;

    void Start()
    {
        foreach (var entry in imageEntries)
        {
            if (entry.transitionCanvas != null)
            {
                entry.transitionCanvas.SetActive(false);
            }
        }
    }

    // Set progress of transition image
    public void SetProgress(int no)
    {
        foreach (var entry in imageEntries)
        {
            if (entry.Number == no)
            {
                entry.transitionCanvas.SetActive(true);
                var transitionProgressController = entry.transitionCanvas.GetComponent<TransitionProgressController>();
                StartCoroutine(AnimateProgressCoroutine(transitionProgressController, entry));
                return;
            }
        }
        Debug.LogWarning("No matching entry found for number: " + no);
    }

    // Animate progress of transition image
    private IEnumerator AnimateProgressCoroutine(TransitionProgressController controller, ImageEntry entry)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            controller.progress = elapsedTime / duration;
            yield return null;
        }
        controller.progress = 1.0f;
        yield return new WaitForSeconds(1);
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            controller.progress = 1.0f - (elapsedTime / duration);
            yield return null;
        }
        controller.progress = 0.0f;

        entry.transitionCanvas.SetActive(false);
    }
}
