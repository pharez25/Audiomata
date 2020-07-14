using UnityEngine;

/// <summary>
/// Interface for interacting with Audio Events
/// </summary>
public interface IAudioEvent
{
    /// <summary>
    /// The object who's values are being modified
    /// </summary>
    Object Target { get; }

    /// <summary>
    /// the id to call upon this event
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Fade in/Out time
    /// </summary>
    float FadeIn { get; set; }

    float FadeOut { get; set; }

    /// <summary>
    /// The status of the event 
    /// setting will update the event target
    /// </summary>
    AudioEventStatus Status { get; set; }

    /// <summary>
    /// Immediately applies the effect
    /// </summary>
    void Apply();

    /// <summary>
    /// Applies the effect based on t/FadeIn
    /// </summary>
    /// <param name="t">How much times has passed since the effect started</param>
    void Apply(float t);

    /// <summary>
    /// Unapplies the effect immediately.
    /// </summary>
    void Remove();

    /// <summary>
    /// Unapplies the event based on t/FadeOut
    /// </summary>
    /// <param name="t">How much time has passed since the fadeout started</param>
    void Remove(float t);
}

/// <summary>
/// The current status of an Audiomata audio event
/// </summary>
public enum AudioEventStatus
{
    Unapplied,Applying,Stopped,Applied,Removing
}

