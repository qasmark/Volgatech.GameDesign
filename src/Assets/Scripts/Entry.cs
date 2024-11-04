using Assets.Scripts.Domain.Book;
using Assets.Scripts.Domain.Elements;
using Assets.Scripts.Domain.Elements.Events;
using Assets.Scripts.Domain.Elements.Repositories.ElementsData;
using Assets.Scripts.Domain.Levels;
using Assets.Scripts.Tests;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Entry : MonoBehaviour
    {
        [SerializeField] 
        public LevelType CurrentLevel = LevelType.Level_0;
        
        private static readonly DrawBookElementsHandler _elementsHandler = new(); 
            
        private void Start()
        {
            ElementsDataRepository.LoadForLevel( CurrentLevel );

            _elementsHandler.DrawAll();
            
            ElementCreatedEventManager.AddWithHighestPriority( OnElementDiscovered );
            ElementCreatedEventManager.AddWithLowestPriority( elementId => Debug.Log( $"Element created: {elementId}" ) );
            
#if UNITY_EDITOR
            RunTests();
#endif
        }

        private static void OnElementDiscovered( ElementId elementId )
        {
            var elementData = ElementsDataRepository.Get( elementId );
            if ( elementData.IsDiscovered )
            {
                return;
            }

            elementData.IsDiscovered = true;
            _elementsHandler.Draw( elementId );
        }

        private void RunTests()
        {
            ElementsDependencyValidator.Validate();
        }
    }
}