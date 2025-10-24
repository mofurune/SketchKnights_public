using System;
using System.Collections.Generic;
using UnityEngine;

public static class MeshExtensions
{
    /// <summary>
    /// Z=0 を開始キャップ、Z=1 を終了キャップとみなし、
    /// 指定に応じて三角形を削除します。
    /// </summary>
    public static void StripCaps(
        this Mesh mesh,
        bool keepStartCap,
        bool keepEndCap,
        float epsilon = 1e-4f)
    {
        // 頂点情報取得
        var vertices = mesh.vertices;
        var triangles = mesh.triangles;

        List<int> kept = new List<int>(triangles.Length);

        bool IsStart(float z) => Mathf.Abs(z) < epsilon;
        bool IsEnd(float z)   => Mathf.Abs(z - 1f) < epsilon;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int ia = triangles[i];
            int ib = triangles[i + 1];
            int ic = triangles[i + 2];

            float za = vertices[ia].z;
            float zb = vertices[ib].z;
            float zc = vertices[ic].z;

            bool aStart = IsStart(za);
            bool bStart = IsStart(zb);
            bool cStart = IsStart(zc);

            bool aEnd = IsEnd(za);
            bool bEnd = IsEnd(zb);
            bool cEnd = IsEnd(zc);

            bool isStartCap = aStart && bStart && cStart;
            bool isEndCap   = aEnd   && bEnd   && cEnd;

            if ((isStartCap && !keepStartCap) ||
                (isEndCap   && !keepEndCap))
            {
                // 捨てる
                continue;
            }

            kept.Add(ia);
            kept.Add(ib);
            kept.Add(ic);
        }

        mesh.triangles = kept.ToArray();
        mesh.RecalculateBounds();
    }
}
