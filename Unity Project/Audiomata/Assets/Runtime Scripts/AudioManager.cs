using UnityEngine;

public class RuntimeAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    [SerializeField]
    private TextAsset taggingJson;

    public RuntimeAudioManager Instance { get; private set; }

    // Start is called before the first frame update
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
        }

        
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    
    
}
