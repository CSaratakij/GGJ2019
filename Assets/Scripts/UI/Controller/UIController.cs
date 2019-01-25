using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        RectTransform[] panels;

        enum Menu
        {
            MainMenu,
            InGameMenu,
            GameOverMenu
        }

        void Awake()
        {
            SubscribeEvent();
        }

        void Start()
        {
            Show(Menu.MainMenu);
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void SubscribeEvent()
        {
            GameController.OnGameStart += OnGameStart;
            GameController.OnGameStop += OnGameStop;
        }

        void UnsubscribeEvent()
        {
            GameController.OnGameStart -= OnGameStart;
            GameController.OnGameStop -= OnGameStop;
        }

        void OnGameStart()
        {
            Show(Menu.InGameMenu);
        }

        void OnGameStop()
        {
            Show(Menu.GameOverMenu);
        }

        void HideAllMenu()
        {
            foreach (RectTransform menu in panels)
            {
                menu.gameObject.SetActive(false);
            }
        }

        void Show(Menu menu)
        {
            HideAllMenu();
            panels[(int)menu].gameObject.SetActive(true);
        }
    }
}
