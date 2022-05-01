using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    private int tickCount = 0;
    [SerializeField] PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Weapons.SkipLevel.started += ctx => skipCurrentLevel(); // not working
    }

    List<Target> GetAllTargetsOnlyInScene()
    {
        List<Target> targetsInScene = new List<Target>();

        foreach (Target ship in Resources.FindObjectsOfTypeAll(typeof(Target)) as Target[])
        {
            if (!EditorUtility.IsPersistent(ship.transform.root.gameObject) && !(ship.hideFlags == HideFlags.NotEditable || ship.hideFlags == HideFlags.HideAndDontSave))
                targetsInScene.Add(ship);
        }

        return targetsInScene;
    }

    public void checkEndLevel()
    {
        int numPlayerTargets = 0;
        int numEnemyTargets = 0;
        List<Target> targetsInScene = GetAllTargetsOnlyInScene();
        foreach (Target ship in targetsInScene)
        {
            if (ship.isPlayer)
                numPlayerTargets++;
            else if (ship.isEnemy)
                numEnemyTargets++;
        }
        // if theres no player that means they died. reload the current level
        if (numPlayerTargets <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // if there's no enemies then the level is over, load the next nevel
        else if (numEnemyTargets <=0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    void skipCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    // run 50 times a second
    void FixedUpdate()
    {
        tickCount++;
        if (tickCount % 50 == 0) // every second
            checkEndLevel();
    }
}