﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Leap.Unity;
using Leap.Unity.Interaction;

[RequireComponent(typeof(AudioSource))]
public class InteractionButtonSounds : MonoBehaviour
{
    public AudioClip hoverSound;
    public AudioClip downSound;
    public AudioClip upSound;

    public AudioSource source;

    private InteractionButton _interactionButton;

    private bool hover = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (source == null) source = GetComponent<AudioSource>();
        _interactionButton = GetComponent<InteractionButton>();
        _interactionButton.OnPress += OnDown;
        _interactionButton.OnUnpress += OnUp;
    }

    private void OnDisable()
    {
        _interactionButton.OnPress -= OnDown;
        _interactionButton.OnUnpress -= OnUp;
    }

    private void Update()
    {
        if (_interactionButton.isPrimaryHovered)
        {
            if (!hover)
            {
                hover = true;
                if (hoverSound != null) { source.PlayOneShot(hoverSound); }
            }
        }
        else
        {
            hover = false;
        }
    }

    public void OnDown()
    {
        if (downSound != null) { source.PlayOneShot(downSound); }
    }

    public void OnUp()
    {
        if (upSound != null) { source.PlayOneShot(upSound); }
    }
}