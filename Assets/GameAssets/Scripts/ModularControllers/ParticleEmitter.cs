using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour
{
    [Serializable] public enum ParticleType
    {
        Heal,
        Damage,
        Walking
    }

    [Serializable] public class ParticleEntry
    {
        public ParticleType type;
        public ParticleSystem prefab;
        public Vector3 offset = Vector3.zero; // <-- offset personalizzato
    }

    [Header("Particle Library")]
    [SerializeField] private List<ParticleEntry> particleEntries = new List<ParticleEntry>();

    private Dictionary<ParticleType, ParticleSystem> particleLibrary = new Dictionary<ParticleType, ParticleSystem>();

    private void Awake()
    {
        // Inizializza dizionario
        foreach (var entry in particleEntries)
        {
            if (entry.prefab != null && !particleLibrary.ContainsKey(entry.type))
            {
                // Istanzio il prefab come figlio, con offset
                ParticleSystem instance = Instantiate(entry.prefab, transform);
                instance.transform.localPosition = entry.offset;
                instance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particleLibrary.Add(entry.type, instance);
            }
        }
    }

    public void Play(ParticleType type)
    {
        if (particleLibrary.TryGetValue(type, out ParticleSystem ps))
        {
            ps.Play();
        }
        else
        {
            Debug.LogWarning($"ParticleEmitter: nessuna particella trovata per {type}");
        }
    }

    public void Stop(ParticleType type)
    {
        if (particleLibrary.TryGetValue(type, out ParticleSystem ps))
        {
            ps.Stop();
        }
    }

    public void SetColor(ParticleType type, Color color)
    {
        if (particleLibrary.TryGetValue(type, out ParticleSystem ps))
        {
            var main = ps.main;
            main.startColor = color;
        }
    }

    public void SetSpeed(ParticleType type, float speed)
    {
        if (particleLibrary.TryGetValue(type, out ParticleSystem ps))
        {
            var main = ps.main;
            main.startSpeed = speed;
        }
    }

    public void SetSize(ParticleType type, float size)
    {
        if (particleLibrary.TryGetValue(type, out ParticleSystem ps))
        {
            var main = ps.main;
            main.startSize = size;
        }
    }
}
