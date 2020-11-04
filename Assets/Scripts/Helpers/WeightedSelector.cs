using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

public class WeightedGroup<T>
{
    Dictionary<T, float> _availableItems;
    float _sumOfWeights;

    public WeightedGroup()
    {
        _availableItems = new Dictionary<T, float>();
    }

    public T GetRandom()
    {
        // possible solution made from https://stackoverflow.com/questions/56692/random-weighted-choice

        float randomNumber = UnityEngine.Random.Range(0, _sumOfWeights);

        T selectedItem = default(T);
        foreach (var item in _availableItems)
        {
            if (randomNumber < item.Value)
            {
                selectedItem = item.Key;
                Debug.Log($"Getting random: Random weight number({randomNumber}). Returning {(selectedItem as ScriptableObject).name}");

                break;
            }

            randomNumber = randomNumber - item.Value;
        }

        return selectedItem;
    }

    public T GetRandomWithinDifficultyValue(float allowedDifficulty)
    {
        // loop through all robbers
        // create an array of robbers with less difficulty weight than the supplied paramter
        // Add the robbers spawn weight to the "currentSumOfWeights"
        // Then call GetRandom, but using this newly defined array.

        // Note: If we change difficult weight to an integer, We could cache these arrays in a dictionary.
        // Each time we add a new robber we could go through each difficulty weight level _above_ the current robber difficulty weight and add that robber.
        // Also when we add a new robber that has a difficulty rating that doesn't already exist in the dictionary, we would need to create a copy of the next lowest difficulty and add this robber to it.
        // This will end up with us having a heap of dictionaries of robber data, but it avoids recreating those every time we want to spawn more... :\ 


        // Another option would be to create a new Weighted group for each difficulty level. Then we determine a difficulty group first before asking for a weighted robber of that group.
        // POTENTIALLY we can created a weighted group or weighted groups, which would allow us to weight higher difficulty groups higher than lower difficulty groups.


        // possible solution made from https://stackoverflow.com/questions/56692/random-weighted-choice


        var matches = _availableItems.Where(kvp => (kvp.Key as RobberData).InitialDifficultyWeight <= allowedDifficulty);
        float currentSumOfSpawnWeights = 0;
        
        foreach (var match in matches)
        {
            currentSumOfSpawnWeights += match.Value;
        }

        float randomNumber = UnityEngine.Random.Range(0, currentSumOfSpawnWeights);

        T selectedItem = default(T);
        foreach (var item in matches)
        {
            if (randomNumber < item.Value)
            {
                selectedItem = item.Key;
                //Debug.Log($"Getting random: Random weight number({randomNumber}). Returning {(selectedItem as ScriptableObject).name}");

                break;
            }

            randomNumber = randomNumber - item.Value;
        }

        return selectedItem;
    }

    public void AddItem(T item, float weight)
    {
        // (Note this doesn't allow for very small numbers. In order to have a "rare" spawn, everythin else needs 100x entries etc) potentially change weight to be an int, and add that many entries into the dictionary (would need to change from being a hashset)
        // If doing this, getting a random selection would be as simple as Range(0, dict.Count)
        // But would also have to track the indices of the entries so it's simple enough to remove them (or add new ones. Not sure how that would work)
        _availableItems.Add(item, weight); // This isn't working as expected. Hashset is not uniqur to item, it's unique to "new WeightedItem" which will always be a new instance
        _sumOfWeights += weight;

        Debug.Log($"Adding item {(item as ScriptableObject).name} with weight of {weight}, making total weight equal {_sumOfWeights}");

        // Possibly I could extend the package I downloaded. Create a middleware in between that holds and array of all the available items
        // , sums their weights and converts it into the percentage for that package manager.
        // This would mean updating the middleware array and redefining the entire weightedRandomizer object whenever rates change (because the percentage of each will change)
    }

    public void ModifyWeight(T item, float newWeight)
    {
        float weight;
        _availableItems.TryGetValue(item, out weight);

        if (weight > 0)
        {
            _sumOfWeights -= weight;
            _sumOfWeights += newWeight;
            _availableItems[item] = newWeight;
        }
    }
}

/*public class WeightedItem<T>
{
    float _weight;
    T _item;
    public float Weight => _weight;
    public T Item => _item;

    public WeightedItem(float weight, T item)
    {
        _weight = weight;
        _item = item;
    }

    public float UpdateWeight(float newWeight) => _weight = newWeight;
}*/

