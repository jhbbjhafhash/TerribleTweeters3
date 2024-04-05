using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 3;
    Vector2 _startPostiion;
    Rigidbody2D _rigidbody2D;

    SpriteRenderer _spriteRenderer;

    public bool IsDragging { get; private set; }

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPostiion =  _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
    }

    void OnMouseDown()
    {
         _spriteRenderer.color = Color.red;
         IsDragging = true;
    }

    void OnMouseUp()
    {
        Vector2 currentPosition =  _rigidbody2D.position;
        Vector2 direction = _startPostiion - currentPosition;
        direction.Normalize();

         _rigidbody2D.isKinematic = false;
         _rigidbody2D.AddForce(direction * _launchForce);

        _spriteRenderer.color = Color.white;
        IsDragging = false;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 desiredPosition = mousePosition;  

        float distance = Vector2.Distance(desiredPosition, _startPostiion);
        if (distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPostiion;
            direction.Normalize();
            desiredPosition = _startPostiion + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPostiion.x) 
            desiredPosition.x = _startPostiion.x;

        _rigidbody2D.position = desiredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
       yield return new WaitForSeconds(3);
        _rigidbody2D.position = _startPostiion;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
