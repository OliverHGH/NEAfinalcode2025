using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManage;
public class Scene2ObjectManager : InventoryObjectManager
{
    public GameObject paperobject1,papersheet1, paperobject2,papersheet2;
    public Text p1t, p2t;
    ciphergeneration ciphermaker = new ciphergeneration();
    string keytext;
    string codetext;
    public override void ListofAllItems()
    {
        ciphermaker.CreateCipher();//creates a cipher from random key
        keytext = "key:   " + ciphermaker.keytodisplay;
        codetext = "Code: " + ciphermaker.encodedtext;
        PaperItem keypaper = new PaperItem(papersheet1,paperobject1,p1t, keytext, "Key paper",p);
        ItemList.Add(keypaper);
        PaperItem CodePaper = new PaperItem(papersheet2,paperobject2, p2t,codetext,"Code paper", p);
        ItemList.Add(CodePaper);
        base.ListofAllItems();//adds new items to lost for 2nd level
    }

}
