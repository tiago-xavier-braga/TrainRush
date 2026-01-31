using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using XaviGames.Attributes;

namespace XaviGames.Animation
{
    public class SpawnAnimation : MonoBehaviour
    {
        [FormerlySerializedAs("_duration")]
        [field: SerializeField]
        public float Duration = 1.0f;

        [SerializeField]
        private LeanTweenType _leanTweenType;

        private int _id = -1;
        private Vector3 _initialScale;

        private void Start()
        {
            _initialScale = transform.localScale;
        }

        public void Animate(GameObject gameObject, Vector3 from, Vector3 to, UnityAction onFinish = null)
        {
            if (_id != -1)
            {
                LeanTween.cancel(_id);
            }

            _initialScale = from;
            gameObject.transform.localScale = _initialScale;
            
            _id = LeanTween.scale(gameObject, to, Duration)
            .setEase
            (
                _leanTweenType
            )
            .setOnComplete
            (
                () => 
                { 
                    onFinish?.Invoke(); 
                }    
            )
            .id;
        }

        [Button(true)]
        public void Spawn()
        {
            Animate(gameObject, transform.localScale, Vector3.one);
        }

        [Button(true)]
        public void Despawn()
        {
            Animate(gameObject, transform.localScale, Vector3.zero);
        }

        public void Cancel(GameObject gameObject)
        {
            LeanTween.cancel(_id);
            gameObject.transform.localScale = _initialScale;
        }
    }
}
