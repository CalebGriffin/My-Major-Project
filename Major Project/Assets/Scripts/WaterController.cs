using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pinwheel.Poseidon;

public class WaterController : MonoBehaviour
{
    [SerializeField] private PWater water;

    // Water Variables
    private const float MIN_RIPPLE_HEIGHT = 0.4f, MAX_RIPPLE_HEIGHT = 1f;
    private const float MIN_RIPPLE_SPEED = 6f, MAX_RIPPLE_SPEED = 10f;
    private const float MIN_WAVE_DIRECTION = 225f, MAX_WAVE_DIRECTION = 315f;
    private const float MIN_WAVE_SPEED = 3f, MAX_WAVE_SPEED = 10f;
    private const float MIN_WAVE_HEIGHT = 0.6f, MAX_WAVE_HEIGHT = 1.8f;
    private const float MIN_WAVE_LENGTH = 30f, MAX_WAVE_LENGTH = 60f;
    private const float MIN_WAVE_STEEPNESS = 0f, MAX_WAVE_STEEPNESS = 0.5f;
    private const float MIN_WAVE_DEFORM = 0.5f, MAX_WAVE_DEFORM = 1f;

    private const float LERP_TIME = 60f;
    private const float WAIT_TIME = 30f;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(UpdateWaterValues());
    }

    // Update is called once per frame
    void Update()
    {
        water.ManualTimeSeconds += Time.deltaTime;
        //water.UpdateMaterial();
    }

    private IEnumerator UpdateWaterValues()
    {
        yield return new WaitForSeconds(WAIT_TIME);

        float previousRippleHeight = water.Profile.RippleHeight;
        float previousRippleScale = water.Profile.RippleNoiseScale;
        float previousRippleSpeed = water.Profile.RippleSpeed;
        float previousWaveDirection = water.Profile.WaveDirection;
        float previousWaveSpeed = water.Profile.WaveSpeed;
        float previousWaveHeight = water.Profile.WaveHeight;
        float previousWaveLength = water.Profile.WaveLength;
        float previousWaveSteepness = water.Profile.WaveSteepness;
        float previousWaveDeform = water.Profile.WaveDeform;
        float previousShorelineDistance = water.Profile.FoamDistance;

        LeanTween.value(water.gameObject, previousRippleHeight, Random.Range(MIN_RIPPLE_HEIGHT, MAX_RIPPLE_HEIGHT), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.RippleHeight = value;
        });
        LeanTween.value(water.gameObject, previousRippleSpeed, Random.Range(MIN_RIPPLE_SPEED, MAX_RIPPLE_SPEED), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.RippleSpeed = value;
        });
        LeanTween.value(water.gameObject, previousWaveDirection, Random.Range(MIN_WAVE_DIRECTION, MAX_WAVE_DIRECTION), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.WaveDirection = value;
        });
        LeanTween.value(water.gameObject, previousWaveSpeed, Random.Range(MIN_WAVE_SPEED, MAX_WAVE_SPEED), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.WaveSpeed = value;
        });
        LeanTween.value(water.gameObject, previousWaveHeight, Random.Range(MIN_WAVE_HEIGHT, MAX_WAVE_HEIGHT), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.WaveHeight = value;
        });
        LeanTween.value(water.gameObject, previousWaveLength, Random.Range(MIN_WAVE_LENGTH, MAX_WAVE_LENGTH), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.WaveLength = value;
        });
        LeanTween.value(water.gameObject, previousWaveSteepness, Random.Range(MIN_WAVE_STEEPNESS, MAX_WAVE_STEEPNESS), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.WaveSteepness = value;
        });
        LeanTween.value(water.gameObject, previousWaveDeform, Random.Range(MIN_WAVE_DEFORM, MAX_WAVE_DEFORM), LERP_TIME).setOnUpdate((float value) =>
        {
            water.Profile.WaveDeform = value;
        }).setOnComplete(() =>
        {
            StartCoroutine(UpdateWaterValues());
        });
    }

    [ContextMenu("AddWater")]
    public void AddWater()
    {
        for (int x = 0; x < 50; x++)
        {
            for (int z = 0; z < 50; z++)
            {
                if (x == 0 && z == 0)
                    continue;
                water.TileIndices.Add(new PIndex2D(x, z));
            }
        }
    }
}
