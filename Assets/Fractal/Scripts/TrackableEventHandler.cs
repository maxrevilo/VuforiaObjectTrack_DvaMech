using UnityEngine;
using UnityEngine.Events;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class TrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES
    
    [SerializeField]
    protected bool DoesActivateChildrenOnTrack = true;
    [SerializeField]
    protected bool DoesDeactivateChildrenOnTrack = true;
    [SerializeField]
    protected UnityEvent OnTrackingFoundEvent;
    [SerializeField]
    protected UnityEvent OnTrackingLostEvent;

    protected TrackableBehaviour MTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        MTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (MTrackableBehaviour)
            MTrackableBehaviour.RegisterTrackableEventHandler(this);

        if (OnTrackingFoundEvent == null)
        {
            OnTrackingFoundEvent = new UnityEvent();
        }

        if (OnTrackingLostEvent == null)
        {
            OnTrackingLostEvent = new UnityEvent();
        }
    }

    protected virtual void OnDestroy()
    {
        if (MTrackableBehaviour)
            MTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <inheritdoc />
    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + MTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + MTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {

        if (DoesActivateChildrenOnTrack)
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (Collider component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (Canvas component in canvasComponents)
                component.enabled = true;
        }
        OnTrackingFoundEvent.Invoke();
    }


    protected virtual void OnTrackingLost()
    {

        if (DoesDeactivateChildrenOnTrack)
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (Collider component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (Canvas component in canvasComponents)
                component.enabled = false;
        }
        OnTrackingLostEvent.Invoke();
    }

    #endregion // PROTECTED_METHODS
}
