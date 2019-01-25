using UnityEngine;

namespace GGJ
{
    public class Stat : MonoBehaviour
    {
        [SerializeField]
        float current;

        [SerializeField]
        float maximum;


        public float Current => current;
        public float Max => maximum;

        public delegate void _Func(float value);
        public event _Func OnValueChagned;


        void OnDestroy()
        {
            OnValueChagned = null;
        }

        public void Clear()
        {
            current = 0;
        }

        public void FullRestore()
        {
            current = maximum;
            OnValueChagned?.Invoke(current);
        }

        public void Restore(float value)
        {
            current = (current + value) > maximum ? maximum : (current + value);
            OnValueChagned?.Invoke(current);
        }

        public void Remove(float value)
        {
            current = (current - value) < 0 ? 0 : (current - value);
            OnValueChagned?.Invoke(current);

            //Test[:wa

            Debug.Log("R");
        }
    }
}
