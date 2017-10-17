using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2013, Jean Moreno

// Drag/Drop this script on a Particle System (or an object having Particle System objects as children) to prevent a Shuriken bug
// where a system would emit at its original instantiated position before being translated, resulting in particles in-between
// the two positions.
// Possibly a threading bug from Unity (as of 3.5.4)

public class CFX_ShurikenThreadFix : MonoBehaviour
{
	private ParticleSystem[] systems;
	
	void OnEnable()
	{
		systems = GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem ps in systems) {
            ParticleSystem.EmissionModule emission = ps.emission;
            emission.enabled = false;
        }
		
		StartCoroutine("WaitFrame");
	}
	
	IEnumerator WaitFrame()
	{
		yield return null;
		
		foreach(ParticleSystem ps in systems)
		{
            ParticleSystem.EmissionModule emission = ps.emission;
            emission.enabled = true;
            ps.Play(true);
		}
	}
}