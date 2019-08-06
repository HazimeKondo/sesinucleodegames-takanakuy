using System;
using System.Collections;
using System.Linq;
using OverlapColliders;
using UnityEngine;
using UnityEngine.Serialization;

public class WhipBehaviour : MonoBehaviour {
    [SerializeField] private float _maxRange = 6f;
    [SerializeField] private float _halfThickness = 0.5f;
    [SerializeField] private bool _isChargeAttack;
    [SerializeField] private float _maxChargeTime = 2f;
    [SerializeField] private GameObject _whip;
    [SerializeField] private float _chargeTime;
    private bool charge;

    private void Start() {
        Player.Input.Keyboard.Attack.performed += ctx => { Attack(); };
        Player.Input.Keyboard.Attack.canceled += ctx => { EndCharge(); }; ;

        if (!_isChargeAttack) {
            _whip.transform.localPosition = (Vector3.forward * (_maxRange / 2f - transform.localScale.x / 2f));
            _whip.transform.localScale = new Vector3(1f, 1f, _maxRange);
        }
    }


    [Button]
    private void Attack() {
        if (!_isChargeAttack) {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, _halfThickness,
            _whip.transform.position - transform.position, out hit, transform.localScale.x / 2 + _maxRange)) {
                if (hit.transform.GetComponent<CrowdBehaviour>() != null) {
                    hit.transform.GetComponent<CrowdBehaviour>().Hit();
                }
            }

        } else {
            StartCoroutine(ChargeAttack());
        }
    }

    private void EndCharge() {
        charge = false;
    }
    private IEnumerator ChargeAttack() {
        charge = true;
        _chargeTime = 0;
        while (charge) {
            if (_chargeTime < _maxChargeTime) {
                _chargeTime += Time.deltaTime;
            }
            if (_chargeTime > _maxChargeTime) {
                _chargeTime = _maxChargeTime;
            }
            _whip.transform.localPosition = (Vector3.forward * (_maxRange * (_chargeTime / _maxChargeTime) / 2f - transform.localScale.x / 2f));
            _whip.transform.localScale = new Vector3(1f, 1f, _maxRange * (_chargeTime / _maxChargeTime));

            yield return null;
        }
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _halfThickness,
            _whip.transform.position - transform.position, out hit, transform.localScale.x / 2 + _maxRange * (_chargeTime / _maxChargeTime))) {
            hit.transform.GetComponent<CrowdBehaviour>().Hit();
        }
        _whip.transform.localScale = new Vector3(1f, 0f, 0f);
    }

    private void OnDestroy()
    {
        Player.Input.Keyboard.Attack.performed -= ctx => { Attack(); };
        Player.Input.Keyboard.Attack.canceled -= ctx => { EndCharge(); }; ;
    }
}
