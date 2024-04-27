using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Color = UnityEngine.Color;

[RequireComponent(typeof(LineRenderer))]
public class TowerManager : MonoBehaviour
{

    public LineRenderer LineDrawer;
    [Range(6, 60)]   //creates a slider - more than 60 is hard to notice
    float ThetaScale = 0.01f;//���ܧΪ�
    public float radius = 50f;
    private int Size;
    private float Theta = 0f;

    void Start()
    {
        LineDrawer = GetComponent<LineRenderer>();
        LineDrawer.loop = true;

        LineDrawer.startColor = Color.red;
        LineDrawer.endColor = Color.red;
        LineDrawer.startWidth = 0.1f;
        LineDrawer.endWidth = 0.1f;

        DrawTowerRange();
    }


    private void DrawTowerRange()
    {//�Q�εe�@����D�`�h���h��Ϊ��覡�ӵe�X��νd��
        LineDrawer.enabled = true;
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;
        var tempPos = transform.position;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(tempPos.x + x, tempPos.y, tempPos.z + y));
        }
    }

    private void StopDrawTowerRange()
    {
        LineDrawer.enabled = false;
    }
}
