using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private AnimationCurve curve;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    public void FadeTo(object s, GameEvent.SceneChangeEvent e)
    {
        StartCoroutine(FadeOut(e.SceneName));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f,0f,0f,a);
            yield return null;
        }
    }



    private IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }

    private void OnEnable()
    {
        EventHelper.SceneChangedEvent += FadeTo;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EventHelper.SceneChangedEvent -= FadeTo;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
