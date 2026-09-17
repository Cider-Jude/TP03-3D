using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways] 
[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    public enum CurveType { Quadratic, Cubic, Custom }

    [Header("Type de courbe")]
    public CurveType curveType = CurveType.Quadratic;

    [Header("Points de contrôle")]
    [Tooltip("3 points pour une courbe quadratique, 4 pour une cubique (ajusté automatiquement). " +
             "En mode Custom, ajoute/retire des points librement via la taille de la liste.")]
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
        // Quadratic/Cubic : nombre de points fixe et automatique.
        // Custom : l'utilisateur gère librement la taille de la liste dans l'Inspecteur
        // (au moins 2 points nécessaires pour former une courbe).
        if (curveType == CurveType.Custom)
        {
            while (controlPoints.Count < 2) controlPoints.Add(null);
            return;
        }

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
        if (controlPoints.Count < 2 || controlPoints.Any(p => p == null)) return;

        lineRenderer.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            lineRenderer.SetPosition(i, CalculateBezierPoint(t, controlPoints));
        }
    }

    // Algorithme de De Casteljau, version RÉCURSIVE (exercice 9) : fonctionne pour
    // n'importe quel nombre n ≥ 2 de points de contrôle (3 = quadratique, 4 = cubique,
    // N = Custom), sans aucune modification.
    //
    // Équivalent mathématiquement aux formules explicites :
    //   Quadratique : B(t) = (1-t)²P0 + 2(1-t)t P1 + t²P2
    //   Cubique     : B(t) = (1-t)³P0 + 3(1-t)²t P1 + 3(1-t)t²P2 + t³P3
    Vector3 CalculateBezierPoint(float t, List<Transform> points)
    {
        List<Vector3> positions = points.Select(p => p.position).ToList();
        return CalculateBezierPointRecursive(t, positions);
    }

    // Cas de base : un seul point restant = le point de la courbe pour ce t.
    // Cas récursif : on interpole chaque paire de points consécutifs, ce qui
    // réduit la liste d'un élément, puis on rappelle la fonction sur cette
    // liste réduite jusqu'à atteindre le cas de base.
    Vector3 CalculateBezierPointRecursive(float t, List<Vector3> points)
    {
        if (points.Count == 1)
        {
            return points[0];
        }

        List<Vector3> nextLevel = new List<Vector3>(points.Count - 1);
        for (int i = 0; i < points.Count - 1; i++)
        {
            nextLevel.Add(Vector3.Lerp(points[i], points[i + 1], t));
        }

        return CalculateBezierPointRecursive(t, nextLevel);
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