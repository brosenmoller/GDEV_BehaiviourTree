namespace BehaiviourTree
{
    public abstract class Node
    {
        public NodeStatus Status = NodeStatus.Uninitialised;

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        public NodeStatus Tick()
        {
            if (Status == NodeStatus.Uninitialised)
            {
                Status = NodeStatus.Running;
                OnEnter();
            }

            OnUpdate();

            NodeStatus returnStatus = Status;

            if (Status == NodeStatus.Succes || Status == NodeStatus.Failed)
            {
                Reset();
            }

            return returnStatus;
        }

        private void Reset()
        {
            Status = NodeStatus.Uninitialised;
            OnExit();
        }
    }
}


