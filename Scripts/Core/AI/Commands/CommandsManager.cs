using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AI.Commands
{
    public sealed class CommandsManager
    {
        private readonly Queue<Command> _commands = new();

        public void AddCommand(Command command)
        {
            _commands.Enqueue(command);
        }

        public void Execute()
        {
        }

        private IEnumerator Execute_routine()
        {
            foreach (var command in _commands)
            {
                command.Execute();

                yield return new WaitForSeconds(command.Delay);
            }
        }

        public void ClearAllCommands()
        {
            _commands.Clear();
        }
    }
}