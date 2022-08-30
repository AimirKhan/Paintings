using UnityEngine;
using UnityEngine.UI;

public class SimpleInfScroll : MonoBehaviour
{
    [SerializeField] private Sprite[] scrollItems;

    [SerializeField] private Image item1;
    [SerializeField] private Image item2;
    [SerializeField] private bool animPause = false;

    private Animator animator1;
    private Animator animator2;

    private bool activeFirst;
    private bool animIsActive;

    private int activeItem;

    private void OnEnable()
    {
        animator1 = item1.gameObject.GetComponent<Animator>();
        animator2 = item2.gameObject.GetComponent<Animator>();
        activeFirst = true;
        animIsActive = false;
        animator1.Play("open");
        animator2.Play("close");
        activeItem = 0;
        item1.sprite = scrollItems[activeItem];
    }

    public void NextItem()
    {
        AnimStateUpdate();
        if (animIsActive)
        {
            return;
        }

        int oldItem = activeItem;
        activeItem++;
        if (activeItem >= scrollItems.Length)
        {
            activeItem = 0;
        }

        activeFirst = !activeFirst;

        if (!activeFirst)
        {
            animator1.Play("close_left");
            animator2.Play("open_right");

            item1.sprite = scrollItems[oldItem];
            item2.sprite = scrollItems[activeItem];
        }
        else
        {
            animator2.Play("close_left");
            animator1.Play("open_right");

            item2.sprite = scrollItems[oldItem];
            item1.sprite = scrollItems[activeItem];
        }
    }

    public void PrevItem()
    {
        AnimStateUpdate();
        if (animIsActive)
        {
            return;
        }

        int oldItem = activeItem;
        activeItem--;
        if (activeItem <= 0)
        {
            activeItem = scrollItems.Length - 1;
        }

        activeFirst = !activeFirst;

        if (activeFirst)
        {
            animator1.Play("close_right");
            animator2.Play("open_left");

            item1.sprite = scrollItems[oldItem];
            item2.sprite = scrollItems[activeItem];
        }
        else
        {
            animator2.Play("close_right");
            animator1.Play("open_left");

            item2.sprite = scrollItems[oldItem];
            item1.sprite = scrollItems[activeItem];
        }

    }

    private void AnimStateUpdate()
    {
        if (!animPause)
        {
            animIsActive = false;
            return;
        }

        animIsActive =
            !((animator1.GetCurrentAnimatorStateInfo(0).IsName("close") ||
            animator1.GetCurrentAnimatorStateInfo(0).IsName("open")) &&
            (animator2.GetCurrentAnimatorStateInfo(0).IsName("close") ||
            animator2.GetCurrentAnimatorStateInfo(0).IsName("open")));
    }
}
