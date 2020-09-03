using Audiomata;
using System.Collections;
using UnityEngine;

public class SequencePlayer : MonoBehaviour
{
    [SerializeField]
    private string[] querySet;
    [SerializeField]
    private AudioSource source;
    int currentIdx = 0;

    [SerializeField]
    bool playOnePerQuery = false;

    [SerializeField]
    private bool loop = true;

    QueryManager qm;

    void Start()
    {
        if (!source)
        {
            source = GetComponent<AudioSource>();
        }
        qm = AudioManager.Instance.QueryManager;

        StartCoroutine(SequnceAudioSet());
    }
    

    IEnumerator SequnceAudioSet()
    {
        while (loop)
        {
            while (currentIdx < querySet.Length)
            {

                string nextQuery = querySet[currentIdx];
                qm.QueryAudio(nextQuery, out var clips);

                if (playOnePerQuery)
                {
                    source.clip = clips[Random.Range(0, clips.Length)];
                    source.Play();
                    yield return new WaitForSeconds(source.clip.length);
                }
                else
                {
                    for (int i = 0; i < clips.Length; i++)
                    {
                        AudioClip next = clips[i];
                        source.clip = next;
                        source.Play();
                        yield return new WaitForSeconds(next.length);
                    }

                }


                currentIdx++;
            }
            currentIdx = 0;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
