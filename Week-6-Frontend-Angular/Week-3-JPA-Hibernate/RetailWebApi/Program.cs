namespace RetailWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================================
            // 1. SERVICES CONFIGURATION (Dependency Injection Area)
            // ==========================================
            builder.Services.AddControllers(); // Registers routing engines, formatting targets, parameters mapping etc.
            builder.Services.AddEndpointsApiExplorer(); // Exposes endpoints metadata for tracking engines
            builder.Services.AddSwaggerGen(); // Configures Swagger OpenAPI client interaction dashboards

            var app = builder.Build();

            // ==========================================
            // 2. HTTP MIDDLEWARE PIPELINE CONFIGURATION
            // ==========================================
            if (app.Environment.IsDevelopment())
            {
                // Enables visual tooling playground to test your API entries easily
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Explicitly maps incoming requests to our defined Controllers routes structure
            app.MapControllers();

            app.Run();
        }
    }
}