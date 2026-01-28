using UnityEngine;
using XaviGames.Attributes;

namespace XaviGames.Interaction
{
    public class QueuePosition : MonoBehaviour
    {
        [field: SerializeField]
        [field: ReadOnly]
        public GameObject OccupiedEntity { get; private set; }

        public bool IsOccupied { get; private set; }

        private float _minDistance = 0.1f;

        public void SetOccupiedEntity(GameObject entity)
        {
            if (entity == null)
            {
                ClearOccupiedEntity();
                return;
            }

            OccupiedEntity = entity;
            IsOccupied = true;
        }

        public void ClearOccupiedEntity()
        {
            OccupiedEntity = null;
            IsOccupied = false;
        }

        public bool IsEntityAtPosition()
        {
            float distance = Vector3.Distance(transform.position, OccupiedEntity.transform.position);
            return distance < _minDistance;
        }
    }
}
