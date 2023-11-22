using System.Collections.Generic;

namespace BehaiviourTree 
{
    public class BlackBoard
    {
        private readonly Dictionary<string, object> variables = new();

        public T GetVariable<T>(string name)
        {
            if (variables.ContainsKey(name))
            {
                return (T)variables[name];
            }
            return default;
        }

        public void SetVariable<T>(string name, T variable)
        {
            if (variables.ContainsKey(name))
            {
                variables[name] = variable;
            }
            else
            {
                variables.Add(name, variable);
            }
        }
    }
}

