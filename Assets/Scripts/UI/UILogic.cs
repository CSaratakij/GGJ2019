using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ
{
    public class UILogic : MonoBehaviour
    {
        public void GameStart()
        {
            GameController.GameStart();
        }

        public void GameStop()
        {
            GameController.GameStop();
        }

        public void Restart()
        {
            if (GameController.IsGameStart)
                GameController.GameStop();

            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
