using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;
using System.CodeDom.Compiler;
using System.ComponentModel;
using UnityEngine.SocialPlatforms.Impl;

public class Hogwarts : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public KMSelectable[] Buttons;
    public KMSelectable[] Switches;
    public TextMesh Display;

    private string[] Gryffindor = { "Harry Potter", "Hermione Granger", "Ron Weasley", "Neville Longbottom", "Ginny Weasley", "Fred Weasley", "George Weasley", "Percy Weasley", "Lily Potter", "James Potter", "Sirius Black", "Minerva McGonagall", "Albus Dumbledore", "Rubeus Hagrid", "Seamus Finnigan", "Dean Thomas", "Colin Creevey", "Lavender Brown", "Parvati Patil", "Alicia Spinnet", "Angelina Johnson", "Katie Bell" };
    private string[] Hufflepuff = { "Cedric Diggory", "Nymphadora Tonks", "Pomona Sprout", "Hannah Abbott", "Susan Bones", "Ernie Macmillan", "Justin Finch-Fletchley", "Zacharias Smith", "Newt Scamander", "Susan Bones", "Ernie Macmillan" };
    private string[] Ravenclaw = { "Luna Lovegood", "Cho Chang", "Padma Patil", "Terry Boot", "Michael Corner", "Anthony Goldstein", "Penelope Clearwater", "Filius Flitwick", "Gilderoy Lockhart", "Moaning Myrtle", "Quirinus Quirrell" };
    private string[] Slytherin = { "Draco Malfoy", "Severus Snape", "Tom Riddle", "Bellatrix Lestrange", "Lucius Malfoy", "Narcissa Malfoy", "Pansy Parkinson", "Gregory Goyle", "Vincent Crabbe", "Blaise Zabini", "Horace Slughorn", "Regulus Black", "Phineas Nigellus Black", "Cassius Warrington", "Graham Montague", "Marcus Flint", "Adrian Pucey" };

    private string[] Professors = { "Minerva McGonagall", "Pomona Sprout", "Filius Flitwick", "Severus Snape", "Horace Slughorn" };
    private string[] Seeker = { "Harry Potter", "Cedric Diggory", "Cho Chang", "Draco Malfoy", "Ginny Weasley" };
    private string[] Chaser = { "Angelina Johnson", "Alicia Spinnet", "Katie Bell", "Ginny Weasley", "Zacharias Smith", "Ernie Macmillan", "Susan Bones", "Roger Davies", "Michael Corner", "Marcus Flint", "Adrian Pucey", "Graham Montague", "Cassius Warrington" };
    private string[] Keeper = { "Ron Weasley", "Miles Bletchley", "Oliver Wood", "Ron Weasley" };
    private string[] Beater = { "Fred Weasley", "George Weasley", "Vincent Crabbe", "Gregory Goyle" };
    private string[] Captain = { "Oliver Wood", "Cedric Diggory", "Roger Davies", "Marcus Flint" };

    private string Gryffindor_Rival = "Slytherin";
    private string Slytherin_Rival = "Gryffindor";
    private string Hufflepuff_Rival = "Gryffindor";
    private string Ravenclaw_Rival = "Ravenclaw";

    private int Gryffindor_Slytherin = 170;

    // Switch 1: 
    private int[] switch_states = { 0, 0, 0 };
    private int[] correct_states = { 0, 0, 0 };

    public Material LedOn;
    public Material LedOff;

    public GameObject[] Lights;

    private string Person = "";

    void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += Activate;
        
        foreach (KMSelectable obj in Buttons) {
            obj.OnInteract += delegate () { ButtonPress(obj); return false; };
        }

        foreach (KMSelectable obj in Switches)
        {
            obj.OnInteract += delegate () { SwitchSwitch(obj); return false; };
        }
    }

    void ButtonPress(KMSelectable origin)
    {
        origin.AddInteractionPunch();

        Debug.Log(string.Format("Button pressed: {0}", origin.transform.name));
        Debug.Log(string.Format("Switch states: {0}.{1}.{2}", switch_states[0], switch_states[1], switch_states[2]));
        Debug.Log(string.Format("Correct states: {0}.{1}.{2}", correct_states[0], correct_states[1], correct_states[2]));

        if (origin.transform.name == "Slytherin")
        {
            if (Slytherin.Any(n => n == Person))
            {
                if (Enumerable.SequenceEqual(switch_states, correct_states)) Solve();
                else Strike();
            }
            else Strike();
        }

        if (origin.transform.name == "Ravenclaw")
        {
            if (Ravenclaw.Any(n => n == Person))
            {
                if (Enumerable.SequenceEqual(switch_states, correct_states)) Solve();
                else Strike();
            }
            else Strike();
        }

        if (origin.transform.name == "Hufflepuff")
        {
            if (Hufflepuff.Any(n => n == Person))
            {
                if (Enumerable.SequenceEqual(switch_states, correct_states)) Solve();
                else Strike();
            }
            else Strike();
        }

        if (origin.transform.name == "Gryffindor")
        {
            if (Gryffindor.Any(n => n == Person))
            {
                if (Enumerable.SequenceEqual(switch_states, correct_states)) Solve();
                else Strike();
            }
            else Strike();
        }

    }

    void SwitchSwitch(KMSelectable origin)
    {
        var index = Switches.ToList().IndexOf(origin);

        if (switch_states[index] == 1)
        {
            Lights[index].GetComponent<MeshRenderer>().material = LedOff;

            switch_states[index] = 0;
        }

        else
        {
            Lights[index].GetComponent<MeshRenderer>().material = LedOn;

            switch_states[index] = 1;
        }

        Debug.Log(string.Format("Switch states: {0}.{1}.{2}", switch_states[0], switch_states[1], switch_states[2]));
    }

    void OnDestroy () { //Shit you need to do when the bomb ends
      
    }

    void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

    }
   
    void Start () { //Shit that you calculate, usually a majority if not all of the module
       

        int house_rnd = Rnd.Range(0, 4);
        List<string> people = new List<string>();
        switch (house_rnd)
        {
            case 0:
                people.AddRange(Gryffindor);
                break;
            case 1:
                people.AddRange(Hufflepuff);
                break;
            case 2:
                people.AddRange(Ravenclaw);
                break;
            case 3:
                people.AddRange(Slytherin);
                break;
        }

        int people_rnd = Rnd.Range(0, people.Count);
        Person = people[people_rnd];

        Display.text = "[ " + Person + " ]";

        var score = RedSwitch();
        correct_states[0] = score > 170 ? 1 : 0;
        Debug.Log("Score: " + score);
        correct_states[1] = Professors.Any(p => p == Person) ? 1 : 0;

        var captain = Captain.Any(p => p == Person);

        if (captain) correct_states[2] = 1;
        else correct_states[2] = BlueSwitch(house_rnd);
        Debug.Log(correct_states[2]);
    }

    private int RedSwitch()
    {
        var batt = Bomb.GetBatteryCount();
        var seri_letters = Bomb.GetSerialNumberLetters();
        var seri_sum = Bomb.GetSerialNumberNumbers().Sum();
        var ind_on = Bomb.GetOnIndicators().Count();
        var ind_off = Bomb.GetOffIndicators().Count();
        var ind_tot = ind_on;
        var ind_score = 1;
        
        if (ind_tot > 1) ind_score = ind_tot;

        var score = ((batt + (2 * seri_sum)) * (ind_score ^ 2) + 60);

        return score;
    }


    private int BlueSwitch(int house)
    {
        var serial_even = Bomb.GetSerialNumberNumbers().Last() % 2 == 0;
        var seeker = Seeker.Any(p => p == Person);
        var beater = Beater.Any(p => p == Person);
        var chaser = Chaser.Any(p => p == Person);
        var keeper = Keeper.Any(p => p == Person);
        var serial_letters = Bomb.GetSerialNumberLetters();

        var flip = 0; 
        var matching = 0;

        for (int i = 0; i < Person.Length; i++)
        {
            if (serial_letters.Any(l => l == Person[i])) matching++;
            if (matching >= 2) break;
        }

        if (matching >= 2)
        {
            Debug.Log("Matching letters >= 2");
            flip++;
        }
        if (serial_even)
        {
            Debug.Log("Serial # even");
            flip++;
        }
        
        switch (house)
        {
            case 0: // Gryffindor
                if (seeker) return (1 + flip) % 2;
                else if (chaser) return (0 + flip) % 2;
                else if (keeper) return (1 + flip) % 2;
                else if (beater) return (1 + flip) % 2;
                else return (0 + flip) % 2;

            case 1: // Hufflepuff
                if (seeker) return (1 + flip) % 2;
                else if (chaser) return (0 + flip) % 2;
                else if (keeper) return (0 + flip) % 2;
                else if (beater) return (0 + flip) % 2;
                else return (1 + flip) % 2;

            case 2: // Ravenclaw
                if (seeker) return (0 + flip) % 2;
                else if (chaser) return (1 + flip) % 2;
                else if (keeper) return (1 + flip) % 2;
                else if (beater) return (1 + flip) % 2;
                else return (1 + flip) % 2;

            case 3: // Slytherin
                if (seeker) return (1 + flip) % 2;
                else if (chaser) return (0 + flip) % 2;
                else if (keeper) return (1 + flip) % 2;
                else if (beater) return (0 + flip) % 2;
                else return (0 + flip) % 2;

            default:
                return 1;
        }
    }

    void Update () { //Shit that happens at any point after initialization

    }

    void Solve () {
        GetComponent<KMBombModule>().HandlePass();
    }

    void Strike () {
        GetComponent<KMBombModule>().HandleStrike();
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string Command) {
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve () {
        yield return null;
    }
}
