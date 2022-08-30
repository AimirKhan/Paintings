using System;
using UnityEngine;
using UnityEngine.UI;


namespace Mainmenu
{
    public class MainmenuGames : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_GamesPrefabs;

        protected void Awake()
        {
            InitGames();
        }

        private void InitGames()
        {
            int countGames = m_GamesPrefabs.Length;
            for (int i = 0; i < countGames; i++)
            {
                var game = Instantiate(m_GamesPrefabs[i], transform);
                game.name = i.ToString();
            }
        }
    }
}

