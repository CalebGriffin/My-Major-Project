using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource, forestSource, seaSource, windSource, forestWindSource, effectSource;

    private float lowForestVolume = 0.2f;
    private float highForestVolume = 0.7f;
    private float forestBlendTime = 3f;

    private float windSourcesVolume = 1f;
    private float windSourcesBlendTime = 10f;

    private Vector3 islandCenter = new Vector3(35, 1, -35);

    [SerializeField] private Collider[] triggers;

    public static SoundSystem Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsInAnyTrigger())
            EnterTown();
        else
            ExitTown();
    }

    // Update is called once per frame
    void Update()
    {
        SetSeaVolume();
    }

    public void PlayEffect(AudioClip clip, float volume = 1f)
    {
        effectSource.PlayOneShot(clip, volume);
    }

    private void SetSeaVolume()
    {
        if (forestSource.volume < highForestVolume)
            seaSource.volume = 0;

        // Calculate distance between island center and player
        float distance = Vector3.Distance(islandCenter, GRefs.Instance.PlayerTransform.position);

        // Calculate volume based on distance
        float volume = Mathf.Clamp((distance / 175) - 0.3f, 0, 0.8f);

        // Set volume
        seaSource.volume = volume;
    }

    public void EnterTown()
    {
        print("EnterTown");
        LeanTween.value(forestSource.gameObject, forestSource.volume, lowForestVolume, forestBlendTime).setOnUpdate((float value) =>
        {
            forestSource.volume = value;
        });
        LeanTween.value(windSource.gameObject, windSource.volume, windSourcesVolume, windSourcesBlendTime).setOnUpdate((float value) =>
        {
            windSource.volume = value;
            forestWindSource.volume = windSourcesVolume - value;
        });
    }

    public void ExitTown()
    {
        print("ExitTown");
        LeanTween.value(forestSource.gameObject, forestSource.volume, highForestVolume, forestBlendTime).setOnUpdate((float value) =>
        {
            forestSource.volume = value;
        });
        LeanTween.value(forestWindSource.gameObject, forestWindSource.volume, windSourcesVolume, windSourcesBlendTime).setOnUpdate((float value) =>
        {
            forestWindSource.volume = value;
            windSource.volume = windSourcesVolume - value;
        });
    }

    private bool IsInAnyTrigger()
    {
        foreach (Collider trigger in triggers)
        {
            if (trigger.bounds.Contains(GRefs.Instance.PlayerTransform.position))
                return true;
        }

        return false;
    }
}
