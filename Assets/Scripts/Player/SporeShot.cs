using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SporeShot : MonoBehaviour
{
    [SerializeField] PlayerController player = null;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] PlayerAnim anim;

    [SerializeField] float shootDelay = 0.5f;
    [SerializeField] float spawnDistance = 0.75f;
    [SerializeField] float recoil = 5;
    [SerializeField] float shootAnimationLock = 0.2f;

    private bool canShoot = true;
    private bool hasShot = true;

    // Update is called once per frame
    void Update()
    {
        if (player.Grounded())
            hasShot = true;

        if (Input.GetKeyDown(KeyCode.Z) && canShoot && hasShot && !player.InputsFrozen())
        {
            AudioManager.Instance.PlaySFXClip("sfx_player_sporeShot", 0.5f);

            Vector2 offset = Vector2.zero;
            Quaternion rotation = Quaternion.identity;

            PlayerController.Direction direction = player.GetFacingDirection();

            switch (direction)
            {
                case PlayerController.Direction.Left:
                    offset = Vector2.left * spawnDistance;
                    rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case PlayerController.Direction.Right:
                    offset = Vector2.right * spawnDistance;
                    rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case PlayerController.Direction.Up:
                    offset = Vector2.up * spawnDistance;
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case PlayerController.Direction.Down:
                    offset = Vector2.down * spawnDistance;
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }

            switch (direction)
            {
                case PlayerController.Direction.Left: anim.SetState(PlayerAnim.States.SporeSide); break;
                case PlayerController.Direction.Right: anim.SetState(PlayerAnim.States.SporeSide); break;
                case PlayerController.Direction.Up: anim.SetState(PlayerAnim.States.SporeUp); break;
                case PlayerController.Direction.Down: anim.SetState(PlayerAnim.States.SporeDown); break;
            }

            StartCoroutine(ShootAnimationDelay());
            GameObject shot = Instantiate(hitEffect, transform.position + (Vector3)offset, quaternion.identity, gameObject.transform);
            shot.transform.rotation = rotation;

            if (!player.Grounded())
                player.Blast(-offset);
            else if (direction.Equals(PlayerController.Direction.Left))
                player.AddVelocity(Vector2.right * recoil);
            else if(direction.Equals(PlayerController.Direction.Right))
                player.AddVelocity(Vector2.left * recoil);

            hasShot = false;
            StartCoroutine(SpawnDelay());
        }
            
    }

    IEnumerator SpawnDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    IEnumerator ShootAnimationDelay()
    {
        anim.SetLock(true);
        yield return new WaitForSeconds(shootAnimationLock);
        anim.SetLock(false);
    }
}
