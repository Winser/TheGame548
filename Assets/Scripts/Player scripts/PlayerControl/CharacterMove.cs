using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    private PlayerData _playerData;

    private LineRenderer _pathLine;
    void Start()
    {
        this._playerData = GetComponent<PlayerData>();
        this._pathLine = new GameObject("PathLine").AddComponent<LineRenderer>();
        this._playerData.InputController.leftMouseDown += this.OnLeftMouseDown;
    }

    void Update()
    {
        if (_playerData.SelectedCharacter == null) return;

        MovableCharacter character;
        _playerData.SelectedCharacter.TryGetComponent<MovableCharacter>(out character);
        if (character) {
            Vector3? mouseTargetPosition = Mouse.GetTargetPoint(this._playerData.SelectedCamera);
            if (mouseTargetPosition != null) {
                DrawPath(character, (Vector3)mouseTargetPosition);
            }
        }
    }

    private void OnLeftMouseDown(MouseEventArgs e)
    {
        if (_playerData.SelectedCharacter == null) return;

        MovableCharacter character;
        _playerData.SelectedCharacter.TryGetComponent<MovableCharacter>(out character);
        if (character && e.TargetObject.tag == "Tera") {
            character.Move((Vector3)e.TargetPoint);
        }
    }

    private void DrawPath(MovableCharacter character, Vector3 mouseTargetPosition)
    {
        List<Vector3> path = character.getPathTo(mouseTargetPosition);
        if (path != null && path.Count > 0) {
            path.Insert(0, character.transform.position);
            this._pathLine.positionCount = path.Count;
            this._pathLine.SetPositions(path.ToArray());
            this._pathLine.enabled = true;
        } else {
            this._pathLine.enabled = false;
        }
    }
}
