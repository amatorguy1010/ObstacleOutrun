using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPipeMiddleScript : MonoBehaviour
{
    public GreenLogicScript logic;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("LogicGreen").GetComponent<GreenLogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        logic.addScore();
    }
}
