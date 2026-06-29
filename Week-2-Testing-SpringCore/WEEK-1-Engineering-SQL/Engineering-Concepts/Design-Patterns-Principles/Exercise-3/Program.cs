using System;

namespace BuilderPatternExample
{
    // --- THE PRODUCT ---
    public class Computer
    {
        // Attributes (Read-only to ensure immutability after construction)
        public string CPU { get; }
        public string RAM { get; }
        public string Storage { get; }
        public bool HasGraphicsCard { get; }
        public bool HasWiFi { get; }

        // Step 4: Private constructor that accepts the Builder
        private Computer(Builder builder)
        {
            CPU = builder.CPU;
            RAM = builder.RAM;
            Storage = builder.Storage;
            HasGraphicsCard = builder.HasGraphicsCard;
            HasWiFi = builder.HasWiFi;
        }

        // Display method to verify the configuration
        public void DisplayConfiguration()
        {
            Console.WriteLine($"Computer Configuration:");
            Console.WriteLine($" - CPU: {CPU}");
            Console.WriteLine($" - RAM: {RAM}");
            Console.WriteLine($" - Storage: {Storage}");
            Console.WriteLine($" - Dedicated GPU: {(HasGraphicsCard ? "Yes" : "No")}");
            Console.WriteLine($" - Wi-Fi: {(HasWiFi ? "Yes" : "No")}");
            Console.WriteLine(new string('-', 30));
        }

        // --- STEP 3: THE NESTED BUILDER CLASS ---
        public class Builder
        {
            // Mandatory or default values
            public string CPU { get; private set; } = "Intel i3";
            public string RAM { get; private set; } = "8GB";
            public string Storage { get; private set; } = "256GB SSD";
            public bool HasGraphicsCard { get; private set; } = false;
            public bool HasWiFi { get; private set; } = false;

            // Fluent setter methods returning the Builder instance
            public Builder SetCPU(string cpu)
            {
                CPU = cpu;
                return this;
            }

            public Builder SetRAM(string ram)
            {
                RAM = ram;
                return this;
            }

            public Builder SetStorage(string storage)
            {
                Storage = storage;
                return this;
            }

            public Builder AddGraphicsCard(bool hasGraphicsCard)
            {
                HasGraphicsCard = hasGraphicsCard;
                return this;
            }

            public Builder AddWiFi(bool hasWiFi)
            {
                HasWiFi = hasWiFi;
                return this;
            }

            // The Build method that returns the final Product
            public Computer Build()
            {
                return new Computer(this);
            }
        }
    }

    // --- STEP 5: TEST THE IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Building Custom Computers ---\n");

            // 1. Basic Office PC (Uses mostly defaults)
            Computer officePC = new Computer.Builder()
                .SetCPU("Intel i5")
                .Build();
            
            Console.WriteLine("Office PC Built:");
            officePC.DisplayConfiguration();

            // 2. High-End Gaming PC (Fully customized)
            Computer gamingPC = new Computer.Builder()
                .SetCPU("AMD Ryzen 9")
                .SetRAM("32GB DDR5")
                .SetStorage("2TB NVMe SSD")
                .AddGraphicsCard(true)
                .AddWiFi(true)
                .Build();

            Console.WriteLine("Gaming PC Built:");
            gamingPC.DisplayConfiguration();

            // 3. Budget Server (Custom storage and RAM, no GPU)
            Computer server = new Computer.Builder()
                .SetCPU("Intel Xeon")
                .SetRAM("64GB ECC")
                .SetStorage("4TB HDD Raid")
                .Build();

            Console.WriteLine("Server Built:");
            server.DisplayConfiguration();
        }
    }
}