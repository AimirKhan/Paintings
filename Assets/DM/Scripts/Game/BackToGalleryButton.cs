using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGalleryButton : MonoBehaviour
{
    public void BackToGalleryScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
