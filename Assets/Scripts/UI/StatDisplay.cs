using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField]
        Slider slider;

        [SerializeField]
        Stat stat;

        void Awake()
        {
            SubscribeEvent();
        }

        void OnEnable()
        {
            slider.value = stat.Current;
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void SubscribeEvent()
        {
            stat.OnValueChagned += stat_OnValueChanged;
        }

        void UnsubscribeEvent()
        {
            stat.OnValueChagned -= stat_OnValueChanged;
        }

        void stat_OnValueChanged(float value)
        {
            slider.value = value;
        }
    }
}
