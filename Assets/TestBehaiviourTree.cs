using BehaiviourTree;
using UnityEngine;

public class TestBehaiviourTree : MonoBehaviour
{
    private Node tree;

    private void Awake()
    {
        tree = new SequenceNode(new HelloWorldNode());
    }

    private void Update()
    {
        tree.Tick();
    }
}

public class HelloWorldNode : Node
{
    private float timer;

    public override NodeStatus Evaluate()
    {
        if (Status == NodeStatus.Uninitialised)
        {
            return NodeStatus.Running;
        }

        return Status;
    }

    public override void OnEnter()
    {
        Debug.Log("Hello World");
        timer = 0;
    }

    public override void OnUpdate()
    {
        Debug.Log("Updated Hello World");
        timer += Time.deltaTime;
        if (timer > 2)
        {
            Status = NodeStatus.Succes;
        }
    }

    public  override void OnExit() 
    {
        Debug.Log("Bye World");
    }
}


