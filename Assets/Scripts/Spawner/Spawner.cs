using UnityEngine;

namespace GGJ
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        int spawnID;

        [SerializeField]
        GameObject playerPrefabs;


        public static int currentSpawnID = 0;


        void Awake() {
            if (currentSpawnID == spawnID)
                Spawn();
        }

        void Spawn()
        {
            Instantiate(playerPrefabs, transform.position, Quaternion.identity);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && currentSpawnID != spawnID) {
                currentSpawnID = spawnID;
            }
        }
    }
}
