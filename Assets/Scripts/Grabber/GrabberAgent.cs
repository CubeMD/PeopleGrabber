using System;
using System.Collections;
using System.Collections.Generic;
using Grabber;
using Unity.MLAgents;
using UnityEngine;
using Walker;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(CharacterVisualization))]
    public class GrabberAgent : Agent, ITeamConvertable
    {
        /// <summary>
        /// First agent is agent being killed and 2nd agent is the agent that killed it
        /// </summary>
        public static event Action<GrabberAgent, GrabberAgent> OnAnyAgentDeath;

        [SerializeField]
        private CharacterVisualization characterVisualization;
       
        private AgentTeam agentTeam;
        public AgentTeam AgentTeam => agentTeam;

        private void OnValidate()
        {
            if (characterVisualization == null)
            {
                characterVisualization = GetComponent<CharacterVisualization>();
            }
        }

        public void SetTeam(AgentTeam team)
        {
            agentTeam = team;
            characterVisualization.SetCharacterColor(team.teamColor);
        }


        /*public override void CollectObservations()
    {
        AddVectorObs(health / 100);

        Collider[] overlapObjs = Physics.OverlapSphere(transform.position, observationRadius, 1 << 8);

        for (int i = 0; i < overlapObjs.Length; i++)
        {
            if (overlapObjs[i] == this.GetComponent<Collider>())
            {
                overlapObjs[i] = overlapObjs[overlapObjs.Length - 1];
                System.Array.Resize(ref overlapObjs, overlapObjs.Length - 1);
                break;
            }
        }

        ParseObjectsIntoObservations(overlapObjs.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList());

        if (showDebug)
        {
            Monitor.Log("Health", health.ToString(), transform);
            Monitor.Log("Reward", info.reward.ToString(), transform);
        }
        
    }*/


        /*public void ParseObjectsIntoObservations(List<Collider> objs)
    {
        float j = 0;

        for (int i = 0; i < objs.Count; i++)
        {

            j++;

            if (i < numPerceivableObjs)
            {
                if (objs[i].gameObject.CompareTag("Floor"))
                {
                    AddVectorObs(1);
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(0);
                }
                else if (objs[i].gameObject.CompareTag("Obstacle"))
                {
                    AddVectorObs(0);
                    AddVectorObs(1);
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(0);
                }
                else if (objs[i].gameObject.CompareTag("Reward"))
                {
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(1);
                    AddVectorObs(0);
                    AddVectorObs(0);
                }
                else if (objs[i].gameObject.CompareTag("Block"))
                {
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(1);
                    AddVectorObs(0);
                }
                else if (objs[i].gameObject.CompareTag("Agent"))
                {
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(0);
                    AddVectorObs(1);
                }
                
                Vector3 objPos = (objs[i].transform.position - transform.position) / observationRadius;

                Vector3 pos = objPos;

                if (objPos.x >= 0)
                {
                    pos.x = 1 - objPos.x;
                }
                else
                {
                    pos.x = -1 - objPos.x;
                }

                if (objPos.y >= 0)
                {
                    pos.y = 1 - objPos.y;
                }
                else
                {
                    pos.y = -1 - objPos.y;
                }

                if (objPos.z >= 0)
                {
                    pos.z = 1 - objPos.z;
                }
                else
                {
                    pos.z = -1 - objPos.z;
                }

                AddVectorObs(pos);

                Vector3 objVelocity;

                if (objs[i].GetComponent<Rigidbody>() != null)
                {
                    objVelocity = objs[i].GetComponent<Rigidbody>().velocity;
                }
                else
                {
                    objVelocity = Vector3.zero;
                }

                Vector3 vel = (objVelocity - rb.velocity) / (maxVel);

                if (objs[i].transform.position.x - transform.position.x > 0)
                {
                    vel = new Vector3(-vel.x, vel.y, vel.z);
                }

                if (objs[i].transform.position.y - transform.position.y > 0)
                {
                    vel = new Vector3(vel.x, -vel.y, vel.z);
                }

                if (objs[i].transform.position.z - transform.position.z > 0)
                {
                    vel = new Vector3(vel.x, vel.y, -vel.z);
                }

                AddVectorObs(vel);

                if (showDebug)
                {
                    Monitor.Log("Vel", (Mathf.Round(vel.x * 100f) / 100f).ToString() + " "
                    + (Mathf.Round(vel.y * 100f) / 100f).ToString() + " "
                    + (Mathf.Round(vel.z * 100f) / 100f).ToString(), objs[i].transform);

                    Monitor.Log("Pos", (Mathf.Round(pos.x * 100f) / 100f).ToString() + " "
                        + (Mathf.Round(pos.y * 100f) / 100f).ToString() + " "
                        + (Mathf.Round(pos.z * 100f) / 100f).ToString(), objs[i].transform);

                    Monitor.Log("Tag", objs[i].gameObject.tag, objs[i].transform);

                    Monitor.Log("Index", i.ToString(), objs[i].transform);
                }

            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < numPerceivableObjs - j; i++)
        {
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
        }

    }*/

        /*public override void AgentAction(float[] vectorAction, string textAction)
    {
        //Vector3 dir = new Vector3(Mathf.Clamp(vectorAction[0], -1f, 1f), 0, Mathf.Clamp(vectorAction[1], -1f, 1f));

        //rb.AddForce(dir.normalized *  force);

        //rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0 , maxVel);

        //AddReward(rb.velocity.magnitude / maxVel * -0.1f);
        //AddReward(0.1f);

        //health -= healthDecay * Time.deltaTime;

       // AddReward(-(100f - health) / 100f);

       // if (health <= 0)
       // {
       //     AddReward(-1f);
       //     Done();
       // }


        moveDir = new Vector3(Mathf.Clamp(vectorAction[0], -1f, 1f), 0, Mathf.Clamp(vectorAction[1], -1f, 1f));
        moveDir *= speed;

        controller.Move(moveDir * Time.deltaTime);

        if (moveDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDir);
        CheckCollider();
    }*/
        
        public void AddNpc(GameObject npc)
        {
            /*NpcWalker npcWalker = npc.GetComponent<NpcWalker>();

        if (npcWalker.currentState == NpcWalker.State.Wondering)
        {
            npcWalker.AddPlayer(gameObject);
            AddAmount(1);
        }
        else if (npcWalker.currentState == NpcWalker.State.Following && npcWalker.player != gameObject)
        {
            if (npcWalker.player.GetComponent<GrabberAgent>().amount < amount)
            {
                npcWalker.player.GetComponent<GrabberAgent>().AddAmount(-1);
                npcWalker.AddPlayer(gameObject);
                AddAmount(1);
            }
        }*/
        }

        public void KillPlayer(GameObject player)
        {
            /*if (player.GetComponent<GrabberAgent>().amount == 1)
        {
            GameManager.GM.DestroyPlayer(player);
            Debug.Log("Player was killed");
            GameManager.GM.SpawnFollowers(gameObject);
        }*/
        }
        public void CheckCollider()
        {
            /*foreach (Collider col in Physics.OverlapSphere(transform.position, 2))
        {
            if (col.gameObject.tag == "Npc" && col.GetComponent<NpcWalker>().player != gameObject)
            {
                AddNpc(col.gameObject);
            }
            else if (col.gameObject.tag == "Player" && col.GetComponent<GrabberAgent>().amount < amount)
            {
                KillPlayer(col.gameObject);
            }
        }*/
        }

        /*public override void AgentReset()
    {
        //transform.localPosition = Vector3.zero;
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        health = 100;
    }*/


        /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            health -= collision.impulse.magnitude * collisionDamage;

            if (health <= 0)
            {
                AddReward(-1f);
                Done();
            }
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            health += healthPerReward;
            AddReward(1f);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Agent"))
        {
            health -= agentCollisionDamage;

            if (health <= 0)
            {
                AddReward(-1f);
                Done();
            }

        }

    }*/



        public bool CanBeConverted(int walkersAmount)
        {
            return agentTeam.WalkerCount < 1;
        }

        public void Convert(AgentTeam team)
        {
            OnAnyAgentDeath?.Invoke(this, team.agent);
        }
    }

    public interface IConvertableListener
    {
        bool OnConvertableEnter(ITeamConvertable convertable);
    }
    
    public class ConvertableTrigger : MonoBehaviour
    {
        private const float TRIGGER_DELAY = 0.5f;
        
        private IConvertableListener listener;
        private bool isResetting = false;
        private Coroutine resetRoutine;
        private List<ITeamConvertable> convertablesWhileInReset = new List<ITeamConvertable>();

        private void Awake()
        {
            listener = GetComponent<IConvertableListener>();
            if (listener == null)
            {
                enabled = false;
            }
        }

        private void OnDisable()
        {
            if (resetRoutine != null)
            {
                StopCoroutine(resetRoutine);
                resetRoutine = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITeamConvertable teamConvertable))
            {
                if (isResetting)
                {
                    if (!convertablesWhileInReset.Contains(teamConvertable))
                    {
                        convertablesWhileInReset.Add(teamConvertable);
                    }
                }
                else
                {
                    BroadcastConvertableEnter(teamConvertable);
                }
            }
        }

        private void BroadcastConvertableEnter(ITeamConvertable convertable)
        {
            if(listener.OnConvertableEnter(convertable))
            {
                StartResetRoutine();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ITeamConvertable teamConvertable))
            {
                if (convertablesWhileInReset.Contains(teamConvertable))
                {
                    convertablesWhileInReset.Remove(teamConvertable);
                }
            }
        }

        private void StartResetRoutine()
        {
            resetRoutine ??= StartCoroutine(ResetRoutine());
        }

        private IEnumerator ResetRoutine()
        {
            isResetting = true;
            yield return new WaitForSeconds(TRIGGER_DELAY);
            isResetting = false;
            resetRoutine = null;
        }

        private void CheckStayedConvertables()
        {
            if (convertablesWhileInReset.Count > 1)
            {
                ITeamConvertable random = convertablesWhileInReset[Random.Range(0, convertablesWhileInReset.Count)];
                BroadcastConvertableEnter(random);
            }
        }
    }
}
