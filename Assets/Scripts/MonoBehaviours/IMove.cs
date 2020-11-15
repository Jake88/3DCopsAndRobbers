using My.ModifiableStats;

public interface IMove
{
    void ApplyMoveSpeedModifer(StatModifier modifer);
    void RemoveMoveSpeedModifer(StatModifier modifer);
}
