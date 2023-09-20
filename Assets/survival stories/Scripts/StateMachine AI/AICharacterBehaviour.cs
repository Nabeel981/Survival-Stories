using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AICharacterBehaviour : MonoBehaviour
{
    public HitboxTrigger hbTrigger;
    public CharacterStats stats;
    public CharacterData data;
    public Animator animator;
    public bool alive = true;
    public float currenHealth;
    public GameObject CurrentTarget;
    public float resetTime;
    public float timeLeft;
    public float originalHealth;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currenHealth = stats.health.maxQuantity;
        hbTrigger.hitEvent += GiveDamage;
        originalHealth = currenHealth;
    }
    public void GiveDamage(GameObject gam)
    {

        AICharacterBehaviour en = gam.GetComponent<AICharacterBehaviour>();


        en.CurrentTarget = gameObject;

        if (en.GetComponent<ObjectInfo>().playerType == PlayerType.User)
        {

            en.stats.health.currentQuantity -= stats.baseAttackDamage;
            if (en.stats.health.currentQuantity <= 0)
            {
                CurrentTarget = null;
                en.animator.SetLayerWeight(1, 0);
                en.Die();

            }
        }
        else
        {
            en.animator.SetBool("Hit", true);
            InventoryItem data = InventorySystem.HasSword();
            if (data != null)
            {
                CraftingSystem.LosingDurability(data, 0);
                en.currenHealth -= (data.data as ToolData).Damage + stats.baseAttackDamage;
            }
            else
            {
                en.currenHealth -= stats.baseAttackDamage;
            }

            if (en.currenHealth <= 0)
            {
                CurrentTarget = null;
                en.Die();

            }
        }


    }
    public void GiveXponDeath()
    {

    }
    public void Die()
    {
        if (alive)
        {
            if (data.objectType == ObjectType.Enemy)
            {
                PlayerProfileSystem.AddXp(5);
                Debug.Log("xp rewarded");
            }

            CurrentTarget = null;
            alive = false;
            foreach (var item in data.ContainedRes)
            {
                InventorySystem.AddItem(item.objectData, item.quantity);
            }
            animator.SetBool("PlayerDead", true);

        }

    }

    public  void GoToMainMenuScene()
    {

        SceneManager.LoadScene(0);
    }

    public void ResetValuesAfterTime()
    {
        if (gameObject.TryGetComponent<StateController>(out StateController con))
        {
            con.enabled = false;

            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(0).GetComponent<SurroundingCheckAction>().EnemiesInTrigger.Clear(); ;



        }
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        alive = true;
        CurrentTarget = null;
        StartCoroutine(StartResetting(resetTime));
    }
    private IEnumerator StartResetting(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        gameObject.GetComponent<Animator>().enabled = true;
        animator.SetBool("PlayerDead", false);
        currenHealth = originalHealth;
        if (gameObject.TryGetComponent<StateController>(out StateController con))
        {
            con.enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;



    }
    public void GiveKillReward()
    {

    }
}
