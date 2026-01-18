using System.Text;

namespace AoC.Days;

internal class StepDependencyCalculator
{
    private HashSet<char> FullStepList { get; } = [];
    private Dictionary<char, HashSet<char>> Dependencies { get; } = [];
    private List<char> StepsCompleted { get; } = [];

    // Part Two
    private int WorkerPool = 5;
    private HashSet<char> AssignedWorkers = [];
    private Dictionary<char, int> StepWorkLeft = [];

    public void Input(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var firstStep = line.Split(' ')[1][0];
            var secondStep = line.Split(' ')[^3][0];
            Dependencies.TryAdd(secondStep, []);
            Dependencies[secondStep].Add(firstStep);
            FullStepList.Add(firstStep);
            FullStepList.Add(secondStep);
        }
    }

    public string PartOne()
    {
        StepsCompleted.Clear();
        while (FullStepList.Count > StepsCompleted.Count)
        {
            HashSet<char> potentialNextSteps = [];
            foreach (var step in FullStepList.Except(StepsCompleted))
            {
                if (!Dependencies.TryGetValue(step, out HashSet<char>? dependencies))
                {
                    potentialNextSteps.Add(step);
                    continue;
                }

                bool dependenciesCreated = true;
                foreach (var dependency in dependencies)
                {
                    if (!StepsCompleted.Contains(dependency))
                    {
                        dependenciesCreated = false;
                    }
                }

                if (dependenciesCreated)
                {
                    potentialNextSteps.Add(step);
                }
            }

            char nextStep = '#';
            foreach (var potentialNextStep in potentialNextSteps)
            {
                if (nextStep == '#')
                {
                    nextStep = potentialNextStep;
                }
                else if (nextStep > potentialNextStep)
                {
                    nextStep = potentialNextStep;
                }
            }

            StepsCompleted.Add(nextStep);
        }

        return new([.. StepsCompleted]);
    }

    public int PartTwo(int workerCount, int taskTime)
    {
        WorkerPool = workerCount;
        AssignedWorkers.Clear();
        StepsCompleted.Clear();

        // Initialise StepCompletion
        foreach(char step in FullStepList)
        {
            StepWorkLeft[step] = ((step - 'A' + 1) + taskTime);
        }

        int timeTaken = 0;
        while(StepsCompleted.Count < FullStepList.Count)
        {
            // If workers are available, get next steps.
            if (WorkerPool > 0)
            {
                HashSet<char> potentialNextSteps = [];
                foreach (var step in FullStepList.Except(StepsCompleted).Except(AssignedWorkers))
                {
                    if (!Dependencies.TryGetValue(step, out HashSet<char>? dependencies))
                    {
                        potentialNextSteps.Add(step);
                        continue;
                    }

                    bool dependenciesCreated = true;
                    foreach (var dependency in dependencies)
                    {
                        if (!StepsCompleted.Contains(dependency))
                        {
                            dependenciesCreated = false;
                        }
                    }

                    if (dependenciesCreated)
                    {
                        potentialNextSteps.Add(step);
                    }
                }

                // Do this until all workers assigned OR no more next steps available: Assign alphabetically earliest next step to a worker.
                int assignmentsToMake = Math.Min(WorkerPool, potentialNextSteps.Count);
                for (int i = 0; i < assignmentsToMake; i++)
                {
                    char nextStep = '#';
                    foreach (var potentialNextStep in potentialNextSteps)
                    {
                        if (nextStep == '#')
                        {
                            nextStep = potentialNextStep;
                        }
                        else if (nextStep > potentialNextStep)
                        {
                            nextStep = potentialNextStep;
                        }
                    }
                    WorkerPool--;
                    AssignedWorkers.Add(nextStep);
                    potentialNextSteps.Remove(nextStep);
                }
            }

            // Pass time until a step is completed.
            bool stepCompleted = false;
            while (!stepCompleted)
            {
                timeTaken++;
                foreach (char stepInProgress in AssignedWorkers)
                {
                    if (--StepWorkLeft[stepInProgress] == 0)
                    {
                        StepsCompleted.Add(stepInProgress);
                        AssignedWorkers.Remove(stepInProgress);
                        WorkerPool++;
                        stepCompleted = true;
                    }
                }
            }
        }

        return timeTaken;
    }
}
