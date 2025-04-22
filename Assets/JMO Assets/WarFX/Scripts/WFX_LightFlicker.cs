using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public float time = 0.05f;
	
	private float timer;
	
	void OnEnable ()
	{
		timer = time;
		StartCoroutine("Flicker");
	}
	
	IEnumerator Flicker()
	{
		var light = GetComponent<Light>();
    	light.enabled = true;

		while(true)
		{
			do
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			
			light.enabled = !light.enabled;
			timer = time;
		}
	}
}
