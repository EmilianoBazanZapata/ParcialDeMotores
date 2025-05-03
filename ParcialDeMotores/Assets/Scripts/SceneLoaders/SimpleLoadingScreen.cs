using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoaders
{
    public class SimpleLoadingScreen: MonoBehaviour
    {
        [SerializeField] private string sceneToLoad = "CementeryLevel";
        [SerializeField] private float waitTime = 10f;

        private void Start()
        {
            StartCoroutine(DelayedLoad());
        }

        private IEnumerator DelayedLoad()
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}