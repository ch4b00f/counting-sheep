using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument _root;

    private Button _button;

    private void Start()
    {
        _root = GetComponent<UIDocument>();

        _button = _root.rootVisualElement.Q<Button>("button");

        _button.clicked += () => Play();
    }

    private void Play()
    {
        SceneHandler.OnPlayClicked.Invoke("Level");
    }
}
