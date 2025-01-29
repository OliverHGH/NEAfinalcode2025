using UnityEngine;

public class torchbatteryscript : MonoBehaviour
{
    public GameObject player,battery;
    InventoryManager inventoryforbattery;
    TorchItem torch;
    MathsMethods diffinder = new MathsMethods();
    void Start()
    {
        inventoryforbattery= player.GetComponent<InventoryManager>();//gets referenc to inventory
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 5; i++)
        {
            if (inventoryforbattery.loadlist[i, 0] != null)
            {
                if (inventoryforbattery.loadlist[i, 0].objectname == "Torch")//checks if a torch is in inventory
                {
                    torch = (TorchItem)inventoryforbattery.loadlist[i, 0];
                    if (diffinder.findDif(transform.position, player.transform.position) < 1)
                    {//checks if player close to battery
                        torch.chargepercent += 20;
                        if (torch.chargepercent > 100)
                        {
                            torch.chargepercent = 100;
                        }//increases charge of battery
                        battery.SetActive(false);

                    }
                }
            }
        }
    }
}
