using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    private ParticleSystem PS;

    void Awake()
    {
        PS = gameObject.GetComponent<ParticleSystem>();
    }

    public void emit(int amount, float lifetime)
    {
        PS.Emit(amount);
        Destroy(gameObject, lifetime);
    }
}
