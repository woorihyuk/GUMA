using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractiveObjectChecker : MonoBehaviour
{
    private Collider2D _collider;
    private readonly List<Collider2D> _colliders = new();
    private ContactFilter2D _filter;
    public LayerMask contactLayerMask;
    private InteractiveData _lastInteractiveObject;

    public class InteractiveData
    {
        public float distance;
        public InteractiveObject interactiveObject;

        public InteractiveData(float dst, InteractiveObject iObj)
        {
            distance = dst;
            interactiveObject = iObj;
        }
    }

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _colliders.Clear();
        _filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = contactLayerMask
        };
    }

    private void Update()
    {
        var counts = _collider.OverlapCollider(_filter, _colliders);
        if (counts == 0)
        {
            _lastInteractiveObject?.interactiveObject.OnDeselect();
            _lastInteractiveObject = null;
            return;
        }
        foreach (var col in _colliders)
        {
            var iObj = col.GetComponent<InteractiveObject>();
            if (iObj.objectType == InteractiveObjectType.None) continue;

            if (_lastInteractiveObject == null)
            {
                _lastInteractiveObject = new InteractiveData(Vector2.Distance(transform.position, iObj.transform.position), iObj);
                iObj.OnSelect();
            }
            else
            {
                var dst = Vector2.Distance(transform.position, iObj.transform.position);
                if (_lastInteractiveObject.distance < dst)
                {
                    _lastInteractiveObject.distance = dst;
                    continue;
                }
                if (_lastInteractiveObject.interactiveObject != iObj)
                {
                    iObj.OnSelect();
                    _lastInteractiveObject.interactiveObject.OnDeselect();
                }
                _lastInteractiveObject = new InteractiveData(dst, iObj);
            }
        }
    }

    public bool TryGetLastInteractiveObject(out InteractiveObject iObj)
    {
        if (_lastInteractiveObject == null)
        {
            iObj = null;
            return false;
        }
        iObj = _lastInteractiveObject.interactiveObject;
        return true;
    }
}