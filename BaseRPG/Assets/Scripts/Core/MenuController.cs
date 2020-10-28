using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;
namespace RPG.Core
{
    public class MenuController : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject[] panels;

        [Header("Sub Menu Panels")]
        [SerializeField] StatsUI statsUI;

        [Header("Controller config")]
        bool isOpen = false;

        private void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.I) || Input.GetMouseButtonDown(1))
            {
                if (!isOpen)
                {
                    OpenMenu();
                }
                else
                {
                    CloseMenu();
                }
                
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                int x = 10;
                int y = 20;
                x += x == 20 ? x / y : y / x;
                y -= y == 10 ? y / x : x / y;
                Debug.Log("x "+ x + "/ y:" + y);
            }
        }
        #region PUBLIC METHODS
        public void OpenMenu()
        {
            isOpen = true;
            Movement.Mover.SetCanMove(false);
            mainMenu.SetActive(true);
            
        }

        public void CloseMenu()
        {
            isOpen = false;
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }
            statsUI.Close();
            Movement.Mover.SetCanMove(true);
            mainMenu.SetActive(false);
        }

        public void OpenPanel(int index)
        {
            for(int i = 0; i< panels.Length; i++)
            {
                if(i != index)
                {
                    ClosePanel(i);
                }
            }
            panels[index].SetActive(true);
            statsUI.Close();
            if (index == 1)
            {
                statsUI.LoadPartyPanels();
            }
        }

        public void ClosePanel(int index)
        {
            panels[index].SetActive(false);
        }
        #endregion

    }
}

