using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tile : MonoBehaviour
{
    [SerializeField] private Material hoveredMaterial = null;
    [SerializeField] private Material activeMaterial = null;

    private bool _isActive;
    private State status;
    public Pieces piece;
    private MeshRenderer _renderer;
    public  static bool selected = false;
    private void Awake()
    {
        
        _renderer = GetComponent<MeshRenderer>();
        SetInit(false);
     
    }

    public void SetInit(bool active)
    {
      
        _isActive = active;
        SetState(_isActive ? State.Hidden : State.Inactive);
    }
    public void SetActive(bool active) {

        selected = active;
        SetState(active ? State.Active : State.Hidden);
    }
    private void OnMouseEnter()
    {
       
        if (_isActive && !selected)
        {
            Board.tileHover = this;
            SetState(State.Hover);
        }
        //prob dont need selected,for in order to be active,
        //would need selected to be true
        if (status == State.Active && selected)
        {
            
            Board.tileHover = this;
            SetState(State.Hover);
        }
    }
    

    private void OnMouseExit()
    {
        Board.tileHover = null;
        if (_isActive && !selected)
        {
            SetState(State.Hidden);
        }
        //prob dont need selected,for in order to be active,
        //would need selected to be true
        if (status == State.Hover && selected)
        {
            
            SetState(State.Active);
        }
    }


    private void SetState(State state)
    {
        status = state;
        switch (state)
        {
            case State.Inactive:
                gameObject.SetActive(false);
                break;
            case State.Hidden:
                gameObject.SetActive(true);
                _renderer.enabled = false;
                break;
            case State.Active:
                //gameObject.SetActive(true);
                _renderer.enabled = true;
                _renderer.material = activeMaterial;
                break;
            case State.Hover:
                //gameObject.SetActive(true);
                _renderer.enabled = true;
                _renderer.material = hoveredMaterial;
                break;
            default:
                break;
        }
    }

    private enum State
    {
        Hidden, Active, Hover,Inactive
    }
}
