
public class GridData 
{

        /// <summary>
        /// �����m
        /// </summary>
        public Vector2Short GridPos;
        /// <summary>
        /// ����W����
        /// </summary>
        public long TowerUid;
        /// <summary>
        /// ���檬�A
        /// </summary>
        public GridState GridState;
        /// <summary>
        /// �O�_�i�H�ؿv
        /// </summary>
        public bool CanBuild => GridState != GridState.Block && GridState != GridState.Building;
        /// <summary>
        /// ���|��
        /// </summary>
        public bool NowOverlap => TowerUid != 0;

        public void Clear()
        {
            TowerUid = 0;
            GridState = GridState.Empty;
        }
}

/// <summary>
/// ���檬�A
/// </summary>
public enum GridState
{
    /// <summary> �Ů�  </summary>
    Empty,
    /// <summary> �ؿv </summary>
    Building,
    /// <summary> ��ê </summary>
    Block,
}