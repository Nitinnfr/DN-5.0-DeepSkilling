using System;

namespace DependencyInjectionExample
{
    // A simple Customer Model for the application
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // --- STEP 2: DEFINE REPOSITORY INTERFACE ---
    public interface ICustomerRepository
    {
        Customer FindCustomerById(int id);
    }


    // --- STEP 3: IMPLEMENT CONCRETE REPOSITORY ---
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        public Customer FindCustomerById(int id)
        {
            // Simulating a database fetch
            Console.WriteLine($"[Database] Querying SQL database for Customer ID: {id}...");
            return new Customer 
            { 
                Id = id, 
                Name = "Jane Doe", 
                Email = "jane.doe@example.com" 
            };
        }
    }


    // --- STEP 4 & 5: DEFINE SERVICE CLASS WITH DEPENDENCY INJECTION ---
    public class CustomerService
    {
        // The dependency is stored via its interface, not the concrete implementation
        private readonly ICustomerRepository _customerRepository;

        // Constructor Injection: The dependency is passed in at runtime
        public CustomerService(ICustomerRepository customerRepository)
        {
            // Fallback safety check to prevent null references
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public void DisplayCustomerDetails(int id)
        {
            // Service class delegates data access to the injected repository
            Customer customer = _customerRepository.FindCustomerById(id);

            if (customer != null)
            {
                Console.WriteLine("\n--- Customer Record Found ---");
                Console.WriteLine($"ID:    {customer.Id}");
                Console.WriteLine($"Name:  {customer.Name}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine("-----------------------------\n");
            }
            else
            {
                Console.WriteLine($"[Error] Customer with ID {id} not found.");
            }
        }
    }


    // --- STEP 6: TEST THE DEPENDENCY INJECTION IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Customer Management System Hub ---\n");

            // 1. Create the dependency object (The low-level component)
            ICustomerRepository repository = new CustomerRepositoryImpl();

            // 2. Inject the dependency into the service object (The high-level component)
            CustomerService customerService = new CustomerService(repository);

            // 3. Execute operations
            customerService.DisplayCustomerDetails(101);
        }
    }
}