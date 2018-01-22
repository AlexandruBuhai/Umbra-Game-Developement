using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;     

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dText;

    public GameObject pickUp1;
    public GameObject pickUp2;
    public GameObject pickUp3;
    public string[] dialogLines; 
    public string whoSpeaks;
    public int currentLine;

    public bool dialogueActive;
    public bool firstTime;
    public bool canDialogue; // to stop repeating 
    public bool moleApperead;
    public GameObject dialogHold;
    public GameObject moleSurvivor;

    private GameObject gameMan;
    private bool secondTime;

	// Use this for initialization
	void Start () {
        //dialogueActive = true; //TODO turn off 
        gameMan = GameObject.FindGameObjectWithTag("GameManager");
        firstTime = true;
        secondTime = true;
        canDialogue = true;
        pickUp1 = GameObject.FindGameObjectWithTag("Health");
        pickUp2 =  GameObject.FindGameObjectWithTag("Speed");
        pickUp3 = GameObject.FindGameObjectWithTag("Damage");
        moleSurvivor = GameObject.FindGameObjectWithTag("NPC2");
        pickUp1.SetActive(false);
        pickUp2.SetActive(false);
        pickUp3.SetActive(false);
        if (moleSurvivor != null)
        {
            if (moleSurvivor.activeInHierarchy)
            {
                moleApperead = true;
            }
            else
                moleApperead = false;
        }
        if (moleApperead && firstTime)
        {
            //Debug.Log("Fac dialogul");
            string[] s1 =  { "You want to know what I think about him? I don’t like him. But he proved himself to be useful. He guards this place during night." , "He cooks well. I wouldn’t hesitate to kill him the second I saw a spark of betrayal in his eyes though…", "Once Cartita, always Cartita no matter what he says." };
            dialogHold = GameObject.FindGameObjectWithTag("NPC1");
            dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
//            firstTime = false;
//            pickUp1.SetActive(true);
//            pickUp2.SetActive(true);
//            pickUp3.SetActive(true);
        }
        if (gameMan.GetComponent<GameManager>().GetLevelCount() >= 15 && moleApperead)
        {
            string[] s1 =  { "Ma… master! Come here, quickly!" , 
                "> What is going on?", 
                "The… the scroll. It is written in High language. It… it speaks about the Calamity!" ,
                "> That is nonsense. There is no evidence if this event but the world around us. There was no one left to write about it when it happened.",
                "> It is a miracle I have found you, there is no one else left. But… but it speaks about the Calamity before it happened.",
                "> Is there something more?",
                "An… another scroll. It… It seems that Pasari planned to wipe out Cartites.",
                "> WHAT?",
                "But… but the plan backfired. The explosion in the tunnel systems of Cartites got out of hand…",
                ">What are you suggesting?",
                "Pasari… they caused the Calamity."
            };
            dialogHold = GameObject.FindGameObjectWithTag("NPC2");
            dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
            //secondTime = false;
        }

	}
	
	// Update is called once per frame
	void Update () {
	    
        if (currentLine >= dialogLines.Length)
        {
            dBox.SetActive(false);
            dialogueActive = false;
            canDialogue = false;
            StartCoroutine(Wait());

            currentLine = 0;
            Debug.Log(whoSpeaks);

           
            if (firstTime && whoSpeaks == "NPC")
            {
                Debug.Log("talk here should spawn runes");
                pickUp1.SetActive(true);
                pickUp2.SetActive(true);
                pickUp3.SetActive(true);
                firstTime = false;

            }
            else
                if (secondTime && whoSpeaks == "NPC2")
                {
                    secondTime = false;
                }
            else
            if (!secondTime && whoSpeaks == "NPC2")
            {
                int rand = Random.Range(1, 4);
                if (rand == 1)
                {
                    string[] s1 =  { "I… I am no longer a Cartita. I was casted away before the Calamity.", "> Why?", "For… For seeking knowledge. Seek… seeking answers." };
                    dialogHold = GameObject.FindGameObjectWithTag("NPC2");
                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                }
                    else
                if (rand == 2)
                {
                    string[] s1 =  { "Mole… Mole folks… you know them as Cartite. They have a caste system. Thus… thus you are my master from now on.", "> I am not your master.", "Yes… yes you are. Master."  };
                    dialogHold = GameObject.FindGameObjectWithTag("NPC2");
                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                }
                    else
                if (rand == 3)
                {
                    string[] s1 =  { "Master… Master. Look. Scrolls. In High language. It… it is a map of tunnels.", "> What tunnels?", "Cartite tunnels." };
                    dialogHold = GameObject.FindGameObjectWithTag("NPC2");
                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                }
                    
                   else
                if (rand == 4)
                {
                    string[] s1 =  { "Yes… yes, I know High and Low language. Also, the language of Men.", "> How is that possible? I never met a Cartite who could speak.", "I… I studied forbidden scrolls. Only for scholar’s eyes. But… but I was born in the uppermost tunnels.", "I was only a labourer. This… this knowledge was not mine to take. So, I was casted away." };
                    dialogHold = GameObject.FindGameObjectWithTag("NPC2");
                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                }
                    else
                if (rand == 5)
                {
                    string[] s1 =  { "You… you want to know more about Mole folk and High birds, master?", "> Yes please.", "Cart… Cartite lived in the caves They dig and dig and dig. Pasari lived on the mountain tops. They fly and fly and fly. They were never friends.", "No… no friends. And… and now. Only killing. All knowledge lost. Forever. No more caves. No more mountains. Only death and cold."};
                    dialogHold = GameObject.FindGameObjectWithTag("NPC2");
                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                }
            }
            if (!firstTime && whoSpeaks == "NPC")
            {
                int rand = Random.Range(1, 4);
                if (rand == 1)
                {
                    string[] s1 =  { "You want to know if I had family? Who didn’t…" };
                    dialogHold = GameObject.FindGameObjectWithTag("NPC1");
                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                }
                else
                    if (rand == 2)
                    {
                        string[] s1 =  { "Keep an eye out for arrows.", "> Why?", "I used to be an immortal like you, then I took an arrow in the knee."  };
                        dialogHold = GameObject.FindGameObjectWithTag("NPC1");
                        dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                    }
                    else
                        if (rand == 3)
                        {
                            string[] s1 =  { "Seems like you had a rough night." };
                            dialogHold = GameObject.FindGameObjectWithTag("NPC1");
                            dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                        }

                        else
                            if (rand == 4)
                            {
                                string[] s1 =  { "Have you ever thought about what will happen once you fulfill your purpose in this world?", "> Not yet. Will I die?", "Maybe… Maybe we both will." };
                                dialogHold = GameObject.FindGameObjectWithTag("NPC1");
                                dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                            }
                            else
                                if (rand == 5)
                                {
                                    string[] s1 =  { "That is strange, I would swear I heard some dog running around and barking the whole night. But he is nowhere to be seen today. Hope he is alright."};
                                    dialogHold = GameObject.FindGameObjectWithTag("NPC1");
                                    dialogHold.GetComponent<DialogHolder>().dialogueLines = s1;
                                }
            }
        }
        else if (canDialogue && dialogueActive && Input.GetKeyDown(KeyCode.Space) && currentLine < dialogLines.Length)
        {
//            dBox.SetActive(false);
//          dialogueActive = false;
            Debug.Log("I am trying to continue with the dialog");
            currentLine++;
        }
        
        if (currentLine < dialogLines.Length)
        {
            dText.text = dialogLines[currentLine];
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        canDialogue = true;
    }
    public void ShowDialog()
    {
        dialogueActive = true;
        dBox.SetActive(true);
        
    }
}
