using System.Linq;
using System.Text.RegularExpressions;

public static class TextFormatter
{
    #region Methods

    public static string Format(string input)
    {
        while (true)
        {
            var match = Regex.Match(input, @"\$INPUT_(.+?)\$");
            if (!match.Success)
            {
                break;
            }

            var bindingId = match.Groups[1].Value;
            var binding = InputManager.Instance.GetBinding(bindingId);
            var bindingStr = string.Join("/", binding.Bindings.Select(b => b.ToString()).ToArray());
            input = input.Remove(match.Index, match.Length);
            input = input.Insert(match.Index, $"{binding.DisplayName} ({bindingStr})");
        }

        return input;
    }

    #endregion
}