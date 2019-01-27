﻿using System.Collections;
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
            Resetter.instance.OnReset += OnReset;
        }

        void UnsubscribeEvent()
        {
            GameController.OnGameStart -= OnGameStart;
            GameController.OnGameStop -= OnGameStop;
            Resetter.instance.OnReset -= OnReset;
        }

        void OnGameStart()
        {
            Show(Menu.InGameMenu);
        }

        void OnGameStop()
        {
            //Hacks
            Camera.main.GetComponent<CameraFollow>().MakeScroll(false);
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

        void OnReset()
        {
            //
        }
    }
}
