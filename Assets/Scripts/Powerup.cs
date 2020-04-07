using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID for Powerups
    [SerializeField]
    private int powerupID;//0 = triple Shot,1 = Speed,2 = Shields

    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3 (adjust in inspector)
        //when we leave the screen, destroy this object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

    }

    //OnTriggerCollision
    //Only be collectable by the Player (via using tags)
    //on collected,destroy as in  be collected by the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
		{
            //communicate with the player script
            //handle to the component i want
            //assign the handle to the component
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if(player != null)
			{
				switch (powerupID)
				{
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }

			}

            Destroy(this.gameObject);
		}

    }
}