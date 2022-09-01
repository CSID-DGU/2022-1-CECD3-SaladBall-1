using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Button> teamCountButtons;

    [SerializeField]
    private List<Button> teamNumButtons;

    private CreateGameRoomData roomData;
    // Start is called before the first frame update
    void Start()
    {
        roomData = new CreateGameRoomData(){ teamCount = 1, teamNum = 1 };
    }
    
    public void UpdateTeam(){
        for(int i = 0; i < teamNumButtons.Count; i++){
            var text = teamNumButtons[i].GetComponentInChildren<Text>();
            if(i < teamCountButtons.Count){
                teamNumButtons[i].interactable=false;
                text.color=Color.gray;
            }
            else{
                teamNumButtons[i].interactable=true;
                text.color=Color.black;
            }
        }
    }
}

public class CreateGameRoomData{
    public int teamCount;
    public int teamNum;
}
