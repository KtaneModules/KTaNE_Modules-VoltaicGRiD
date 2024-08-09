using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class Customs : MonoBehaviour
{

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMNeedyModule Needy;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public class CountryCustoms
    {
        public string Country { get; set; }
        public string[] Restrictions { get; set; }
        public string Document { get; set; }

        public CountryCustoms(string country, string[] restrictions, string document)
        {
            Country = country;
            Restrictions = restrictions;
            Document = document;
        }
    }

    public class CustomsItem
    {
        public string Item { get; set; }
        public string Category { get; set; }
        public bool Eligible { get; set; } = true;
        public bool ListExists { get; set; } = true;

        public CustomsItem(string item, string category, bool listExists = true)
        {
            Item = item;
            Category = category;
            ListExists = listExists;
        }
    }

    List<CountryCustoms> countryList = new List<CountryCustoms>
   {
      new CountryCustoms("Australia", new string[] { "Agricultural" }, "Quarantine declaration"),
      new CountryCustoms("Brazil", new string[] { "Electronics" }, "Authorization from ANATEL"),
      new CountryCustoms("Canada", new string[] { "Weapons" }, "Firearms declaration"),
      new CountryCustoms("China", new string[] { "Printed Material" }, "Content declaration"),
      new CountryCustoms("France", new string[] { "Animal" }, "Health certificate"),
      new CountryCustoms("Germany", new string[] { "Medication" }, "Customs declaration"),
      new CountryCustoms("India", new string[] { "Electronics" }, "Special import permit"),
      new CountryCustoms("Japan", new string[] { "Medication" }, "Yakkan Shoumei certificate"),
      new CountryCustoms("Mexico", new string[] { "Weapons" }, "Special import permit"),
      new CountryCustoms("New Zealand", new string[] { "Agricultural" }, "Biosecurity declaration"),
      new CountryCustoms("Russia", new string[] { "Printed Material" }, "Cultural artifact declaration"),
      new CountryCustoms("Saudi Arabia", new string[] { "Alcohol" }, ""),
      new CountryCustoms("South Africa", new string[] { "Animal" }, "Veterinary certificate"),
      new CountryCustoms("United Kingdom", new string[] { "Weapons" }, "Firearms import license"),
      new CountryCustoms("United States", new string[] { "Agricultural" }, "APHIS permit"),
      new CountryCustoms("Italy", new string[] { "Artifacts" }, "Cultural import declaration"),
      new CountryCustoms("South Korea", new string[] { "Electronics" }, "Radio frequency authorization")
   };

    List<CustomsItem> itemList = new List<CustomsItem>
    {
        new CustomsItem("Antique Furniture", "Artifacts"),
        new CustomsItem("Artwork", "Artifacts"),
        new CustomsItem("Beef Jerky", "Agricultural"),
        new CustomsItem("Books", "Printed Material"),
        new CustomsItem("Camera", "Electronics"),
        new CustomsItem("Canned Soup", "Agricultural"),
        new CustomsItem("Cheese", "Agricultural"),
        new CustomsItem("Chocolate", "Agricultural"),
        new CustomsItem("Drone", "Electronics"),
        new CustomsItem("Face Cream", "Health"),
        new CustomsItem("Fireworks", "Weapons"),
        new CustomsItem("Fresh Apples", "Agricultural"),
        new CustomsItem("Fresh Flowers", "Agricultural"),
        new CustomsItem("Fur Coats", "Animal"),
        new CustomsItem("Gold Jewelry", "Artifacts"),
        new CustomsItem("Golf Clubs", "Sporting"),
        new CustomsItem("Handgun", "Weapons"),
        new CustomsItem("Herbal Supplements", "Medication"),
        new CustomsItem("Hunting Knife", "Weapons"),
        new CustomsItem("Jewelry Box", "Artifacts"),
        new CustomsItem("Keychain Souvenir", "Artifacts"),
        new CustomsItem("Leather Jackets", "Animal"),
        new CustomsItem("Lipstick", "Health"),
        new CustomsItem("Live Fish", "Animal"),
        new CustomsItem("Live Lobsters", "Animal"),
        new CustomsItem("Maple Syrup", "Agricultural"),
        new CustomsItem("Magazines", "Printed Material"),
        new CustomsItem("Meat Products", "Agricultural"),
        new CustomsItem("Metal Detector", "Electronics"),
        new CustomsItem("Painting Supplies", "Artifacts"),
        new CustomsItem("Perfume", "Health"),
        new CustomsItem("Plant Seeds", "Agricultural"),
        new CustomsItem("Political Pamphlets", "Printed Material"),
        new CustomsItem("Pottery", "Artifacts"),
        new CustomsItem("Prescription", "Medication"),
        new CustomsItem("Rifle", "Weapons"),
        new CustomsItem("Rock Collection", "Artifacts"),
        new CustomsItem("Satellite Phone", "Electronics"),
        new CustomsItem("Sculpture", "Artifacts"),
        new CustomsItem("Seeds", "Agricultural"),
        new CustomsItem("Shampoo", "Health"),
        new CustomsItem("Silver Coins", "Artifacts"),
        new CustomsItem("Smartphone", "Electronics"),
        new CustomsItem("Sports Supplements", "Medication"),
        new CustomsItem("Tablet", "Electronics"),
        new CustomsItem("Tea Leaves", "Agricultural"),
        new CustomsItem("Tequila", "Alcohol"),
        new CustomsItem("Trail Mix", "Agricultural"),
        new CustomsItem("Travel Guidebooks", "Printed Material"),
        new CustomsItem("Tripod", "Electronics"),
        new CustomsItem("Unmanned Aerial Vehicle", "Electronics"),
        new CustomsItem("Video Game Console", "Electronics"),
        new CustomsItem("Vitamins", "Medication"),
        new CustomsItem("Whiskey", "Alcohol"),
        new CustomsItem("Bluetooth Speaker", "Electronics", false),
        new CustomsItem("Spices", "Agricultural", false),
        new CustomsItem("Sunscreen", "Health", false),
        new CustomsItem("Electric Toothbrush", "Health", false),
        new CustomsItem("Hair Dryer", "Electronics", false),
        new CustomsItem("Power Bank", "Electronics", false),
        new CustomsItem("Bow and Arrows", "Weapons", false),
        new CustomsItem("Ceramic Vase", "Artifacts", false),
        new CustomsItem("Bonsai Tree", "Agricultural", false),
        new CustomsItem("Gourmet Cheese", "Agricultural", false),
        new CustomsItem("Graphic Tablet", "Electronics", false),
        new CustomsItem("Green Tea", "Agricultural", false),
        new CustomsItem("Japanese Tea Set", "Artifacts", false),
        new CustomsItem("Wine", "Alcohol", false),
        new CustomsItem("Beer", "Alcohol", false),
        new CustomsItem("Hopps", "Agricultural", false),
        new CustomsItem("Maple Syrup", "Agricultural", false),
        new CustomsItem("Noise Meter", "Electronics", false),
        new CustomsItem("Origami Paper", "Artifacts", false),
        new CustomsItem("Paintball Gun", "Weapons", false),
        new CustomsItem("Plant Seeds", "Agricultural", false),
        new CustomsItem("Pottery Wheel", "Artifacts", false),
        new CustomsItem("Solar Charger", "Electronics", false),
        new CustomsItem("Stamp Collection", "Artifacts", false),
        new CustomsItem("Terracotta Pot", "Artifacts", false),
        new CustomsItem("Vintage Vinyl", "Artifacts", false),
    };

    public KMSelectable Left;
    public KMSelectable Right;
    public KMSelectable Allow;
    public KMSelectable Deny;

    public TextMesh Country;
    public TextMesh Item;
    public TextMesh Document;

    private List<CustomsItem> Items = new List<CustomsItem>();
    private int ItemIndex = 0;

    private bool AllowIn = true;
    private bool IsActive = false;

    void Awake()
    { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        Needy.OnActivate += OnActivate;
        Needy.OnNeedyActivation += OnNeedyActivation;
        Needy.OnNeedyDeactivation += OnNeedyDeactivation;
        Needy.OnTimerExpired += OnTimerExpired;

        Left.OnInteract += delegate () { LeftPress(Left); return false; };
        Right.OnInteract += delegate () { RightPress(Right); return false; };
        Allow.OnInteract += delegate () { AllowPress(Allow); return false; };
        Deny.OnInteract += delegate () { DenyPress(Deny); return false; };
    }

    void LeftPress(KMSelectable origin)
    {
        ItemIndex--;
        if (ItemIndex <= 0)
        {
            ItemIndex = Items.Count - 1;
        }

        Item.text = "[ " + Items.ElementAt(ItemIndex).Item.ToUpper() + " ]";

        origin.AddInteractionPunch();
    }

    void RightPress(KMSelectable origin)
    {
        ItemIndex++;
        if (ItemIndex >= Items.Count)
        {
            ItemIndex = 0;
        }

        Item.text = "[ " + Items.ElementAt(ItemIndex).Item.ToUpper() + " ]";

        origin.AddInteractionPunch();
    }

    void AllowPress(KMSelectable origin)
    {
        if (IsActive)
        {
            if (AllowIn)
                OnNeedyDeactivation();
            else
            {
                Strike();
                OnNeedyDeactivation();
            }
        }
    }

    void DenyPress(KMSelectable origin)
    {
        if (IsActive)
        {
            if (AllowIn)
            {
                Strike();
                OnNeedyDeactivation();
            }
            else
                OnNeedyDeactivation();
        }
    }

    void OnDestroy()
    { //Shit you need to do when the bomb ends

    }

    void OnActivate()
    { 

    }

    protected void OnNeedyActivation()
    {
        IsActive = true;
    }

    protected void OnNeedyDeactivation()
    { //Shit that happens when a needy turns off.
        Needy.OnPass();
        IsActive = false;

        System.Random rand = new System.Random();
        var countryId = rand.Next(0, countryList.Count);
        var country = countryList.ElementAt(countryId);

        var itemCount = rand.Next(2, 5);

        Items.Clear();

        for (int i = 0; i <= itemCount; i++)
        {
            var itemId = rand.Next(0, itemList.Count);
            var item = itemList.ElementAt(itemId);
            if (country.Restrictions.Any(r => r.ToLower().Contains(item.Category.ToLower())) || item.ListExists == false)
            {
                item.Eligible = false;
            }
            Items.Add(item);
            itemList.RemoveAt(itemId);
        }

        Country.text = "[ " + country.Country.ToUpper() + " ]";

        var documentId = rand.Next(0, countryList.Count);
        var document = countryList.ElementAt(documentId).Document;

        Document.text = "[ " + document.ToUpper() + " ]";

        Item.text = "[ " + Items[0].Item.ToUpper() + " ]";

        if (Items.Count(i => itemList.Any(l => l.Item == i.Item)) >= 2)
        {
            AllowIn = false;
        }

        if (Items.Any(i => i.ListExists == false) && country.Document != document)
        {
            AllowIn = false;
        }

        if (country.Country != "Saudi Arabia" && document == country.Document)
        {
            AllowIn = true;
        }

        Debug.LogFormat("[CUSTOMS!!! #{0}] Country: {1}, Document: {2}, Items: [{3}], Answer: {4}", ModuleId, country.Country, document, Items.Select(i => i.Item).Join(", "), AllowIn);
    }

    protected void OnTimerExpired()
    { //Shit that happens when a needy turns off due to running out of time.
        Strike();
        OnNeedyDeactivation();
    }

    void Start()
    { //Shit that you calculate, usually a majority if not all of the module
        Needy.SetResetDelayTime(60f, 120f);

        System.Random rand = new System.Random();
        var countryId = rand.Next(0, countryList.Count);
        var country = countryList.ElementAt(countryId);

        var itemCount = rand.Next(2, 5);

        Items.Clear();

        for (int i = 0; i <= itemCount; i++)
        {
            var itemId = rand.Next(0, itemList.Count);
            var item = itemList.ElementAt(itemId);
            if (country.Restrictions.Any(r => r.ToLower().Contains(item.Category.ToLower())) || item.ListExists == false)
            {
                item.Eligible = false;
            }
            Items.Add(item);
            itemList.RemoveAt(itemId);
        }

        Country.text = "[ " + country.Country.ToUpper() + " ]";

        var documentId = rand.Next(0, countryList.Count);
        var document = countryList.ElementAt(documentId).Document;

        Document.text = "[ " + document.ToUpper() + " ]";

        Item.text = "[ " + Items[0].Item.ToUpper() + " ]";

        if (Items.Count(i => itemList.Any(l => l.Item == i.Item)) >= 2)
        {
            AllowIn = false;
        }

        if (Items.Any(i => i.ListExists == false) && country.Document != document)
        {
            AllowIn = false;
        }

        Debug.LogFormat("[CUSTOMS!!! #{0}] Country: {1}, Document: {2}, Items: [{3}], Answer: {4}", ModuleId, country.Country, document, Items.Select(i => i.Item).Join(", "), AllowIn);
    }

    void Update()
    { //Shit that happens at any point after initialization

    }

    void Strike()
    {
        Needy.HandleStrike();
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command)
    {
        yield return null;
    }

    void TwitchHandleForcedSolve()
    { //Void so that autosolvers go to it first instead of potentially striking due to running out of time.
        StartCoroutine(HandleAutosolver());
    }

    IEnumerator HandleAutosolver()
    {
        yield return null;
    }
}