using System.Threading.Tasks.Dataflow;

namespace DuplicatesFinder
{
    public class SearchThreadManager
    {
        private ActionBlock<SearchTask> _threadManager;
        private CancellationTokenSource _tokenSource = new();
        private int _threadsCount = 0;

        public SearchThreadManager(int numThreads)
        {
            var workerOptions = new ExecutionDataflowBlockOptions
            {
                CancellationToken = _tokenSource.Token,
                MaxDegreeOfParallelism = numThreads,
                BoundedCapacity = DataflowBlockOptions.Unbounded
            };
            _threadManager = new ActionBlock<SearchTask>(taskId => ExecuteWorkAsync(taskId, _tokenSource.Token), workerOptions);
        }

        public void AddTask(SearchTask task)
        {
            Interlocked.Increment(ref _threadsCount);
            _threadManager.Post(task);
        }

        public void CancelTasks()
        {
            _tokenSource.Cancel();
        }

        public void FinishQueue()
        {
            _threadManager.Complete();
        }

        public async Task WaitForTaskCompletion()
        {
            await _threadManager.Completion;
        }

        async Task ExecuteWorkAsync(SearchTask task, CancellationToken cancellation)
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
            timeout.CancelAfter(TimeSpan.FromSeconds(task.Timeout));

            try
            {
                SearchEngineSimple.SearchDirectory(task.DirectoryToSearch);
            }
            catch (OperationCanceledException)
            {
                if (cancellation.IsCancellationRequested)
                    return;
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
            finally
            {
                int newThreadsCount = Interlocked.Decrement(ref _threadsCount);
                if (newThreadsCount == 0)
                {
                    FinishQueue();
                }
            }
        }

    }
}