using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public ParticleSystem Highlighter;
    public Color hoveredColor = Color.white;
    public Color selectedColor = Color.green;

    private ParticleSystem _hoveredHighlighter;
    private ParticleSystem _slectedHighlighter;
    private PlayerData _playerData;
    private ControlledСharacter _hoveredCharacter;

    void Start()
    {
        this._playerData = GetComponent<PlayerData>();   
        this._hoveredHighlighter = Instantiate(this.Highlighter);
        this._hoveredHighlighter.startColor = this.hoveredColor;
        this._slectedHighlighter = Instantiate(this.Highlighter);
        this._slectedHighlighter.startColor = this.selectedColor;
        this._playerData.InputController.leftMouseDown += this.OnLeftMouseDown;

    }

    void Update()
    {
        this.HighlighteSelected();
        this.HighlighteHovered();
    }

    private void OnLeftMouseDown(MouseEventArgs e) {
        if (e.TargetObject == null) return;
        
        ControlledСharacter character;
        e.TargetObject.TryGetComponent<ControlledСharacter>(out character);
        if (character != null) {
            _playerData.SelectedCharacter = character;
            this._playerData.InputController.DropState();
        }
    }

    private void FixedUpdate() 
    {
        if (this._playerData.SelectedCamera == null) return;
        
        GameObject target = Mouse.GetTargetObject(this._playerData.SelectedCamera);
        if (target != null) {
            ControlledСharacter character;
            target.TryGetComponent<ControlledСharacter>(out character);
            this._hoveredCharacter = character;
        }
    }

    private void HighlighteHovered()
    {
        if (this._hoveredCharacter != null && this._hoveredCharacter != _playerData.SelectedCharacter) {
            this._hoveredHighlighter.transform.position = this._hoveredCharacter.transform.position;
        } else {
            this._hoveredHighlighter.transform.position = new Vector3(0, -100, 0);
        }
    }

    private void HighlighteSelected()
    {
        if (_playerData.SelectedCharacter != null) {
            this._slectedHighlighter.transform.position = _playerData.SelectedCharacter.transform.position;
        } else {
            this._slectedHighlighter.transform.position = new Vector3(0, -100, 0);
        }
    }
}
