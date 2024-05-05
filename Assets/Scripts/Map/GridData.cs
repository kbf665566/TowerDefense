
public class GridData 
{

        /// <summary>
        /// 網格位置
        /// </summary>
        public Vector2Short GridPos;
        /// <summary>
        /// 網格上的塔
        /// </summary>
        public long TowerUid;
        /// <summary>
        /// 網格狀態
        /// </summary>
        public GridState GridState;
        /// <summary>
        /// 是否可以建築
        /// </summary>
        public bool CanBuild => GridState != GridState.Block && GridState != GridState.Building;
        /// <summary>
        /// 重疊中
        /// </summary>
        public bool NowOverlap => TowerUid != 0;

        public void Clear()
        {
            TowerUid = 0;
            GridState = GridState.Empty;
        }
}

/// <summary>
/// 網格狀態
/// </summary>
public enum GridState
{
    /// <summary> 空格  </summary>
    Empty,
    /// <summary> 建築 </summary>
    Building,
    /// <summary> 障礙 </summary>
    Block,
}