using System.Diagnostics.Metrics;
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

            // assuming

            ///<summary>
            ///we can achieve this using a Stack<T,value>, List<T> or LinkedList<T>
            ///Stack<T,value>:
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





            // Accepts both integers and decimal numbers from command-line arguments
            if (args.Length == 0 || !double.TryParse(args[0], out double result))
            {
                Console.Write("Please enter the valid number: ");
                result = double.Parse(Console.ReadLine()); // Read and parse the entered input value as a double

                /// Validate that the input result is greater than or equal to 0
                while (result < 0)
                {
                    Console.WriteLine("Invalid Number, Please enter valid number");
                    result = double.Parse(Console.ReadLine()); // Allow re-entering input until a valid number is provided
                }
            }

            // Use a stack to store commands  with  previous result using push and pop

            Stack<(ICommand, double PreviousResult)> history = new Stack<(ICommand, double)>();

            while (true)
            {
                Console.Write("Enter command (increment, decrement, double, randadd, undo, exit): ");
                string input = Console.ReadLine().ToLower(); // convert all the commands into lowercase

                if (input == "exit") break; // terimates program

                ICommand command = null; //create an variable instance  

                //prefer switch case than nested if (Better memory usage, faster(direct jump table), cleaner)
                switch (input)
                {
                    case "increment":  // to perform addition
                        command = new Increment();
                        break;
                    case "decrement":  // to perform subtraction
                        command = new Decrement();
                        break;
                    case "double":  // to perform multiple by 2
                        command = new Double();
                        break;
                    case "randadd":  // to generate random number btw 1 to 10
                        command = new RandAdd();
                        break;
                    case "undo":  // to perform revert above commands
                        if (history.Count > 0)
                        {
                            var lastCommand = history.Pop();
                            result = lastCommand.PreviousResult;
                            Console.WriteLine($"Result after undo: {result}");
                        }
                        else  // calls if there is no commands left
                            Console.WriteLine("Nothing to undo.");
                        continue;
                    default:   // other than given commands
                        Console.WriteLine("Unknown command.");
                        continue;
                }

                // Store the command and its result in the stack for undo operations
                history.Push((command, result));
                // Apply the selected command on the current result.
                result = command.Execute(result);
                //print result
                Console.WriteLine($"Result: {result}");
            }
        }




        public interface ICommand
        {
            public double Execute(double param);
            public double Undo(double param);
        }

        public class Increment : ICommand
        {
            public double Execute(double i) => i = i + 1;
            public double Undo(double i) => i = i = 1;
        }

        public class Decrement : ICommand
        {
            public double Execute(double s) => s = s - 1;
            public double Undo(double s) => s = s + 1;
        }
        public class Double : ICommand
        {
            public double Execute(double d) => d = d * 2;
            public double Undo(double d) => d = d / 2;
        }

        public class RandAdd : ICommand
        {
            public double randvalue;

            public RandAdd()
            {
                Random rd = new Random();
                randvalue = rd.Next(1, 10);
                Console.WriteLine($"Rand value is:  {randvalue}");
            }
            public double Execute(double r) => r = r + randvalue;
            public double Undo(double r) => r = r - randvalue;

        }
    }
}
