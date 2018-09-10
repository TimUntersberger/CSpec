using System;
using System.Collections.Generic;

namespace CSpec
{
    public abstract class Test
    {
        private const string TICK_CHARACTER = "\u221A";
        private const string X_MARK_CHARACTER = "\u00D7";
        private readonly string className;
        private int depth;

        public void Start() {
            Log(className);
            depth++;
            Run();
            depth--;
        }

        public abstract void Run();

        public Test()
        {
            this.depth = 0;
            var className = this.GetType().Name;
            this.className = className.Substring(0, className.Length - 4);
        }

        private void ContinueTest(string message, Action action, bool canFail)
        {
            depth++;
            if (canFail)
            {
                try
                {
                    action.Invoke();
                    Log(message, true);
                }
                catch (Exceptions.AssertionException ex)
                {
                    Log(message, false);
                    depth++;
                    Log("", ConsoleColor.Red);
                    foreach(var line in ex.Message.Split('\n'))
                    {
                        Log(line, ConsoleColor.Red);
                    }
                    depth--;
                }
            }
            else
            {
                Log(message);
                action.Invoke();
            }
            depth--;
        }

        protected void Describe(string message, Action action) => ContinueTest(message, action, false);

        protected void Context(string message, Action action)
        {
            if (!message.StartsWith("when "))
                throw new Exception("The message of a context has to begin with 'when '");
            ContinueTest(message, action, false);
        }

        protected void It(string message, Action action) => ContinueTest(message, action, true);

        protected void Log(string message)
        {
            if (message.StartsWith("#") || message.StartsWith("."))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.WriteLine((" ".Repeat(depth))+ message);
            Console.ResetColor();
        }

        protected void Log(string message, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Log((" ".Repeat(depth)) + message);
        }

        protected void Log(string message, bool successful)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (successful)
            {
                Log($"{TICK_CHARACTER} {message}", ConsoleColor.Green);
            }
            else
            {
                Log($"{X_MARK_CHARACTER} {message}", ConsoleColor.Red);
            }
        }
    }
}
