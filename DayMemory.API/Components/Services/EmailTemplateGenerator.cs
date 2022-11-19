using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace DayMemory.Core.Services
{
    public interface IEmailTemplateGenerator
    {
        string GenerateMailTemplate(string templateName, string language, IDictionary<string, string> paramsToReplace);
    }

    public class EmailTemplateGenerator : IEmailTemplateGenerator
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ILogger<EmailTemplateGenerator> _logger;

        // Get our parameterized configuration
        public EmailTemplateGenerator(IWebHostEnvironment hostingEnvironment, ILogger<EmailTemplateGenerator> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public string GenerateMailTemplate(string templateName, string language, IDictionary<string, string> paramsToReplace)
        {
            string dirPath = _hostingEnvironment.WebRootPath + "/content/mails";
            var filePath = $"{dirPath}/{templateName}.{language}.html";
            if (!File.Exists(filePath))
            {
                //filePath = $"{dirPath}/{templateName}./{Consts.defaultCulture.TwoLetterISOLanguageName}.html";
                filePath = $"{dirPath}/{templateName}.en.html";
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"Can't file a template: {filePath}");
                    throw new ArgumentException($"Template does not exist! Path: {filePath}");
                }
            }

            var content = File.ReadAllText(filePath);
            var result = ReplaceTags(content, paramsToReplace);

            return result;
        }

        public string ReplaceTags(string template, params string[] pairs)
        {
            for (var i = 0; i < pairs.Length; i += 2)
            {
                template = template.Replace(string.Format("{{{0}}}", pairs[i]), pairs[i + 1]);
            }
            return template;
        }

        public string ReplaceTags(string template, IDictionary<string, string> pairs)
        {
            foreach (var item in pairs.Keys)
                template = template.Replace(string.Format("{{{0}}}", item), pairs[item]);

            return template;
        }
    }
}
