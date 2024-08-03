using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using UnityEngine.UI;


public class Jurassic : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    
    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public KMSelectable[] Buttons;
    public RawImage Display;

    private List<Dinosaur> Dinosaurs = new List<Dinosaur>();
    private string CorrectDino = "";

    public GameObject[] Lights;
    private int Steps = 0;

    public Material SolvedLight;

    private List<Dinosaur> GlobalUsed = new List<Dinosaur>();

    public List<Texture2D> Assets = new List<Texture2D>();

    private class Dinosaur
    {
        public string Name { get; set; }
        public Texture2D Path { get; set; }
    }

    void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += Activate;       

        foreach (KMSelectable obj in Buttons) {
            obj.OnInteract += delegate () { ButtonPress(obj); return false; };
        }
    }

    void Next()
    {
        foreach (var asset in Assets)
        {
            Dinosaur dino = new Dinosaur();

            dino.Name = asset.name;
            dino.Path = asset;

            Debug.Log(string.Format("Dino: {0}; At: {1}", dino.Name, dino.Path));

            Dinosaurs.Add(dino);
        }

        List<Dinosaur> used = new List<Dinosaur>();
        used.Clear();

        for (int x = 0; x < 4; x++)
        {
            int random = Rnd.Range(0, Dinosaurs.Count);
            Dinosaur dino = Dinosaurs[random];
            int tries = 0;
            while (used.Any(n => n.Name == dino.Name) || GlobalUsed.Any(n => n.Name == dino.Name)) 
            {
                tries++;
                random = Rnd.Range(0, Dinosaurs.Count);
                dino = Dinosaurs[random];
                if (tries > 25) break;
            }
            Buttons[x].GetComponentInChildren<TextMesh>().text = "<b>[ " + dino.Name.ToUpperInvariant() + " ]</b>";
            used.Add(dino);
        }

        foreach (Dinosaur dino in used)
        {
            Debug.Log(dino.Name);
        }

        int disp_random = Rnd.Range(0, used.Count);
        Dinosaur sel_dino = used[disp_random];
        GlobalUsed.Add(sel_dino);

        Debug.Log(sel_dino.Name);

        Display.texture = sel_dino.Path;
        CorrectDino = sel_dino.Name;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        CompleteStep();
    }

    void CompleteStep()
    {
        Steps++;

        if (Steps > 2)
        {
            Solve();
        }
        else
        {
            Next();
        }
    }

    void ButtonPress(KMSelectable origin)
    {
        origin.AddInteractionPunch();
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, origin.transform);

        if (origin.GetComponentInChildren<TextMesh>().text.ToLower().Contains(CorrectDino))
        {
            Display.texture = null;

            for (int x = 0; x < 4; x++)
            {
                Buttons[x].GetComponentInChildren<TextMesh>().text = "";
            }

            Lights[Steps].GetComponent<MeshRenderer>().material = SolvedLight;

            StartCoroutine(Delay());
        }
        else
        {
            Strike();
        }
    }

    void OnDestroy () { //Shit you need to do when the bomb ends
      
    }

    void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on
        Next();
    }

    void Start () { //Shit that you calculate, usually a majority if not all of the module
      
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
