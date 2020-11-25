using UnityEngine;

[RequireComponent(typeof(RobberSpawner))]
public class GrandHeistManager : MonoBehaviour
{
    // Array of GrandHeists, in the order to go through them as the level progresses. For a 10 day level we would have 10 heists defined in the list.
    // --- how do we cator for "grace periods" in the game(currently in GameTime), or is there none?
    [SerializeField] GrandHeist[] _heists;
    int _currentHeistIndex;
    RobberSpawner _robberSpawner;


    void Awake()
    {
        _robberSpawner = GetComponent<RobberSpawner>();
    }

    public void SpawnNext()
    {
        // increment heist index
        // if heist.addedDifficulty > 0
          // newHeist = RobberFactory.GenerateByDifficulty(heist.addedDifficulty)
          // newHeist += heist.robbers

        // foreach robber in heist, robber.addAbilities(heist.abilities) // Might have to do this in robber spawner since it might need to be applied when grabbed from the pool.
        // The above line adds specific abilities. Maybe we want to provide an integer of random abilities to add to each robber too?

        // Trigger heist animation

        // On heist animation end
           // RobberSpawner.spawn(newHeist)
    }

    // Function to create a random heist? // mainly for a sandbox type scenario
    // --- Pick from pre defined heists? Utilise RobberSpawner and just ask for a set of heists?
    public void SpawnRandomHeist()
    {

    }

}

public class GrandHeist : ScriptableObject
{
    // List of robbers that spawn on this heist
    // Name of the heist
    // difficulty level of the heist?
    // Potentially a difficulty weight that can be applied to create some random robbers to spawn AS WELL AS the list of robbers here
            // To create a completely random heist we can just use this value and least the robber list empty.
    
    // Do the below abilities affect boss robbers? Should boss robbers have their own script that prevents random abilities being applied? :S Probably not.
    // abilities[] // an array of abilities to apply to every robber in the heist?
    // int numberOfRandomAbilitiesToAddToEachRobber 
}
