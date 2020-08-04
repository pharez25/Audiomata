using UnityEngine;
namespace Audiomata
{


    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioData[] clips;



        public static AudioManager Instance { get; private set; }

        public QueryManager QueryManager { get; private set; }
        public bool IsSetUp { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("2 Audiomata Runtime Managers present on this scene, this is a static-based class");
                Destroy(this);
                return;
            }
            QueryManager = new QueryManager(clips);
            IsSetUp = true;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}