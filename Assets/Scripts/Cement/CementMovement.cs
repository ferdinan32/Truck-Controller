using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CementEffect
{
    public class CementMovement : MonoBehaviour
    {
        private bool isGrounded = false;
        private bool isStartPour = false;
        private bool isPouring = false;
        private float minScale = 0f;
        private float maxScale = 100f;
        private float maxPour = 1f;
        private float minPour = 0f;
        private float showPour = .04f;
        private float pourSpeed = .5f;
        private float smoothTime = .5f;
        private float spreadSpeed = .05f;
        private float spreadPosY = .02f;
        private Vector3 velocityPour;
        private Vector3 defaultScale;
        private Vector3 defaultPos;
        private Vector3 hitPos;
        private Vector3 pourPos;
        private List<Transform> spreadPrefabs;
        private Transform spreadPrefab;
        private Transform pourEffect;
        private Transform pourCenterPoint;
        private Transform pourBotPoint;
        private Transform pourTopPoint;
        private GameObject liquidBlock;
        private ItemID itemID;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if(isStartPour && !isGrounded)
            {
                OnCementPour(maxScale);
            }
            else if(!isStartPour && isGrounded)
            {
                if(isPouring)
                {
                    OnCementPour(defaultScale.y);
                }
            }
        }

        void OnCementPour(float endScale)
        {
            pourEffect.localScale = new Vector3(pourEffect.localScale.x, Mathf.SmoothDamp(pourEffect.localScale.y, endScale, ref velocityPour.y, smoothTime, pourSpeed), pourEffect.localScale.z);
            pourEffect.position -= (pourEffect.up) * Time.deltaTime * pourSpeed;

            if(transform.localScale.y >= showPour)
            {
                liquidBlock.SetActive(true);
            }

            if(pourTopPoint.position.y <= (hitPos.y) && isGrounded)
            {
                SetDefault();
            }
        }

        void SetDefault()
        {
            isGrounded = false;
            isPouring = false;
            itemID = null;
            pourEffect.localPosition = defaultPos;
            pourEffect.localScale = defaultScale;
            liquidBlock.SetActive(false);
            SetCementActive(false);
        }

        void SetCementActive(bool isActive = true)
        {
            isStartPour = isActive;
            gameObject.SetActive(isActive);
        }

        void SpawnCementGround()
        {
            if(!isPouring)
            {
                spreadPrefabs.Add(Instantiate(spreadPrefab, new Vector3(hitPos.x, hitPos.y + spreadPosY, hitPos.z), Quaternion.identity));
            }
        }

        void SpreadDestroy()
        {
            for (int i = 0; i < spreadPrefabs.Count; i++)
            {
                if(spreadPrefabs[i].localScale.x <= spreadPrefab.localScale.x)
                {
                    GameObject selectedPrefab = spreadPrefabs[i].gameObject;
                    spreadPrefabs.Remove(spreadPrefabs[i]);
                    Destroy(selectedPrefab);
                }
            }
        }

        public void SetGroundedState(bool isGrounded)
        {
            this.isGrounded = isGrounded;
        }

        public void CementPour()
        {
            if(pourEffect.localScale.y <= minScale)
            {
                SetCementActive();
            }
            else
            {
                isPouring = true;
            }
        }

        public void SetCementComponent(CementComponent args)
        {
            pourCenterPoint = args.pourCenterPoint;
            pourBotPoint = args.pourBotPoint;
            pourTopPoint = args.pourTopPoint;
            liquidBlock = args.liquidBlock;
            spreadPrefab = args.spreadPrefab;

            pourEffect = this.transform;
            pourEffect.localScale = new Vector3(args.cementSize, minScale, args.cementSize);
            maxPour = minPour = args.cementSize;
            defaultPos = pourEffect.localPosition;
            defaultScale = pourEffect.localScale;
            spreadPrefabs = new List<Transform>();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if(other.tag != "Player")
            {
                maxPour += minPour;
                isGrounded = true;
                isStartPour = false;
                pourPos = pourCenterPoint.position;
                itemID = other.GetComponent<ItemID>();
                hitPos = other.transform.InverseTransformDirection(pourBotPoint.position);

                if(itemID == null)
                {
                    SpawnCementGround();
                }
            }
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerStay(Collider other)
        {
            if(other.tag != "Player")
            {
                isGrounded = true;
                isStartPour = false;

                if(itemID != null)
                {
                    if(itemID.itemTag == ItemID.Tag.cement && !isPouring)
                    {
                        itemID.transform.localScale = Vector3.Lerp(itemID.transform.localScale, Vector3.one, Time.deltaTime * spreadSpeed);
                        SpreadDestroy();
                    }
                }
                else
                {
                    if(isGrounded)
                    {
                        hitPos = other.transform.InverseTransformDirection(pourBotPoint.position);
                        SpawnCementGround();
                    }
                }
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if(other.GetComponent<ItemID>() != null)
            {
                if(other.GetComponent<ItemID>().itemTag == ItemID.Tag.cement)
                {
                    itemID = null;
                    isGrounded = false;
                    isStartPour = true;
                    // if(pourCenterPoint.position.y != pourPos.y)
                    // {
                    //     pourCenterPoint.position = new Vector3(pourCenterPoint.position.x, pourPos.y, pourCenterPoint.position.z);
                    // }
                }
            }
        }
    }

    [System.Serializable]
    public class CementComponent
    {
        public Transform spreadPrefab;
        public Transform pourCenterPoint;
        public Transform pourBotPoint;
        public Transform pourTopPoint;
        public GameObject liquidBlock;
        public float cementSize;
    }
}