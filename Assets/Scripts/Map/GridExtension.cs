using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridExtension
{
    public static int GridUnitSize = 1;
    public static float HalfUnitSize => GridUnitSize * .5f;

    /// <summary>
    /// 網格換算世界座標 (網格中心)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector3 ToWorldPos(this Vector2Short pos)
    {
        return new Vector3(pos.x * GridUnitSize + HalfUnitSize, 0, pos.y * GridUnitSize + HalfUnitSize);
    }

    /// <summary>
    /// 網格換算世界座標 (網格左下角)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector3 ToWorldPosCorner(this Vector2Short pos)
    {
        return new Vector3(pos.x * GridUnitSize, 0, pos.y * GridUnitSize);
    }

    /// <summary>
    /// 取得單位中心世界座標
    /// </summary>
    /// <param name="centerGrid">起始網格位置</param>
    /// <param name="size">佔地大小</param>
    /// <returns>佔地中心世界座標</returns>
    public static Vector3 CalculateCenterPos(this Vector2Short centerGrid, Vector2Short size)
    {
        var sPos = centerGrid.ToWorldPos();
        centerGrid = centerGrid + size - Vector2Short.One;
        var ePos = centerGrid.ToWorldPos();
        return (sPos + ePos) * 0.5f;
    }

    public static Vector2Short GetCenterGrid(Vector2Short gridPos, Vector2Short size)
    {
        int centerX = Mathf.Max(Mathf.FloorToInt(size.x * 0.5f + 0.5f) - 1, 0);
        int centerY = Mathf.Max(Mathf.FloorToInt(size.y * 0.5f + 0.5f) - 1, 0);

        return new Vector2Short(gridPos.x + centerX, gridPos.y + centerY);
    }

    /// <summary>
    /// 取得物件錨點網格座標 (左下角)
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="unitSize"></param>
    /// <returns></returns>
    public static Vector2Short ToAnchorGridPos(this Vector3 pos, Vector2Short unitSize)
    {
        var offsetX = (unitSize.x - 1) * HalfUnitSize;
        var offsetY = (unitSize.y - 1) * HalfUnitSize;
        pos -= new Vector3(offsetX, 0, offsetY);
        return pos.ToGridViewPos();
    }
    /// <summary>
    /// 世界座標轉換為視覺網格
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector2Short ToGridViewPos(this Vector3 pos)
    {
        pos -= new Vector3(HalfUnitSize, 0, HalfUnitSize);
        pos.x = Mathf.Round(pos.x);
        pos.z = Mathf.Round(pos.z);
        return new Vector2Short((short)pos.x / GridUnitSize, (short)pos.z / GridUnitSize);
    }
}
