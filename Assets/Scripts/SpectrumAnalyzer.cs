﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpectrumAnalyzer : MonoBehaviour
{
    public int resolution = 1024;
    public Transform lowMeter, midMeter, highMeter;
    public float lowFreqThreshold = 14700, midFreqThreshold = 29400, highFreqThreshold = 44100;
    public float lowEnhance = 1f, midEnhance = 10f, highEnhance = 100f;

    private AudioSource audio_;

    private float[] spectrum;

    void Start()
    {
        spectrum = new float[resolution];
        audio_ = GetComponent<AudioSource>();
        audio_.Play();
    }

    void Update()
    {
        audio_.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        var deltaFreq = AudioSettings.outputSampleRate / resolution;
        float low = 0f, mid = 0f, high = 0f;

        for (var i = 0; i < resolution; ++i)
        {
            var freq = deltaFreq * i;
            if (freq <= lowFreqThreshold) low += spectrum[i];
            else if (freq <= midFreqThreshold) mid += spectrum[i];
            else if (freq <= highFreqThreshold) high += spectrum[i];
        }

        low *= lowEnhance;
        mid *= midEnhance;
        high *= highEnhance;

        lowMeter.localScale = new Vector3(lowMeter.localScale.x, low, lowMeter.localScale.z);
        midMeter.localScale = new Vector3(midMeter.localScale.x, mid, midMeter.localScale.z);
        highMeter.localScale = new Vector3(highMeter.localScale.x, high, highMeter.localScale.z);
    }
}
