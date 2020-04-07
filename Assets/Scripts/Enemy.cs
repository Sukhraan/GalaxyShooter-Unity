using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;
    //handle to animator component

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //sort of caching (handle to player) on startup
        _player = GameObject.Find("Player").GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();

        //null check the player
        if(_player == null)
		{
            Debug.LogError("The Player is NULL");
		}
        //assign the component to Anim
        _anim = GetComponent<Animator>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if bottom of screen
        //respawn at top with random x position
        if(transform.position.y < -5f)
		{
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
		}
        
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
        //Debug.Log("Hit: " + other.transform.name);

        //if other is Player
        //damage the player first
        //then destroy us
        if (other.tag == "Player")
		{
            Player player = other.transform.GetComponent<Player>();
            //null exception check just in case player(script) component gets removed/isn't there
            if(player != null)
            {
                //damage player by calling damage method on Player(Script) component via GetComponent method
                //other example other.transform.GetComponent<MeshRenderer>().material.color
                player.Damage();
            }
            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0; //animated component stop moving too
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);//destroy after 2.8 sec
            
        }


        //if other is laser
        //destroy laser
        //then destroy us
        if(other.tag == "Laser")
		{
            Destroy(other.gameObject);
            //add 10 to score
            if(_player != null)
			{
                _player.AddScore(10);
			}
            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            
        }

	}
}
