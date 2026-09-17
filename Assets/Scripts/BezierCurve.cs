using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways] // Permet de voir la courbe se mettre à jour en temps réel dans l'éditeur, sans lancer le jeu
[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    public enum CurveType { Quadratic, Cubic }

    [Header("Type de courbe")]
    public CurveType curveType = CurveType.Quadratic;

    [Header("Points de contrôle")]
    [Tooltip("3 points pour une courbe quadratique, 4 pour une cubique (ajusté automatiquement)")]
    public List<Transform> controlPoints = new List<Transform>();

    [Header("Rendu")]
    [Range(2, 200)]
    public int resolution = 50; // Nombre de segments : plus c'est élevé, plus la courbe est lisse
    public bool showControlPolygon = true;

    private LineRenderer lineRenderer;

    void Reset()
    {
        // Config par défaut pratique quand on ajoute le script pour la première fois
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.widthMultiplier = 0.1f;
    }

    void OnValidate()
    {
        // Garde automatiquement le bon nombre de slots selon le type de courbe choisi
        int required = curveType == CurveType.Quadratic ? 3 : 4;
        while (controlPoints.Count < required) controlPoints.Add(null);
        while (controlPoints.Count > required) controlPoints.RemoveAt(controlPoints.Count - 1);
    }

    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (controlPoints.Count == 0 || controlPoints.Any(p => p == null)) return;

        lineRenderer.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            lineRenderer.SetPosition(i, CalculateBezierPoint(t, controlPoints));
        }
    }

    // Algorithme de De Casteljau : interpolation linéaire répétée entre points
    // consécutifs jusqu'à n'en obtenir plus qu'un seul. Fonctionne pour n'importe
    // quel nombre de points de contrôle (3 = quadratique, 4 = cubique, etc.)
    //
    // Équivalent mathématiquement aux formules explicites :
    //   Quadratique : B(t) = (1-t)²P0 + 2(1-t)t P1 + t²P2
    //   Cubique     : B(t) = (1-t)³P0 + 3(1-t)²t P1 + 3(1-t)t²P2 + t³P3
    Vector3 CalculateBezierPoint(float t, List<Transform> points)
    {
        List<Vector3> temp = points.Select(p => p.position).ToList();

        int count = temp.Count;
        while (count > 1)
        {
            for (int i = 0; i < count - 1; i++)
            {
                temp[i] = Vector3.Lerp(temp[i], temp[i + 1], t);
            }
            count--;
        }

        return temp[0];
    }

    void OnDrawGizmos()
    {
        if (controlPoints.Count == 0 || controlPoints.Any(p => p == null)) return;

        Gizmos.color = Color.red;
        foreach (Transform p in controlPoints)
        {
            Gizmos.DrawSphere(p.position, 0.15f);
        }

        if (showControlPolygon)
        {
            Gizmos.color = Color.gray;
            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
            }
        }
    }
}