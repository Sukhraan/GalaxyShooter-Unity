using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public variable can be seen in inspector window for gameobject too
    //best practice keep data private
    //if we want designers to play with the speed without changing code -> solution will be concept of attribute
    //add attr [SerializeField] above private variable to make it visible in inspector window
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f; // can fire every 0.5 second i.e cool down system
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;

    //variable refernece to the shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;


    [SerializeField]
    private int _score;
    private UIManager _uiManager;

    //variable to store the audio clip
    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {

        // take the current position = new position (0,0,0)

        transform.position = new Vector3(0,0,0);


        //findinging the GameObject "Spawn_Manager" via find method and getting the component SpawnManager(script)
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();

        //null exception handling
        if (_spawnManager == null)
		{
            Debug.LogError("The Spawn Manager is NULL.");
		}

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL.");
        }
		else
		{
            _audioSource.clip = _laserSoundClip;
		}

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)  //Time.time = how long the game has been running
        {
            FireLaser();
        }


    }

    void CalculateMovement()
	{
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //new Vector3(1, 0, 0) 1 unit/m to right per frame, Update fn called 60 frames per sec
        //new Vector3(-5,0,0) * real time == Vector3.left * 5 * Time.deltaTime


        //transform.Translate(Vector3.right * horizontalInput *_speed * Time.deltaTime); // deltaTime converts the movement to realtime seconds rather then frames, thus *Time.deltaTime => 1 m per sec
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);  //more optimized as just one new operator used
      

        // if player position on the y is greater than 0
        //y position = 0, keeping x the same
        //else if position on th y is less than -3.8f
        //y pos = -3.8f

        //if (transform.position.y >= 0)  //Clamping the movement for y position between -3.8f and 0
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        // Alternate for clamping position y between -3.8f and 0

        transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y,-3.8f,0),0);


        //if player on the x > 11
        //x pos = -11
        //else if player on the x is less than -11
        //x pos = 11

        if (transform.position.x > 11.3f) // wrapping around the movement for x axis
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }


    }

    void FireLaser()
	{
        // if i hit the space key
        //spawn gameObject
   
         _canFire = Time.time + _fireRate; // cool down system, so player can fire only afer 0.5 seconds interval

         //Debug.Log("Space Key Pressed");
         //Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
         // spawn obect at 0.8 offset above to player's position with default prefab/ no rotation


        if(_isTripleShotActive == true)
		{
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        }
        else
		{
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);

        }

        //if space key press
        //if tripleshotActive is true
        //fire 3 lasers (triple shot prefab)

        //else fire 1 laser


        //play the laser audio clip
        _audioSource.Play();

    }

    public void Damage()
	{
        //if shields is active
        //do nothing....
        //deactivate shields
        //return;

        if(_isShieldsActive == true)
		{
            _isShieldsActive = false;
            //disable the shield visualizer
            _shieldVisualizer.SetActive(false);
            return;
		}


        _lives--;
        //if lives is 2
        //enable left engine
        //else if lives is 1
        //enable right engine
        if(_lives == 2)
		{
            _leftEngine.SetActive(true);
		}
        else if(_lives == 1)
		{
            _rightEngine.SetActive(true);
		}


        _uiManager.UpdateLives(_lives);

        //check if dead
        //destroy us

        if(_lives < 1)
		{
            //Communicate with Spawn Manager
            //i.e. find the GameObject. then get component/ public method
            //done in start()
            //Let them know to stop spawning
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
		}
	}

    public void TripleShotActive()
	{
        //tripleShotActive becomes true
        _isTripleShotActive = true;

        //start the power down coroutine for triple shot
        StartCoroutine(TripleShotPowerDownRoutine());
	}

    //IEnumerator TripleShotPowerDownRoutine
        //wait 5 seconds
        //set the triple shot to false

    IEnumerator TripleShotPowerDownRoutine()
	{
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
	}

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        //enable the shield visualizer
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
	{
        _score += points;
        _uiManager.UpdatScore(_score);
	}
    //method to add 10 to the score
    //Communicate with the UI to update the score!
}
