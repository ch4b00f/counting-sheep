using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneHandler : MonoBehaviour
{
    private UIDocument _root;

    private VisualElement _upper;
    private VisualElement _lower;

    public static UnityEvent OnSleepClicked = new UnityEvent();
    public static UnityEvent<string> OnPlayClicked = new UnityEvent<string>();

    private void Awake()
    {
        _root = GetComponent<UIDocument>();

        _upper = _root.rootVisualElement.Q("upper");
        _lower = _root.rootVisualElement.Q("lower");

        // make this different later
        OnSleepClicked.AddListener(Quit);
        OnPlayClicked.AddListener(LoadScene);
    }

    private void Start()
    {
        StartCoroutine(OpenCurtain());
    }

    private void LoadScene(string scene)
    {
        StartCoroutine(CloseCurtain(scene));
    }

    private void Quit()
    {
        StartCoroutine(CloseCurtain());
    }

    private IEnumerator CloseCurtain(string scene)
    {
        _upper.RemoveFromClassList("upper-open");
        _lower.RemoveFromClassList("lower-open");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator CloseCurtain()
    {
        _upper.RemoveFromClassList("upper-open");
        _lower.RemoveFromClassList("lower-open");
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    private IEnumerator OpenCurtain()
    {
        yield return new WaitForSeconds(.5f);
        _upper.AddToClassList("upper-open");
        _lower.AddToClassList("lower-open");
    }
}
