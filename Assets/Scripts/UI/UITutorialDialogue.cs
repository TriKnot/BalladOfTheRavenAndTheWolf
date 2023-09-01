using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UITutorialDialogue : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textComponent;
        [SerializeField] private string[] _lines;
        [SerializeField] private Image[] _images;
        [SerializeField] private float _textSpeed;
        
        [SerializeField] private Button _nextLineButton;
        [SerializeField] private Button _loadSceneButton;
        [SerializeField] private GameObject _textBox;
        [SerializeField] private GameObject _tutorialBox;
        [SerializeField] private GameObject _controlsBox;

        private int _index;

        // Start is called before the first frame update
        void Start()
        {
            _textComponent.text = string.Empty;
            StartDialogue();
            _nextLineButton.gameObject.SetActive(true);
            _nextLineButton.Select();
            _loadSceneButton.gameObject.SetActive(false);
            //_tutorialBox.gameObject.SetActive(false);
            _controlsBox.gameObject.SetActive(false);
        }


        void StartDialogue()
        {
            _index = 0;
            StartCoroutine(TypeLine());
        }

        IEnumerator TypeLine()
        {
            foreach (char c in _lines[_index].ToCharArray())
            {
                _textComponent.text += c;
                FMODUnity.RuntimeManager.PlayOneShot("event:/MenuSounds/TypeSound");
                yield return new WaitForSeconds(_textSpeed);
            }
        }

        void NextPanel()
        {
            if (_index < _lines.Length - 1)
            {
                _index++;
                _textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else
            {
                _nextLineButton.gameObject.SetActive(false);
                _tutorialBox.gameObject.SetActive(false);
                _loadSceneButton.gameObject.SetActive(true);
                _controlsBox.gameObject.SetActive(true);
                _loadSceneButton.Select();
            }

            if (_index <= _images.Length - 1)
            {
                foreach (var image in _images)
                {
                    image.gameObject.SetActive(false);
                }
                _images[_index].gameObject.SetActive(true);
            }
            else
            {
                _nextLineButton.gameObject.SetActive(false);
                _tutorialBox.gameObject.SetActive(false);
                _loadSceneButton.gameObject.SetActive(true);
                _controlsBox.gameObject.SetActive(true);
                _loadSceneButton.Select();
            }
        }

        public void NexLine()
        {
            if (_textComponent.text == _lines[_index])
            {
                NextPanel();
            }
            else
            {
                StopAllCoroutines();
                _textComponent.text = _lines[_index];
            }
        }
    
        public void LoadNewScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
