using System.Collections.Generic;
using UnityEngine;

public class Star_controller : MonoBehaviour
{
    [SerializeField] Vector3 activeStarOffset;
    [SerializeField] StarFinderHelper starFinderHelper;

    private List<KeyValuePair<int, Animator>> list = new List<KeyValuePair<int, Animator>>();

    private int activeStarIndex = 0;

    private Picture_manager picture_Manager;

    private ClickZone_controller clickZone_Controller;

    void Awake()
    {
        if (picture_Manager == null)
        {
            picture_Manager = FindObjectOfType<Picture_manager>();
        }

        if (clickZone_Controller == null)
        {
            clickZone_Controller = FindObjectOfType<ClickZone_controller>();
        }
    }

    public void Clear()
    {
        activeStarIndex = 0;
        list.Clear();
    }

    public void Add(Animator starAnim, Vector3 position, int index)
    {
        list.Add(new KeyValuePair<int, Animator>(index, starAnim));
        list[list.Count - 1].Value.transform.localPosition = position;
    }

    public void StartGame()
    {
        if (list.Count <= 2)
            Debug.LogError("stars count <= 2");


        clickZone_Controller.SetActiveZone(true);
        NextStage();
        list[0].Value.Play("state4");
        list[0].Value.transform.localPosition += activeStarOffset;
    }

    public void NextStage()
    {
        if (picture_Manager.GetIsPrintAnim())
         return;

        if (activeStarIndex + 1 > list.Count)  // Win
        {
            list[0].Value.Play("state3");
            clickZone_Controller.SetActiveZone(false);
            picture_Manager.PrintLine();
            return;
        }

        list[activeStarIndex].Value.Play("state3");
        list[activeStarIndex].Value.transform.localPosition -= activeStarOffset;

        activeStarIndex++;


        if (activeStarIndex == list.Count) // Last star
        {
            ActiveStar(0);

            clickZone_Controller.SetPosition(list[0].Value.transform.localPosition);
            picture_Manager.PrintLine(list[activeStarIndex - 1].Key);
            return;
        }

        picture_Manager.PrintLine(list[activeStarIndex - 1].Key); // Def

        ActiveStar(activeStarIndex);

        clickZone_Controller.SetPosition(list[activeStarIndex].Value.transform.localPosition);
    }

    private void ActiveStar(int index)
    {
        list[index].Value.Play("state1");
        list[index].Value.transform.localPosition += activeStarOffset;

        starFinderHelper.SetPosition(list[index].Value.transform.localPosition);
    }

    public void Debug_star_list()
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(i + ") " + list[i].Value.transform.localPosition);
        }
    }
}
