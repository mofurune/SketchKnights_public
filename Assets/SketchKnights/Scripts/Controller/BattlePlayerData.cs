using SplineMesh;
using UnityEngine;

namespace SketchKnights.Scripts.Controller
{
    public enum WeaponStyle
    {
        Sword,
        Guard
    }
    
    public class BattlePlayerData
    {
        public readonly SplineNode[][] SwordNodes;
        public readonly SplineNode[][] GuardNodes;
        
        public BattlePlayerData(SplineNode[][] swordNodes, SplineNode[][] guardNodes)
        {
            SwordNodes = swordNodes;
            GuardNodes = guardNodes;
        }
    }
    
    
public static class WeaponSampleData
{
    // 剣のサンプルデータ 5個
    public static SplineNode[][] SwordSamples = new SplineNode[][]
    {
        // 剣サンプル1: 基本的な直刀
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.2f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.4f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.6f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.8f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.0f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.2f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.4f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.6f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f }
        },
        
        // 剣サンプル2: 曲線を持つサーベル風
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(0.1f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.02f, 0.2f, 0f), new Vector3(0.1f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.05f, 0.4f, 0f), new Vector3(0.15f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.08f, 0.6f, 0f), new Vector3(0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.12f, 0.8f, 0f), new Vector3(0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.16f, 1.0f, 0f), new Vector3(0.15f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.18f, 1.2f, 0f), new Vector3(0.1f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.2f, 1.4f, 0f), new Vector3(0.05f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.21f, 1.6f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.22f, 1.8f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f }
        },
        
        // 剣サンプル3: ジグザグした形状
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(0.5f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.1f, 0.2f, 0f), new Vector3(0.3f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.15f, 0.4f, 0f), new Vector3(-0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.1f, 0.6f, 0f), new Vector3(-0.3f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.8f, 0f), new Vector3(-0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(-0.05f, 1.0f, 0f), new Vector3(0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.2f, 0f), new Vector3(0.3f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.05f, 1.4f, 0f), new Vector3(0.1f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.08f, 1.6f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f }
        },
        
        // 剣サンプル4: 太めの両手剣風
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.15f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.15f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.15f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.3f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.14f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.45f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.6f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.8f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.11f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.0f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.2f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.09f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.5f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.08f, Roll = 0f },
            new SplineNode(new Vector3(0f, 1.8f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.07f, Roll = 0f }
        },
        
        // 剣サンプル5: S字カーブの剣
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(0.8f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.15f, 0.2f, 0f), new Vector3(0.6f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.25f, 0.4f, 0f), new Vector3(0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, 0.6f, 0f), new Vector3(-0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.25f, 0.8f, 0f), new Vector3(-0.6f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(0.1f, 1.0f, 0f), new Vector3(-0.8f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(-0.1f, 1.2f, 0f), new Vector3(-0.6f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(-0.15f, 1.4f, 0f), new Vector3(-0.2f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(-0.18f, 1.6f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f }
        }
    };
    
    // 盾のサンプルデータ 5個
    public static SplineNode[][] GuardSamples = new SplineNode[][]
    {
        // 盾サンプル1: 円形の盾
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, 0f, 0f), new Vector3(0.7f, 0.7f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.42f, 0.21f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.42f, 0.42f, 0f), new Vector3(-0.7f, 0.7f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.21f, 0.63f, 0f), new Vector3(-1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.63f, 0f), new Vector3(-0.7f, -0.7f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.21f, 0.42f, 0f), new Vector3(0f, -1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.21f, 0.21f, 0f), new Vector3(0.7f, -0.7f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f }
        },
        
        // 盾サンプル2: 四角い盾
        new SplineNode[]
        {
            new SplineNode(new Vector3(-0.3f, -0.3f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, -0.3f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, -0.3f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, 0f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, 0.3f, 0f), new Vector3(-1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.3f, 0f), new Vector3(-1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, 0.3f, 0f), new Vector3(0f, -1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, 0f, 0f), new Vector3(0f, -1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, -0.3f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f }
        },
        
        // 盾サンプル3: 六角形の盾
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, -0.35f, 0f), new Vector3(0.87f, 0.5f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, -0.175f, 0f), new Vector3(0.87f, 0.5f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, 0.175f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.35f, 0f), new Vector3(-0.87f, 0.5f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, 0.175f, 0f), new Vector3(-0.87f, -0.5f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, -0.175f, 0f), new Vector3(-0.87f, -0.5f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, -0.35f, 0f), new Vector3(0f, -1f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, -0.175f, 0f), new Vector3(0.87f, 0.5f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f }
        },
        /*
        // 盾サンプル4: 涙滴型の盾
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, -0.4f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.11f, Roll = 0f },
            new SplineNode(new Vector3(0.2f, -0.35f, 0f), new Vector3(0.8f, 0.6f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.35f, -0.2f, 0f), new Vector3(0.6f, 0.8f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(0.4f, 0f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.14f, Roll = 0f },
            new SplineNode(new Vector3(0.35f, 0.2f, 0f), new Vector3(-0.6f, 0.8f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(0.2f, 0.35f, 0f), new Vector3(-0.8f, 0.6f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.6f, 0f), new Vector3(-1f, 0f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(-0.2f, 0.35f, 0f), new Vector3(-0.8f, -0.6f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.35f, 0.2f, 0f), new Vector3(-0.6f, -0.8f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(-0.4f, 0f, 0f), new Vector3(0f, -1f, 0f)) { Scale = Vector3.one * 0.14f, Roll = 0f }
        },
        
        // 盾サンプル5: ハート型の盾
        new SplineNode[]
        {
            new SplineNode(new Vector3(0f, -0.3f, 0f), new Vector3(-0.7f, 0.7f, 0f)) { Scale = Vector3.one * 0.1f, Roll = 0f },
            new SplineNode(new Vector3(-0.15f, -0.15f, 0f), new Vector3(-1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, -0.15f, 0f), new Vector3(0f, 1f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(-0.3f, 0.05f, 0f), new Vector3(0.7f, 0.7f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(-0.15f, 0.2f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0f, 0.2f, 0f), new Vector3(1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.15f, 0.2f, 0f), new Vector3(0.7f, -0.7f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, 0.05f, 0f), new Vector3(0f, -1f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(0.3f, -0.15f, 0f), new Vector3(-0.7f, -0.7f, 0f)) { Scale = Vector3.one * 0.13f, Roll = 0f },
            new SplineNode(new Vector3(0.15f, -0.15f, 0f), new Vector3(-1f, 0f, 0f)) { Scale = Vector3.one * 0.12f, Roll = 0f }
        }*/
    };

    public static BattlePlayerData PlayerSample = new BattlePlayerData(SwordSamples, GuardSamples);
}

}