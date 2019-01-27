using UnityEngine;

namespace GGJ
{
    public class ThemeSongChanger : MonoBehaviour
    {
        [SerializeField]
        bool isDisableOnExist;

        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        AudioClip enterSong;

        [SerializeField]
        AudioClip existSong;


        void OnTriggerEnter2D(Collider2D collision)
        {
            if (audioSource == null)
                return;

            if (collision.CompareTag("Player")) {
                audioSource.clip = enterSong;
                audioSource.Play();
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (audioSource == null)
                return;

            if (collision.CompareTag("Player")) {
                audioSource.clip = existSong;
                audioSource.Play();

                if (isDisableOnExist)
                    gameObject.SetActive(false);
            }
        }

    }
}
