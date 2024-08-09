using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using UnityEngine.UI;
using System.Security.Cryptography;
using Mono.Cecil.Cil;
using Newtonsoft.Json.Converters;
using UnityEngineInternal;
using System.ComponentModel;
using System.Security;
using UnityEditorInternal;
using UnityEngine.Collections;


public class Broken_Calculator : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public KMSelectable[] Buttons;
    public TextMesh Display;
    public TextMesh Operations;
    private char Operation = '\0';

    private decimal? firstNumber = null;
    private decimal? secondNumber = null;

    private bool readyForInput = true;

    void Awake()
    { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        GetComponent<KMBombModule>().OnActivate += Activate;

        foreach (KMSelectable obj in Buttons)
        {
            obj.OnInteract += delegate () { ButtonPress(obj); return false; };
        }
    }

    void Next()
    {
        
    }

    void CompleteStep()
    {
        
    }

    void DisplayAppend(int number)
    {   
        if (Display.text == "0" || readyForInput)
        {
            Display.text = number.ToString();
            readyForInput = false;
        }
        else
            Display.text = Display.text + number.ToString();
    }

    void ButtonPress(KMSelectable origin)
    {
        int num = 0;

        if (int.TryParse(origin.transform.name, out num))
        {
            if (secondNumber != 0)
                secondNumber = 0;
            DisplayAppend(num);
        }

        switch (origin.transform.name)
        {
            // Numbers, "CLR", "MOD", "DIV", "MUL", "PLU", "MIN", "EQU", "DOT"
            case "CLR":
                Display.text = "0";
                Operations.text = "";
                firstNumber = null;
                secondNumber = null;
                break;
            case "MOD":
                HandleOperation('%');       
                break;
            case "DIV":
                HandleOperation('/');
                break;
            case "MUL":
                HandleOperation('*');
                break;
            case "PLU":
                HandleOperation('+');
                break;
            case "MIN":
                HandleOperation('-');
                break;
            case "DOT":
                break;
            case "EQU":
    
                Calculate();
                readyForInput = true;
                break;
        }
    }

    void HandleOperation(char op)
    {
        if (firstNumber == null)
        {
            firstNumber = decimal.Parse(Display.text);
            Display.text = "0";
        }
        else if (secondNumber == null || secondNumber == 0)
        {
            secondNumber = decimal.Parse(Display.text);
            Display.text = "0";
        }
        else
            Calculate();

        Operations.text = op.ToString();
        readyForInput = true;
    }

    void Calculate()
    {
        decimal result = 0;

        Debug.Log(firstNumber);
        Debug.Log(secondNumber);

        if (secondNumber == null || secondNumber == 0)
        {
            secondNumber = decimal.Parse(Display.text);
        }

        switch (Operations.text)
        {
            case "+":
                result = (decimal)firstNumber + (decimal)secondNumber;
                break;
            case "-":
                result = (decimal)firstNumber - (decimal)secondNumber;
                break;
            case "/":
                result = (decimal)firstNumber / (decimal)secondNumber;
                break;
            case "*":
                result = (decimal)firstNumber * (decimal)secondNumber;
                break;
            case "%":
                result = (decimal)firstNumber % (decimal)secondNumber;
                break;
        }

        Display.text = result.ToString();
        firstNumber = decimal.Parse(Display.text);
        Operations.text = "";
    }

    void OnDestroy()
    { //Shit you need to do when the bomb ends

    }

    void Activate()
    { //Shit that should happen when the bomb arrives (factory)/Lights turn on
        Next();
    }

    void Start()
    { //Shit that you calculate, usually a majority if not all of the module

    }

    void Update()
    { //Shit that happens at any point after initialization

    }

    void Solve()
    {
        GetComponent<KMBombModule>().HandlePass();
    }

    void Strike()
    {
        GetComponent<KMBombModule>().HandleStrike();
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command)
    {
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
    }
}
