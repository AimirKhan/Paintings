using UnityEngine;
using UnityEngine.SceneManagement;

public class OnMenu : MonoBehaviour
{
   public void Menu() 
   {
      SceneManager.LoadScene(1, LoadSceneMode.Single);
   }
}
