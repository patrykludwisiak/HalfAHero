using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Ability: MonoBehaviour
{
    public virtual bool Cast()
    {
        return false;
    }

    public static T GetAbilityOfType<T>(List<GameObject> abilitiesList) where T : Ability
    {
        foreach(GameObject ability in abilitiesList)
        {
            T component = ability.GetComponent<T>();
            if (component)
            {
                return component;
            }
        }
        return null;
    }

    public static List<GameObject> LoadAbilities(GameObject gameObject, List<GameObject> abilitiesList)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (GameObject ability in abilitiesList)
        {
            GameObject go = Instantiate(ability);
            gameObjects.Add(go);
            go.transform.parent = gameObject.transform;
            go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        return gameObjects;
    }

    public static void DestroyAbilities(List<GameObject> abilitiesList)
    {
        foreach (GameObject ability in abilitiesList)
        {
            Destroy(ability);
        }
        abilitiesList.Clear();
    }

    public static void DestroyAbility<T>(List<GameObject> abilitiesList) where T : Ability
    {
        int index = -1;
        for(int i = 0; i<abilitiesList.Capacity; i++)
        {
            if(abilitiesList[i].GetComponent<T>())
            {
                index = i;
            }
        }
        if(index != -1)
        {
            GameObject ability = abilitiesList[index];
            abilitiesList.RemoveAt(index);
            Destroy(ability);
        }
    }
}
