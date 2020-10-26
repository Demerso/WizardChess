using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tile : MonoBehaviour
{
    [SerializeField] private Material hoveredMaterial = null;
    [SerializeField] private Material activeMaterial = null;

    private bool _isActive;
    
    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        SetState(_isActive ? State.Active : State.Hidden);
    }

    private void OnMouseEnter()
    {
        if (_isActive)
        {
            SetState(State.Hover);
        }
    }

    private void OnMouseExit()
    {
        if (_isActive)
        {
            SetState(State.Active);
        }
    }

    private void SetState(State state)
    {
        switch (state)
        {
            case State.Hidden:
                gameObject.SetActive(false);
                break;
            case State.Active:
                gameObject.SetActive(true);
                _renderer.material = activeMaterial;
                break;
            case State.Hover:
                gameObject.SetActive(true);
                _renderer.material = hoveredMaterial;
                break;
            default:
                break;
        }
    }

    private enum State
    {
        Hidden, Active, Hover 
    }
}
