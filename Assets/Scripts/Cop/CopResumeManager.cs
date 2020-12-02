using My.Singletons;
using My.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace My.Cops
{
    public class CopResumeManager : MonoBehaviour
    {
        [SerializeField] CopData[] _availableCops;
        [SerializeField] Range _copResumesToGenerate = new Range(1, 3);
        [SerializeField] GameEvent _onResumesGenerated;

        CopFactory _copFactory;

        public Dictionary<CopData, List<CopResume>> CopResumes { get; private set; } = new Dictionary<CopData, List<CopResume>>();

        void Start()
        {
            _copFactory = GetComponent<CopFactory>();
            RefManager.GameTime.PaydayEvent.AddListener(AtNoon);
            foreach (var copType in _availableCops)
                CopResumes.Add(copType, new List<CopResume>());

            AtNoon();
        }

        void AtNoon()
        {
            GenerateCops();
            _onResumesGenerated.Raise();
        }

        void GenerateCops()
        {
            foreach (var copType in _availableCops)
            {
                CopResumes[copType].Clear();
                var numberToGenerate = _copResumesToGenerate.RandomInt;
                for (int i = 0; i < _copResumesToGenerate.Max; i++)
                {
                    if (i < numberToGenerate)
                        CopResumes[copType].Add(_copFactory.GenerateCop(copType));
                }
            }
        }

        void OnDestroy()
        {
            RefManager.GameTime.PaydayEvent.RemoveListener(AtNoon);
        }


        // On noon event, generate a new set of cops with random abilities.
        // This set of robbers should be configurable.


        // This pure data class should then be the foundation supplied to a new Cop on retrieve from the the pool.


        // The new resumes should be sent to the UI panels for displaying and selecting
        // These UI panels also need to clean up the old ones.
        // Should there be a 35-50% chance for a Cop's resume to remain in the pool?
        // Can we lock resumes?
    }
}