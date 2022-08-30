using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Palette _palette;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    public void Click(GameObject value)
    {
        PlayParticles(value.transform.position);
    }

    public void Click(Vector2 value)
    {
        PlayParticles(value);
    }

    private void PlayParticles(Vector2 position)
    {
        //Color color;
        //Material material;
        Material material = _palette.GetColor().Material;
        Color color = _palette.GetColor().Color;
        _particleSystem.transform.position = position;
        var mainPS = _particleSystem.main;
        mainPS.startColor = color;
        _particleSystem.GetComponent<Renderer>().material = material;
        _particleSystem.Play();
        //Debug.Log($"Color = {color}");
    }
}
