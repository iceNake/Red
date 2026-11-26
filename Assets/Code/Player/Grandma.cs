using UnityEngine;

[CreateAssetMenu(fileName = "Grandma", menuName = "Scriptable Objects/Grandma")]
public class Grandma : ScriptableObject
{
    public GameObject[] attacks;
    public Transform[] spawnPoints;
    public enum DI {Neutral, Side, Up, Down};
    public Player player;

    public void ExecuteAttack(DI directionalInput)
    {
        switch(directionalInput)
        {
            case DI.Neutral:
                NeutralG();
                break;
            
            
            case DI.Side:
                SideG();
                break;
            
            case DI.Up:
                UpG();
                break;
            
            case DI.Down:
                DownG();
                break;
        }
    }
    public void NeutralG()
    {
        Instantiate(attacks[0], spawnPoints[0]);
    }
    public void SideG()
    {
        Instantiate(attacks[1], spawnPoints[1]);
    }

    public void UpG()
    {
        Instantiate(attacks[3], spawnPoints[3]);
    }

    public void DownG()
    {
        Instantiate(attacks[4], spawnPoints[4]);
    }

}
