using UnityEngine;

public class AgeTestLoader : MonoBehaviour
{
    [SerializeField] private GameObject AgeTestPannel;

    private GameObject target = null;

    public void ActiveAgeTest(GameObject newTarget)
    {
        target = newTarget;
        AgeTestPannel.SetActive(true);
    }

    public void OnCorrectInput()
    {
        target.SetActive(true);
        Clear();
    }

    public void OnUncorrectInput()
    {
        Clear();
    }

    private void Clear()
    {
        AgeTestPannel.SetActive(false);
        target = null;
    }

    private void OnDisable()
    {
        target = null;
    }
}
