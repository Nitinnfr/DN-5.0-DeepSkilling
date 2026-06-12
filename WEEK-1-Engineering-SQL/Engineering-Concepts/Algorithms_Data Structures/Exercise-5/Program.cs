using System;

namespace TaskManagementSystem
{
    // --- STEP 2: DEFINE TASK CLASS ---
    public class Task
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }

        public Task(string taskId, string taskName, string status)
        {
            TaskId = taskId;
            TaskName = taskName;
            Status = status;
        }

        public override string ToString()
        {
            return $"[ID: {TaskId,-5}] {TaskName,-22} | Status: {Status}";
        }
    }

    // --- STEP 3: IMPLEMENT SINGLY LINKED LIST ---
    
    // Represents a single node wrapper container in memory
    public class Node
    {
        public Task Data { get; set; }
        public Node Next { get; set; }

        public Node(Task task)
        {
            Data = task;
            Next = null;
        }
    }

    public class TaskLinkedList
    {
        private Node _head; // Points to the very first node in the list

        // 1. ADD TASK (Appends to the end of the list)
        public void AddTask(Task task)
        {
            Node newNode = new Node(task);

            if (_head == null)
            {
                _head = newNode;
                Console.WriteLine($"[Success] Set Head Task: {task.TaskName}");
                return;
            }

            Node current = _head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
            Console.WriteLine($"[Success] Appended Task: {task.TaskName}");
        }

        // 2. SEARCH TASK (By ID)
        public Task SearchTaskById(string taskId)
        {
            Node current = _head;

            while (current != null)
            {
                if (current.Data.TaskId.Equals(taskId, StringComparison.OrdinalIgnoreCase))
                {
                    return current.Data; // Target found
                }
                current = current.Next;
            }
            return null; // Target not found
        }

        // 3. TRAVERSE & DISPLAY LIST
        public void TraverseTasks()
        {
            Console.WriteLine("\n================ CURRENT TASK PIPELINE ================");
            if (_head == null)
            {
                Console.WriteLine(" No active tasks found in the pipeline.");
                Console.WriteLine("=======================================================\n");
                return;
            }

            Node current = _head;
            while (current != null)
            {
                Console.WriteLine($" -> {current.Data}");
                current = current.Next;
            }
            Console.WriteLine("=======================================================\n");
        }

        // 4. DELETE TASK
        public bool DeleteTask(string taskId)
        {
            if (_head == null) return false;

            // Scenario A: The node to remove is the Head node itself
            if (_head.Data.TaskId.Equals(taskId, StringComparison.OrdinalIgnoreCase))
            {
                string name = _head.Data.TaskName;
                _head = _head.Next; // Drop the first node by shifting head forward
                Console.WriteLine($"[Success] Deleted Head Task: '{name}'");
                return true;
            }

            // Scenario B: Look deeper down the line
            Node current = _head;
            while (current.Next != null)
            {
                if (current.Next.Data.TaskId.Equals(taskId, StringComparison.OrdinalIgnoreCase))
                {
                    string name = current.Next.Data.TaskName;
                    // Skip over the target node to drop it out of the chain
                    current.Next = current.Next.Next; 
                    Console.WriteLine($"[Success] Deleted Task: '{name}'");
                    return true;
                }
                current = current.Next;
            }

            Console.WriteLine($"[Error] Delete failed. Task ID '{taskId}' not found.");
            return false;
        }
    }

    // --- TEST DRIVER ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Launching Task Tracker Engine ---\n");

            TaskLinkedList taskPipeline = new TaskLinkedList();

            // Populate tasks
            taskPipeline.AddTask(new Task("T01", "Database Architecture Setup", "Completed"));
            taskPipeline.AddTask(new Task("T02", "API Endpoint Security Review", "In Progress"));
            taskPipeline.AddTask(new Task("T03", "UI Dashboard Integration", "Pending"));

            taskPipeline.TraverseTasks();

            // Test Searching
            Console.WriteLine("Action: Finding details for task 'T02'...");
            Task matchedTask = taskPipeline.SearchTaskById("T02");
            if (matchedTask != null)
                Console.WriteLine($" -> Found Match: {matchedTask}\n");

            // Test Deleting an internal task node
            Console.WriteLine("Action: Dropping task 'T02' from queue...");
            taskPipeline.DeleteTask("T02");

            // Verify live link structural adjustments
            taskPipeline.TraverseTasks();
        }
    }
}