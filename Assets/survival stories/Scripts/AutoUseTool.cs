using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUseTool : MonoBehaviour
{
    public InventoryItem invData;
    public bool allow = false;


    private void Start()
    {
     
            

        invData = this.GetComponent<ItemHolderUi>().item;


    }






    public void CollectDataFromTool()
    {
        switch (invData.data.objectType)
        {
            case ObjectType.Tool:
                Debug.Log("111111111111111111111");
                if (((ToolData)invData.data).toolType.HasFlag(ToolType.Container) && ((ToolData)invData.data).toolType.HasFlag(ToolType.Consumable))
                {
                    if (((ToolData)invData.data).contains.data != null)
                    {
                        if ((invData.containerStacks > 0))
                        {
                            ResourceData res = ((ToolData)invData.data).contains.data as ResourceData;
                            Debug.Log("222222222222222222211111");
                            foreach (var item in res.effects)
                            {
                                GameObject gam = new GameObject();
                                EffectsOnPLayer newEff = gam.AddComponent<EffectsOnPLayer>();
                                newEff.effect = item;
                                InventorySystem.effectsOnPlayer.Add(newEff);
                               // gameObject.GetComponent<ItemHolderUi>().item.containerStacks--;

                               // gameObject.GetComponent<ItemHolderUi>().UpdateItemDataToUi(invData);
                              
                                    InventorySystem.instance.RemoveFromContainer(invData);
                                
                            }

                        }

                    }
                }
                if (((ToolData)invData.data).toolType.HasFlag(ToolType.Consumable) && !((ToolData)invData.data).toolType.HasFlag(ToolType.Container))
                {
                    Debug.Log("444444444444444444");
                    foreach (var item in ((ToolData)invData.data).toolEffects)
                    {
                        GameObject gam = new GameObject();
                        EffectsOnPLayer newEff = gam.AddComponent<EffectsOnPLayer>();
                        newEff.effect = item;
                        InventorySystem.effectsOnPlayer.Add(newEff);
                    }
                    InventorySystem.instance.Remove(invData.data);
                }

                break;

            case ObjectType.Resource:

                break;

        }
    }

    public void DoubleTapped()
    {

        AutoUseThis();

    }
    public void AutoUseThis()
    {
        CollectDataFromTool();

    }

    public void AutoUseCheck()
    {

        if ((invData.data as ToolData).useType.HasFlag(UseType.AutoUse))
        {
            CollectDataFromTool();
        }
    }



}
