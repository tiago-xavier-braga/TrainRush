using System.Collections.Generic;
using UnityEngine;
using XaviGames.Animation;
using XaviGames.Attributes;

namespace XaviGames.Characters
{
    public class CharacterSpawnController : MonoBehaviour
    {
        [SerializeField]
        private int _maxCharacterSpawn;

        [SerializeField]
        private List<GameObject> _characterPrefabs;

        [Header("Spawn Settings")]
        [SerializeField]
        private Transform _spawnTransform;

        [SerializeField]
        private Vector3 _spawnSize;

        [Space(8f)]
        [Header("List Info")]
        [SerializeField]
        [ReadOnly]
        private List<GameObject> _charactersSpawned;

        [SerializeField]
        private List<GameObject> _charactersActivated;

        [SerializeField]
        private List<GameObject> _charactersDisabled;

        private void Start()
        {
            SpawnCharacter();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_spawnTransform.position, _spawnSize);
        }

        [Button]
        public GameObject ActivateCharacter()
        {
            if (_charactersDisabled.Count == 0)
            {
                return null;
            }

            int indexRandom = Random.Range(0, _charactersDisabled.Count - 1);
            GameObject character = _charactersDisabled[indexRandom];

            CharacterManager characterManager = character.GetComponent<CharacterManager>();

            Vector3 position = CalculateActivatePosition();
            character.transform.position = position;
            character.transform.rotation = _spawnTransform.rotation;

            _charactersDisabled.Remove(character);
            _charactersActivated.Add(character);

            SpawnAnimation spawnAnimation = characterManager.SpawnAnimation;
            spawnAnimation.Animate
            (
                character,
                Vector3.zero,
                Vector3.one
            );

            character.SetActive(true);
            return character;
        }

        public void DisableCharacter(GameObject character)
        {
            CharacterManager characterManager = character.GetComponent<CharacterManager>();
            SpawnAnimation spawnAnimation = characterManager.SpawnAnimation;
            spawnAnimation.Animate
            (
                character,
                character.transform.localScale,
                Vector3.zero,
                () =>
                {
                    character.SetActive(false);
                }
            );

            _charactersDisabled.Add(character);
            _charactersActivated.Remove(character);
        }

        private void SpawnCharacter()
        {
            DestroyAllCharacters();

            for (int count = 0; count < _maxCharacterSpawn; count++)
            {
                int index = Random.Range(0, _characterPrefabs.Count - 1);
                GameObject gameObject = Instantiate(_characterPrefabs[index], transform);

                gameObject.SetActive(false);
                _charactersSpawned.Add(gameObject);
            }

            _charactersDisabled.Clear();
            _charactersSpawned.ForEach(character => _charactersDisabled.Add(character));
        }

        private void DestroyAllCharacters()
        {
            foreach (GameObject character in _charactersSpawned)
            {
                Destroy(character);
            }

            _charactersSpawned.Clear();
        }

        private Vector3 CalculateActivatePosition()
        {
            Vector3 center = _spawnTransform.position;

            float xDistanceUp = center.x + (_spawnSize.x / 2);
            float xDistanceDown = center.x - (_spawnSize.x / 2);

            float zDistanceUp = center.z + (_spawnSize.z / 2);
            float zDistanceDown = center.z - (_spawnSize.z / 2);

            float xRandom = Random.Range(xDistanceDown, xDistanceUp);
            float zRandom = Random.Range(zDistanceDown, zDistanceUp);

            return new Vector3(xRandom, center.y, zRandom);
        }
    }
}