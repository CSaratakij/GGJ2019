using UnityEngine;

namespace GGJ
{
    public class CameraEdgeScroll : MonoBehaviour
    {
        public static int PictureID = 0;

        [SerializeField]
        Vector3 viewportBound;

        [SerializeField]
        Vector3 offset;

        [SerializeField]
        SpriteRenderer spriteRenderer;

        [SerializeField]
        Sprite[] sprites;


        bool isVisible = false;
        Vector3 targetPos;

        void Awake()
        {
            spriteRenderer.sprite = sprites[0];
            SetVisible(true);
        }

        void LateUpdate()
        {
            if (isVisible) {
                var screenBottomWorldPos = Camera.main.ViewportToWorldPoint(viewportBound);
                targetPos = screenBottomWorldPos + offset;

                targetPos.x = 0.0f;
                targetPos.z = 0.0f;

                transform.position = targetPos;
            }
        }

        public void SetVisible(bool value)
        {
            isVisible = value;
            spriteRenderer.sprite = sprites[PictureID];
            spriteRenderer.enabled = value;
        }
    }
}
