using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpCode.DesignPatterns.PipeFilter
{
    public interface IOperation<T>
    {
        IEnumerable<T> Execute(IEnumerable<T> input);
    }
    public class Pipeline<T>
    {
        private readonly List<IOperation<T>> operations = new List<IOperation<T>>();

        public Pipeline<T> Register(IOperation<T> operation)
        {
            operations.Add(operation);
            return this;
        }

        public void Execute()
        {
            IEnumerable<T> current = new List<T>();
            foreach (IOperation<T> operation in operations)
            {
                current = operation.Execute(current);
            }
            IEnumerator<T> enumerator = current.GetEnumerator();
            while (enumerator.MoveNext()) ;
        }
    }

    //Usage  
    //=============================================
    public class Main
    {
        public void Call()
        {
            var pipe = new BuildPipeline();
            pipe.Execute();
        }

    }

    public class BuildPipeline : Pipeline<Process>
    {
        public BuildPipeline()
        {
            Register(new GetAllProcesses());
            Register(new LimitByWorkingSetSize());
            Register(new PrintProcessName());
        }
    }

    //We have three stages in the pipeline, the first, get processes:
    public class GetAllProcesses : IOperation<Process>
    {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            return Process.GetProcesses();
        }
    }

    //The second, limit by working set size:
    public class LimitByWorkingSetSize : IOperation<Process>
    {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            int maxSizeBytes = 50 * 1024 * 1024;
            foreach (Process process in input)
            {
                if (process.WorkingSet64 > maxSizeBytes)
                    yield return process;
            }
        }
    }

    //The third, print process name:
    public class PrintProcessName : IOperation<Process>
    {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            foreach (Process process in input)
            {
                Console.WriteLine(process.ProcessName);
            }
            yield break;
        }
    }
}
