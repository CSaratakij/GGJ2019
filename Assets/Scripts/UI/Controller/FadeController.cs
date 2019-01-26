using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField]
        Animator anim;

        public void FadeIn()
        {
            anim.Play("FadeIn");
        }

        public void FadeOut()
        {
            anim.Play("FadeOut");
        }
    }
}
