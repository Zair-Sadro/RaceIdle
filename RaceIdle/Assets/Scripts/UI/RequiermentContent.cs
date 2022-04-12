using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiermentContent : MonoBehaviour
{
    [SerializeField] private RequiermentTile _requiermentPrefab;

    private BaseMachineTool _machine;
    private List<RequiermentTile> _createdRequierments = new List<RequiermentTile>();

    public void Init(BaseMachineTool m)
    {
        _machine = m;
    }

    private void OnEnable()
    {
        CreateRequierments();
    }

    private void OnDisable()
    {
        ClearRequierments();
    }

    public void CreateRequierments()
    {
        var requierments = _machine.MachineData.GetNextMachineLevel(_machine.CurrentActiveMachine.Level).Requierments;
        
        for (int i = 0; i < requierments.Count; i++)
        {
            var newReq = Instantiate(_requiermentPrefab, this.transform);
            newReq.Init(requierments[i], this);
            _createdRequierments.Add(newReq);
        }
    }

    public void ClearRequierments()
    {
        if (_createdRequierments.Count <= 0)
            return;

        for (int i = 0; i < _createdRequierments.Count; i++)
            Destroy(_createdRequierments[i].gameObject);

        _createdRequierments.Clear();
    }
}
