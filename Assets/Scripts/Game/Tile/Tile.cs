using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material hoveredMaterial = null;
    [SerializeField] private Material activeMaterial = null;
    [SerializeField] private Material hiddenMaterial;

    private MeshRenderer _renderer;

    public Game game;
    public (int, int) Location = (0, 0);
    public Pieces piece;
    public State state;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        SetState(State.Inactive);
    }

    private void OnMouseEnter()
    {
        if (!game.UIIsOpen)
            IsHovered(true);
    }


    private void OnMouseExit()
    {
        IsHovered(false);
    }


    private void IsHovered(bool hovered)
    {
        if (hovered)
        {
            _renderer.material = hoveredMaterial;
        }
        else
        {
            SetState(state);
        }
    }


    public void SetState(State newState)
    {
        state = newState;
        switch (state)
        {
            case State.Inactive:
                gameObject.SetActive(false);
                break;
            case State.Hidden:
                gameObject.SetActive(true);
                _renderer.material = hiddenMaterial;
                break;
            case State.Active:
                gameObject.SetActive(true);
                _renderer.material = activeMaterial;
                break;
            default:
                break;
        }
    }

    public enum State
    {
        Hidden, Active, Inactive
    }
}
