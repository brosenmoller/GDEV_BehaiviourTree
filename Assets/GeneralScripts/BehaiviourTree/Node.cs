namespace BehaiviourTree
{
    public abstract class Node
    {
        public NodeStatus Status = NodeStatus.Uninitialised;

        protected BlackBoard blackboard;

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

            if (Status == NodeStatus.Success || Status == NodeStatus.Failed)
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

        public virtual void SetupBlackboard(BlackBoard blackboard)
        {
            this.blackboard = blackboard;
        }
    }
}
