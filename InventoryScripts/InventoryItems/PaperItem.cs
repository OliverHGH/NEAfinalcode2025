using UnityEngine;
using UnityEngine.UI;

public class PaperItem : InventoryObj
{
    Text paperwriting;
    GameObject paper;
    public PaperItem(GameObject papersheet, GameObject paperobject, Text writing, string text, string name, Transform p)//constructs paper class
    {
        Object = paperobject;
        Name = name;
        writing.text = text;//assigns the string to be a text displayed to player
        paperwriting = writing;
        paper = papersheet;
        player = p;
        UseableOnce = false;
        stacknum = 1;//only one in stack
        objectparts.Add(paperobject);
    }
    public override void Use()
    {
        paper.SetActive(!paper.activeSelf);
    }
}
