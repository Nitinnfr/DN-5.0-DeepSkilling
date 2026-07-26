using System;

namespace CommandPatternExample
{
    // --- STEP 2: DEFINE COMMAND INTERFACE ---
    public interface ICommand
    {
        void Execute();
    }


    // --- STEP 5: IMPLEMENT RECEIVER CLASS ---
    // The component that actually knows the business logic to perform the action.
    public class Light
    {
        private readonly string _location;

        public Light(string location)
        {
            _location = location;
        }

        public void TurnOn()
        {
            Console.WriteLine($"[Device] The {_location} light is now switched ON.");
        }

        public void TurnOff()
        {
            Console.WriteLine($"[Device] The {_location} light is now switched OFF.");
        }
    }


    // --- STEP 3: IMPLEMENT CONCRETE COMMANDS ---
    
    // Command to turn a light ON
    public class LightOnCommand : ICommand
    {
        private readonly Light _light;

        // Constructor receives the specific Receiver it will control
        public LightOnCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOn();
        }
    }

    // Command to turn a light OFF
    public class LightOffCommand : ICommand
    {
        private readonly Light _light;

        public LightOffCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOff();
        }
    }


    // --- STEP 4: IMPLEMENT INVOKER CLASS ---
    // The remote control doesn't know what device it operates. 
    // It just executes the slot command loaded into it.
    public class RemoteControl
    {
        private ICommand _slot;

        // Decoupled assignment: set any command at runtime
        public void SetCommand(ICommand command)
        {
            _slot = command;
        }

        // Trigger the press of a button
        public void PressButton()
        {
            if (_slot == null)
            {
                Console.WriteLine("[Remote] No command assigned to this slot.");
                return;
            }
            _slot.Execute();
        }
    }


    // --- STEP 6: TEST THE COMMAND IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Smart Home Automation Panel ---\n");

            // 1. Initialize our Receivers (The actual devices)
            Light livingRoomLight = new Light("Living Room");
            Light kitchenLight = new Light("Kitchen");

            // 2. Instantiate our Commands and map them to the Receivers
            ICommand livingRoomOn = new LightOnCommand(livingRoomLight);
            ICommand livingRoomOff = new LightOffCommand(livingRoomLight);
            
            ICommand kitchenOn = new LightOnCommand(kitchenLight);

            // 3. Initialize our Invoker (The remote control hardware)
            RemoteControl remote = new RemoteControl();

            // --- TEST RUNS ---

            // Control Living Room Light
            Console.WriteLine("Action: Pressing button for Living Room On...");
            remote.SetCommand(livingRoomOn);
            remote.PressButton();
            
            Console.WriteLine("\nAction: Pressing button for Living Room Off...");
            remote.SetCommand(livingRoomOff);
            remote.PressButton();

            Console.WriteLine(new string('-', 50));

            // Control Kitchen Light using the exact same remote slot
            Console.WriteLine("\nAction: Reconfigured remote slot. Pressing button for Kitchen On...");
            remote.SetCommand(kitchenOn);
            remote.PressButton();

            Console.WriteLine(new string('-', 50));
        }
    }
}