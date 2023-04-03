using System.Collections;
using UnityEngine;

namespace UIExtended
{
    public class FadingShowElement : ShowElement
    {
        [SerializeField] Animator animator;
        [SerializeField] float showedTime;

        public override void Show()
        {
            base.Show();
            animator.SetBool("Disappear", false);
            animator.SetBool("Appear",true);
        }

        public override void Hide()
        {
            base.Hide();
            animator.SetBool("Appear", false);
            animator.SetBool("Disappear", false);
        }

        public void OnAppeared()
        {
            StartCoroutine(Wait());
        }

        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(showedTime);
            animator.SetBool("Disappear",true);
        }
    }
}