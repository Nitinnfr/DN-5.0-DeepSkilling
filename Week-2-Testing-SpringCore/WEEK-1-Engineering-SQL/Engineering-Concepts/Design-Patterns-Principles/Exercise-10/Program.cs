using System;

namespace MVCPatternExample
{
    // --- STEP 2: DEFINE MODEL CLASS ---
    // Represents the application data and domain business state.
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
    }


    // --- STEP 3: DEFINE VIEW CLASS ---
    // Handles data rendering and display output presentation only.
    public class StudentView
    {
        public void DisplayStudentDetails(string studentName, string studentId, string studentGrade)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("          STUDENT PROFILE             ");
            Console.WriteLine("======================================");
            Console.WriteLine($" ID     : {studentId}");
            Console.WriteLine($" Name   : {studentName}");
            Console.WriteLine($" Grade  : {studentGrade}");
            Console.WriteLine("======================================\n");
        }
    }


    // --- STEP 4: DEFINE CONTROLLER CLASS ---
    // Orchestrates communications by handling updates to the Model and controlling the View.
    public class StudentController
    {
        private readonly Student _model;
        private readonly StudentView _view;

        // Binds the decoupled Model and View records together inside the controller
        public StudentController(Student model, StudentView view)
        {
            _model = model;
            _view = view;
        }

        // Bridge properties to manipulate Model data safely
        public string StudentName
        {
            get => _model.Name;
            set => _model.Name = value;
        }

        public string StudentId
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        public string StudentGrade
        {
            get => _model.Grade;
            set => _model.Grade = value;
        }

        // Invokes the view layer rendering with the underlying Model data properties
        public void UpdateView()
        {
            _view.DisplayStudentDetails(_model.Name, _model.Id, _model.Grade);
        }
    }


    // --- STEP 5: TEST THE MVC IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Launching Academic Record Management Console ---\n");

            // 1. Initialize a baseline database record (The Model data)
            Student studentModel = FetchStudentFromDatabase();

            // 2. Setup the output UI engine (The View presentation)
            StudentView studentUiView = new StudentView();

            // 3. Bind everything within the Management Hub (The Controller mediator)
            StudentController controller = new StudentController(studentModel, studentUiView);

            // Render current loaded structural parameters
            Console.WriteLine("Action: Rendering initial student profile...");
            controller.UpdateView();

            // 4. Update data models dynamically via controller access wrappers
            Console.WriteLine("Action: Processing updates (Name fix & Exam Grade advancement)...");
            controller.StudentName = "Jonathan Doe Jr.";
            controller.StudentGrade = "A+";

            // Push controller update request sequence to sync view outputs
            Console.WriteLine("Action: Rendering updated profile record views...");
            controller.UpdateView();
        }

        // Mock database repository simulator method setup
        private static Student FetchStudentFromDatabase()
        {
            return new Student
            {
                Id = "STU-884920",
                Name = "John Doe",
                Grade = "B-"
            };
        }
    }
}