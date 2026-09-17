using UnityEngine;

[ExecuteAlways]
public class LightingBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [Header("Variables")]
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private float lightMaxIntensity = 5;

    [Header("Time settings")]
    [SerializeField] private float dayDurationInSeconds = 120f; // Durée réelle d'un cycle complet de 24h virtuelles (ex: 120s = 2 minutes)
    [SerializeField] private float fastForwardMultiplier = 5f;  // Multiplicateur appliqué pendant l'accéléré
    [SerializeField] private KeyCode fastForwardKey = KeyCode.LeftShift;

    private void Update()
    {
        if (Preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            float timeSpeed = 24f / Mathf.Max(dayDurationInSeconds, 0.01f);

            if (Input.GetKey(fastForwardKey))
            {
                timeSpeed *= fastForwardMultiplier;
            }

            TimeOfDay += timeSpeed * Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90f, -170f, 0f));

            // Facteur d'intensité basé sur la hauteur du "soleil" dans le ciel :
            // quand la lumière pointe vers le bas (forward.y négatif), le soleil est haut -> intensité max.
            // quand elle pointe vers le haut (sous l'horizon), c'est la nuit -> intensité nulle.
            float intensityFactor = Mathf.Clamp01(Vector3.Dot(DirectionalLight.transform.forward, Vector3.down));
            DirectionalLight.intensity = lightMaxIntensity * intensityFactor;
        }

    }

    //To try and find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
        {
            return;
        }
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsByType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}