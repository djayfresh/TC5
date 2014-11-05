using UnityEngine;
using System.Collections;

public class MoveToFrom : MonoBehaviour {

	public PathNode[] path;
	private float DISTANCE_BUFFER = 0.5f;
	public float lerpSpeed = 2;
	public bool shouldMove = false;
	public float stillTime = 5;
	public bool shouldReverse;
	private float holdTime;
	private int currentNode;
	public ActionTrigger triggerVolumn;
	public bool trackPlayer = true;

	private Animator anim;
	private int speedHash = Animator.StringToHash("Speed");
	private int meleeHash = Animator.StringToHash("Melee");
	private int walkingHash = Animator.StringToHash("Base Layer.Walking");
	private int meleeAttackHash = Animator.StringToHash("Base Layer.Melee Attack");
	public float meleeDistance = 0;

	// Use this for initialization
	void Start () 
	{
		currentNode = 0;
		if(path.Length > 0)
		{
			transform.position = path[currentNode].transform.position;
		}
		holdTime = 0;
		anim = GetComponentInChildren<Animator>();

		ActionTrigger.OnTrigger += TriggerActive;
	}
	public bool atLastNode()
	{
		return currentNode == path.Length;
	}
	public bool hasMoved()
	{
		return currentNode != 0;
	}
	void reverse()
	{
		PathNode tmp;
		for(int i = 0; i < (path.Length - 1)/2; i++)
		{
			tmp = path[i];
			path[i] = path[path.Length - i];
			path[path.Length - i] = tmp;
		}
		currentNode = path.Length - currentNode;
	}

	void TriggerActive(ActionTrigger e)
	{
		if(triggerVolumn == e)
		{
			shouldMove = true;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(trackPlayer)
		{
			GameObject player = Camera.main.gameObject;
			transform.LookAt(player.transform.position);
			if(Vector3.Distance(player.transform.position, transform.position) < meleeDistance && meleeDistance > 0)
			{
				anim.SetTrigger(meleeHash);
			}
		}

		if (shouldMove && currentNode < path.Length) 
		{
			bool notInAnimaton = true;
			if(anim != null)
			{
				anim.SetFloat(speedHash, 1);
				if(anim.GetCurrentAnimatorStateInfo(0).nameHash == meleeAttackHash)
				{
					notInAnimaton = false;
				}
			}
			PathNode dest = path[currentNode];
			Vector3 pos = dest.transform.position;
			float dist = Vector3.Distance (pos, this.transform.position);
			if (dist > DISTANCE_BUFFER && notInAnimaton) 
			{
				transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * path[currentNode].lerpSpeed);
					//transform.position = Vector3.Lerp(initalPos, pos, lerpTime);
				if(!trackPlayer)
				{
					transform.LookAt(pos);
				}
			}
			else if(notInAnimaton)
			{
				if(anim != null)
				{
					anim.SetFloat(speedHash, 0);
				}
				holdTime += Time.deltaTime;
				if(holdTime >= stillTime)
				{

					currentNode++;
					if(shouldReverse == true && currentNode >= path.Length)
					{
						reverse();
					}
					holdTime = 0;
				}
			}
		}
	}

	public bool shouldShoot()
	{
		if (currentNode < path.Length)
			return path [currentNode].shouldShoot;
		else
			return path [currentNode - 1].shouldShoot;
	}
	void OnDestory()
	{
		ActionTrigger.OnTrigger -= TriggerActive;
	}
}
