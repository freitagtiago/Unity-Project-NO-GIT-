using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RPG.Movement
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Tilemap map;

        Vector3 bottomLeftLimit;
        Vector3 topRightLimit;

        float halfHeight;
        float halfWidth;

        public int musicToPlay;
        bool musicStarted = false;

        private Mover mover = null;


        private void Start()
        {
            mover = GameObject.FindGameObjectWithTag("Player").GetComponent<Mover>();
            if(mover == null)
            {
                Debug.LogError("Mover is null");
            }
            else
            {
                target = mover.GetComponent<Transform>();
            }

            SetMapLimits();
        }

        private void LateUpdate()
        {
            if (!musicStarted)
            {
                musicStarted = true;
                AudioController.instance.PlayBackgroundMusic(musicToPlay);
            }
            if (target == null) return;

            transform.position = new Vector3(Mathf.Clamp(target.position.x, bottomLeftLimit.x, topRightLimit.x)
                                            , Mathf.Clamp(target.position.y, bottomLeftLimit.y, topRightLimit.y)
                                            , transform.position.z);

        }

        private void SetMapLimits()
        {
            halfHeight = Camera.main.orthographicSize;
            halfWidth = halfHeight * Camera.main.aspect;

            bottomLeftLimit = map.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
            topRightLimit = map.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0);

            mover.SetBounds(map.localBounds.min, map.localBounds.max);
        }
    }
}

