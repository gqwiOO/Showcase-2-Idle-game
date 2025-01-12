using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Mechanics.Product
{
    public class GameGenreDropDownView: MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;


        public event Action<GameGenre> OnGenreSelected; 
        public void Awake()
        {
            _dropdown.onValueChanged.AddListener(Dropdown_OnValueChanged);
        }

        public void Init()
        {
            InitDropdownValues();
        }

        public void ManualSet(GameGenre gameGenre) => _dropdown.value = (int)gameGenre;

        private void Dropdown_OnValueChanged(int newValue)
        {
            var result = (GameGenre)newValue;
                OnGenreSelected?.Invoke(result);
        }

        private void InitDropdownValues()
        {
            var genres = Enum.GetValues(typeof(GameGenre))
                .Cast<GameGenre>()
                .Select(g => g.ToString())
                .ToList();

            _dropdown.ClearOptions();
            _dropdown.AddOptions(genres);
        }
    }
}