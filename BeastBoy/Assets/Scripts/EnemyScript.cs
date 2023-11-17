using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _cassidy;

    private AudioSource _gunshot;
    private float time = 5;
    public int _cassHP = 100;
    public float distance;

    void Start()
    {
        _gunshot = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            _gunshot.Play();
            timer = time;
            _player.GetComponent<PlayerScript>().doDamage(5);
        }
        distance = Vector3.Distance(_cassidy.transform.position, _player.transform.position);
        if (_cassHP <= 0f)
        {
            Destroy(_cassidy);
            _player.GetComponent<PlayerScript>().win();
        }
    }
    public float getDistance()
    {
        return distance;
    }

    public void takeDamage(int x)
    {
        _cassHP -= x;
    }
    public float getEnemyHP()
    {
        return _cassHP;
    }
}
