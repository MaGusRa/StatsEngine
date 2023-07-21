using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RDotNet;

namespace StatsEngine.Pages
{
    public class IndexModel : PageModel
    {
        

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.Initialize();

            NumericVector group1 = engine.CreateNumericVector(new double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 });
            engine.SetSymbol("group1", group1);
            // Direct parsing from R script.
            NumericVector group2 = engine.Evaluate("group2 <- c(29.89, 29.93, 29.72, 29.98, 30.02, 29.98)").AsNumeric();

            // Test difference of mean and get the P-value.
            GenericVector testResult = engine.Evaluate("t.test(group1, group2)").AsList();
            double p = testResult["p.value"].AsNumeric().First();

            return Content($"the p value is: {p}");

        }
    }
}