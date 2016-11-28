using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpCode.DesignPatterns.PipeFilter
{
    //Source : https://ayende.com/blog/3082/pipes-and-filters-the-ienumerable-appraoch
    // Other : https://eventuallyconsistent.net/tag/pipe-and-filters/

    public interface IFilter<T>
    {
        IEnumerable<T> Execute(IEnumerable<T> input);
    }
    public class Pipeline<T>
    {
        private readonly List<IFilter<T>> filters = new List<IFilter<T>>();

        public Pipeline<T> Pipe(IFilter<T> filter)
        {
            filters.Add(filter);
            return this;
        }

        public void Execute()
        {
            IEnumerable<T> current = new List<T>();
            foreach (IFilter<T> filter in filters)
            {
                current = filter.Execute(current);
            }
            IEnumerator<T> enumerator = current.GetEnumerator();
            while (enumerator.MoveNext()) ;
        }
    }

    //Usage  
    //=============================================
    public class Program
    {
        public void Main()
        {
            var pipeline = new Pipeline<Process>();
            pipeline.Pipe(new GetAllProcesses())
                    .Pipe(new LimitByWorkingSetSize())
                    .Pipe(new PrintProcessName());

            pipeline.Execute();
        }

    }
    

    //Concreate Implementations 
    //===============================================================================
    //We have three stages in the pipeline, the first, get processes:
    public class GetAllProcesses : IFilter<Process>
    {
        public IEnumerable<Process> Execute(IEnumerable<Process> input)
        {
            return Process.GetProcesses();
        }
    }

    //The second, limit by working set size:
    public class LimitByWorkingSetSize : IFilter<Process>
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
    public class PrintProcessName : IFilter<Process>
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
    //Concreate Implementations  END
    //===============================================================================
}
