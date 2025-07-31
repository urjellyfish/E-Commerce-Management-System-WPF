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
                        You are generating xUnit integration tests for a WPF application. Follow these STRICT RULES:

                        ✅ USE:
                        - ProductService constructor: `new ProductService()`
                        - Direct calls to `Add()`, `Update()`, `Delete()` methods
                        - Use `GetAll()` to verify results
                        - Assert using:
                          - `Assert.True(all.Any(...))`
                          - `Assert.False(all.Any(...))`
                          - `Assert.Equal(expected, actual)`
                        - Class: `public class ProductServiceTests`
                        - Tests: `[Fact] public void TestAdd() { ... }`
                        - Tests: `[Fact] public void TestGetAll() { ... }`

                        ❌ DO NOT:
                        - Use interfaces (none exist)
                        - Use dependency injection or service collection
                        - Mock repositories or use `Mock<>`
                        - Use `Assert.Contains()` or `Assert.Single(obj)`
                        - Assume method return values (they're void)

                        ProductService structure:

                        ```csharp
                        public class ProductService
                        {
                            private ProductRepository _repo = new();
                            public List<Product> GetAll() => _repo.GetAll();
                            public void Add(Product p) => _repo.Add(p);
                            public Product? GetById(int id) => _repo.GetById(id);
                            public void Update(Product p) => _repo.Update(p);
                            public void Delete(Product p) => _repo.Delete(p);
                            public List<Product> Search(string keyword) => _repo.Search(keyword);
                            public int GetMaxId() => _repo.GetMaxId();
                        }

                        public class Product
                        {
                            public int ProductID { get; set; }
                            public string Name { get; set; }
                            public decimal Price { get; set; }
                            public string Description { get; set; }
                            public int CategoryID { get; set; }
                            public int? OrderID { get; set; }
                        }
                        
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
