using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceManagementSystem.Business.Services
{
    public class LMStudioService
    {
        public async Task GenerateCodeAsync()
        {
            string classPath = Path.Combine("..", "..", "..", "..", "E-CommerceManagementSystem.Business", "Services", "ProductService.cs");
            if (!File.Exists(classPath))
            {
                Console.WriteLine("Class.cs not found.");
                return;
            }
            string methodCode = await File.ReadAllTextAsync(classPath);
            Console.WriteLine("Loaded Class.cs");

            var prompt = $$""""
                        Trying to mock interfaces that don't exist - Mock<IProductRepository> but you use concrete ProductRepository
                        Wrong constructor - assumes ProductService(IProductRepository) but yours uses new ProductRepository() internally
                        Wrong property names - uses ID but your Product has ProductID
                        Wrong method signatures - assumes methods return values, but yours are void
                        Completely ignoring your actual code structure

                        The AI is hallucinating a "standard" architecture instead of working with what you actually have. Here's a much more specific prompt that should force it to stick to your reality:
                        csharpvar prompt = $"""
                        CRITICAL: You must ONLY use the exact code structure provided. Do NOT assume interfaces, dependency injection, or different method signatures.

                        ACTUAL ProductService code (use EXACTLY this structure):
                        {methodCode}

                        ACTUAL Product entity (use EXACTLY these properties):
                        - int ProductID (NOT "ID")
                        - string Name  
                        - decimal Price
                        - string Description
                        - int CategoryID
                        - int? OrderID

                        EXACT METHOD SIGNATURES TO TEST:
                        - List<Product> GetAll() - returns list
                        - void Add(Product p) - returns nothing
                        - void Update(Product p) - returns nothing  
                        - void Delete(Product p) - returns nothing
                        - List<Product> Search(string keyword) - returns list
                        - int GetMaxId() - returns int

                        CONSTRAINTS:
                        - ProductService creates its own repository internally: private ProductRepository _repo = new();
                        - NO interfaces exist - use concrete classes only
                        - NO dependency injection - just new ProductService()
                        - These are integration tests working with real repository
                        - Use ProductID property, never "ID"
                        - Methods Add/Update/Delete return void, not objects

                        Generate simple integration tests that:
                        1. Create new ProductService() 
                        2. Test the actual void methods
                        3. Use GetAll() to verify results
                        4. Use correct property names

                        Return ONLY the test class code with proper xUnit structure.
                        """";


            var client = new HttpClient();

            var requestBody = new
            {
                model = "mistralai/mistral-7b-instruct-v0.3",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:1234/v1/chat/completions", content);
                var responseText = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Raw response: " + responseText);

                dynamic parsed = JsonConvert.DeserializeObject(responseText);
                var unitTestCode = parsed.choices[0].message.content.ToString();

                Console.WriteLine("\nGenerated Unit Test:\n");
                Console.WriteLine(unitTestCode);

                string solutionRoot = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
                string testProjectPath = Path.GetFullPath(Path.Combine(solutionRoot, "..", "GenAILLMTests", "GeneratedTests.cs"));
                Console.WriteLine($"Saving to: {testProjectPath}");

                Directory.CreateDirectory(Path.GetDirectoryName(testProjectPath)!);
                await File.WriteAllTextAsync(testProjectPath, unitTestCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling LLM: " + ex.Message);
                if (ex is HttpRequestException httpEx && httpEx.StatusCode.HasValue)
                {
                    Console.WriteLine("HTTP Error: " + httpEx.StatusCode.Value);
                }
            }
        }
    }
}
