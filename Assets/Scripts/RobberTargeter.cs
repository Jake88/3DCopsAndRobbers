using System.Linq;

public class RobberTargeter : Targeter<Robber>
{
    // TargetingBehaviour _targetingBehaviour;
    override protected void DetermineNewTarget()
    {
        // Basic targeting system.
        // If current target is still targetable, keep attacking it.
        if (_targetables.Contains(_currentTarget)) return;

        // Otherwise target the first avilable one in the list.
        _currentTarget = _targetables.First();
        _onNewTargets.Invoke(_currentTarget);


        /*
         * This is an example of a more complex system
         * If there is a robber with less HP that the current target, focus it.
         * NOTE: This is be FLAT hp, not percentage.
        Robber newTarget = _currentTarget;
        foreach (var robber in _targetables)
        {
           if (robber.hp < newTarget.hp)
           {
               newTarget = robber;
           }
       }
        
         if (_currentRobber == newTarget) return;

        _currentRobber = newTarget;
        _onNewTargets.Invoke(_currentTarget);
         */
    }
}
