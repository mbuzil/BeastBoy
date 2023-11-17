using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
	[SerializeField] float _mouseSensitivity;
    [SerializeField] float _moveSpeed;
    [SerializeField] Camera _cam;
    [SerializeField] Camera _cam2;
    [SerializeField] Camera _cam3;
    [SerializeField] Camera _cam4;
    [SerializeField] Camera _cam5;
    [SerializeField] Camera _cam6;
    [SerializeField] Camera _cam7;
    [SerializeField] Transform playerTransform;
    [SerializeField] float _jumpStrength;
    [SerializeField] float _jumpDuration;
    [SerializeField] float gravity;

    [SerializeField] GameObject BigPlayer;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _trex;
    [SerializeField] GameObject _spider;
    [SerializeField] GameObject _bird;
    [SerializeField] GameObject _cheetah;
    [SerializeField] GameObject _monke;
    [SerializeField] GameObject _bear;

    [SerializeField] TextMeshProUGUI _HPText;
    [SerializeField] TextMeshProUGUI _enemyHP;
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _lose;

    [SerializeField] int _humanHp;
    [SerializeField] int _trexHp;
    [SerializeField] int _spiderHp;
    [SerializeField] int _birdHp;
    [SerializeField] int _cheetahHp;
    [SerializeField] int _monkeHp;
    [SerializeField] int _bearHp;
    [SerializeField] int _HP;

    [SerializeField] GameObject _enemy;

    float _currentTilt = 0f;
    private bool jumping;
    private float verticalMomentum;
    bool ishuman = true;
    bool isTrex = false;
    bool isBird = false;
    bool isSpider = false;
    bool isCheetah = false;
    bool isMonke = false;
    bool isBear = false;
    int strength = 5;
    float maxRange = 2;

    private AudioSource _hitMarker;
    int rand;

    public void doDamage(int x)
    {
        rand = Random.Range(0,100);
        if(isBird)
        {
            if(rand > 80)
            {
                _HP -= x;
                _birdHp -= x;
            }
        }
        else if(isSpider)
        {
            if(rand > 75)
            {
                _HP -= x;
                _spiderHp -= x;
            }
        }
        else
            _HP -= x;
        if(ishuman)
            _humanHp -= x;
        else if(isTrex)
            _trexHp -= x;
        else if(isCheetah)
            _cheetahHp -= x;
        else if(isMonke)
            _monkeHp -= x;
        else if(isBear)
            _bearHp -= x;
    }

    private bool grounded
    {
        get {
            Ray ray = new Ray(playerTransform.position + new Vector3(0, 0.05f, 0), Vector3.down);
            return Physics.Raycast(ray, 1.2f);
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _hitMarker = GetComponent<AudioSource>();
    }

    void Update()
    {
        _HPText.text = "HP: " + _HP;
        _enemyHP.text = "Enemy HP: " + _enemy.GetComponent<EnemyScript>().getEnemyHP();

        if(_HP <= 0)
            lose();

        Aim();
        
        Movement();
        GetComponent<CharacterController>().Move(transform.up * verticalMomentum * Time.deltaTime);
        if(!jumping)
        {
            if (grounded)
            {
                if(Input.GetButtonDown("Jump"))
                {
                    StartCoroutine(doJump());
                }
                else
                {
                    verticalMomentum = 0;
                }
            }
            else
            {
                verticalMomentum -= gravity * Time.deltaTime;
            }
        }
        if(isBird)
        {
            if(Input.GetButtonDown("Jump"))
            {
                verticalMomentum = 5;
            }
            if(Input.GetKeyDown("left ctrl"))
            {
                verticalMomentum = -5;
            }
        }

        if(ishuman)
        {
            isTrex = false;
            isBird = false;
            isSpider = false;
            isCheetah = false;
            isBear = false;
            isMonke = false;
            maxRange = 2;
        }
        if(Input.GetKeyDown("1"))
        {
            trex();
        }
        if(Input.GetKeyDown("2"))
        {
            spider();
        }
        if(Input.GetKeyDown("3"))
        {
            bird();
        }
        if(Input.GetKeyDown("4"))
        {
            cheetah();
        }
        if(Input.GetKeyDown("5"))
        {
            monke();
        }
        if(Input.GetKeyDown("6"))
        {
            bear();
        }

        if(isSpider)
        {
            _jumpDuration = 0.3f;
            _jumpStrength = 10.0f;
        }
        else
        {
            _jumpDuration = 0.225f;
            _jumpStrength = 6.0f;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(_enemy.GetComponent<EnemyScript>().getDistance());
            if(_enemy.GetComponent<EnemyScript>().getDistance() <= maxRange)
            {
                _hitMarker.Play();
                _enemy.GetComponent<EnemyScript>().takeDamage(strength);
            }
        }

    }
    private IEnumerator doJump()
    {
        jumping = true;
        float startTime = Time.time;
        float endTime = startTime + _jumpDuration;

        while (Time.time < endTime)
        {
            verticalMomentum += _jumpStrength * Time.deltaTime;
            yield return null;
        }
        jumping = false;
    }
    void Aim()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

       
        transform.Rotate(Vector3.up, mouseX * _mouseSensitivity);
        
        _currentTilt -= mouseY * _mouseSensitivity;
        _currentTilt = Mathf.Clamp(_currentTilt, -90, 90);
        _cam.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
        _cam2.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
        _cam3.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
        _cam4.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
        _cam5.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
        _cam6.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
        _cam7.transform.localEulerAngles = new Vector3(_currentTilt, 0, 0);
    }

    void Movement()
    {
        Vector3 sidewaysMovementVector = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardBackwardMovementVector = transform.forward * Input.GetAxis("Vertical");
        Vector3 movementVector = sidewaysMovementVector + forwardBackwardMovementVector;

        GetComponent<CharacterController>().Move(movementVector * _moveSpeed * Time.deltaTime);
    }

    private void trex()
    {
        if(ishuman)
            {
                isTrex = true;
                ishuman = false;
                _trex.SetActive(true);
                _player.SetActive(false);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 30;
                _HP = _trexHp;
                maxRange = 10f;
            }
            else
            {
                ishuman = true;
                _trex.SetActive(false);
                _player.SetActive(true);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 5;
                _HP = _humanHp;
            }
    }
    private void spider()
    {
        if(ishuman)
            {
                ishuman = false;
                _trex.SetActive(false);
                _player.SetActive(false);
                _spider.SetActive(true);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                isSpider = true;
                strength = 1;
                _moveSpeed = 8;
                _HP = _spiderHp;
                maxRange = 1.5f;
            }
            else
            {
                ishuman = true;
                _trex.SetActive(false);
                _player.SetActive(true);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 5;
                _moveSpeed = 5;
                _HP = _humanHp;
            }
    }
    private void bird()
    {
        if(ishuman)
            {
                ishuman = false;
                _trex.SetActive(false);
                _player.SetActive(false);
                _spider.SetActive(false);
                _bird.SetActive(true);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                isBird = true;
                strength = 2;
                _moveSpeed = 7;
                _HP = _birdHp;
                maxRange = 1.8f;
            }
            else
            {
                ishuman = true;
                _trex.SetActive(false);
                _player.SetActive(true);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                isBird = false;
                strength = 5;
                _moveSpeed = 5;
                _HP = _humanHp;
            }
    }
    private void cheetah()
    {
        if(ishuman)
            {
                isCheetah = true;
                ishuman = false;
                _trex.SetActive(false);
                _player.SetActive(false);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(true);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 8;
                _moveSpeed = 12;
                _HP = _cheetahHp;
                maxRange = 2.5f;
            }
            else
            {
                ishuman = true;
                _trex.SetActive(false);
                _player.SetActive(true);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 5;
                _moveSpeed = 5;
                _HP = _humanHp;
            }
    }
    private void monke()
    {
        if(ishuman)
            {
                isMonke = true;
                ishuman = false;
                _trex.SetActive(false);
                _player.SetActive(false);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(true);
                _bear.SetActive(false);
                strength = 13;
                _moveSpeed = 6;
                _HP = _monkeHp;
                maxRange = 3.5f;
            }
            else
            {
                ishuman = true;
                _trex.SetActive(false);
                _player.SetActive(true);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 5;
                _moveSpeed = 5;
                _HP = _humanHp;
            }
    }
    private void bear()
    {
        if(ishuman)
            {
                isBear = true;
                ishuman = false;
                _trex.SetActive(false);
                _player.SetActive(false);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(true);
                strength = 20;
                _moveSpeed = 9;
                _HP = _bearHp;
                maxRange = 3;
            }
            else
            {
                ishuman = true;
                _trex.SetActive(false);
                _player.SetActive(true);
                _spider.SetActive(false);
                _bird.SetActive(false);
                _cheetah.SetActive(false);
                _monke.SetActive(false);
                _bear.SetActive(false);
                strength = 5;
                _moveSpeed = 5;
                _HP = _humanHp;
            }
    }
    public void win()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(2);
    }
    public void lose()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(1);
    }
    
}