using JetBrains.Annotations;
using UnityEngine;

public class DeactivateOnAwake : MonoBehaviour
{
	void Awake ()
	{
        gameObject.SetActive(false);
	}
    
    [UsedImplicitly]
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    [UsedImplicitly]
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
