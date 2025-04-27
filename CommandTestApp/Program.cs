using System.Windows.Input;

namespace CommandTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //requirement

            ///<summary>
            /// Write a C# console program as specified below and send its source code to us.
            /// The program accepts one command line argument: a number value that is used as
            /// initial value of a result variable. The program prompts its user for commands in an
            /// infinte loop. After each prompt, it applies the command and prints out the new result value.
            /// These commands are supported:
            /// +increment -- increases result value by 1
            /// +decrement -- decreases result value by 1
            /// +double -- doubles the result value
            /// +randadd -- changes result by random number
            /// +undo -- reverts the most recent command that is not an undo and was not yet undone
            /// For example, with an inital result value of 1, the command sequence
            /// "increment,increment,increment,double,undo,undo" produces the result values: 2, 3,4, 8, 4, 3
            /// Implement each command as its own class.
            /// If anything is under-specified, make an assumption about it and note it down in a
            /// comment.
            ///</summary>

            // analysis approach

            ///<summary>
            ///we can achieve this using a Stack<T>, List<T> or LinkedList<T>
            ///Stack<T>:
            ///      follows LIFO
            ///      push and pop operations 
            ///      uses less memory
            ///      only supports uno operations 
            ///      
            /// List<T>:
            ///      supports undo and redo operations by managing [index].
            ///      Add, Remove and acess by index.
            ///      
            /// LinkedList<T>
            ///      supports undo and redo with AddLast, RemoveLast
            ///      slightly more memory usage 
            ///</summary>

            int startvalue = 0;
            if (args.Length == 0 || !int.TryParse(args[0], out int result) || result < 0)
            {
                Console.Write("Please enter the valid number: ");
                result = int.Parse(Console.ReadLine());
                startvalue = result;
                while (result < 0)
                {
                    Console.WriteLine("Invalid Number, Please enter valid number");
                    result = int.Parse(Console.ReadLine());
                }
            }


            Stack<ICommand> history = new Stack<ICommand>();

            while (true)
            {
                Console.Write("Enter command (increment, decrement, double, randadd, undo, exit): ");
                string input = Console.ReadLine().ToLower();

                if (input == "exit") break;

                ICommand command = null;

                switch (input)
                {
                    case "increment":
                        command = new Increment();
                        break;
                    case "decrement":
                        command = new Decrement();
                        break;
                    case "double":
                        command = new Double();
                        break;
                    case "randadd":
                        command = new RandAdd();
                        break;
                    case "undo":
                        if (history.Count > 0)
                        {
                            var lastCommand = history.Pop();
                            result = history.Count == 0 ? startvalue: lastCommand.Undo(result);
                            
                            Console.WriteLine($"Result after undo: {result}");
                        }
                        else
                        {
                            Console.WriteLine("Nothing to undo.");
                        }
                        continue;
                    default:
                        Console.WriteLine("Unknown command.");
                        continue;
                }

                result = command.Execute(result);
                history.Push(command);
                Console.WriteLine($"Result: {result}");
            }
        }




        public interface ICommand
        {
            public int Execute(int param);
            public int Undo(int param);
        }

        public class Increment : ICommand
        {
            public int Execute(int i) => i = i + 1;
            public int Undo(int i) => i = i = 1;
        }

        public class Decrement : ICommand
        {
            public int Execute(int s) => s = s - 1;
            public int Undo(int s) => s = s + 1;
        }
        public class Double : ICommand
        {
            public int Execute(int d) => d = d * 2;
            public int Undo(int d) => d = d / 2;
        }

        public class RandAdd : ICommand
        {
            public int randvalue;

            public RandAdd()
            {
                Random rd = new Random();
                randvalue = rd.Next(1, 10);
                Console.WriteLine($"Rand value is:  {randvalue}");
            }
            public int Execute(int r) => r = r + randvalue;
            public int Undo(int r) => r = r - randvalue;

        }
    }
}
