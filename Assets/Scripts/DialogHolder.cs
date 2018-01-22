using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogHolder : MonoBehaviour {


    private DialogueManager dMan;
    public string nameObj;


    public string[] dialogueLines; //dialogue lines holders

	// Use this for initialization
	void Start () {
        dMan = FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
           // Debug.Log("Player entered");

            if (Input.GetKeyUp(KeyCode.Space))
            {
                //dMan.ShowBox(dialogue);
                if (dMan.dialogueActive == false)
                {
                    dMan.ShowDialog();
                    dMan.whoSpeaks = nameObj;
                    dMan.currentLine = 0;
                    dMan.dialogLines = dialogueLines;

                }
            }
        }
    }
}
