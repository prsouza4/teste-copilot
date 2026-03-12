using System.Text.RegularExpressions;

namespace Usuario.Domain.ValueObjects
{
    public class CPF
    {
        public string Value { get; private set; }

        private CPF() { }

        public CPF(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid CPF");

            Value = value;
        }

        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11) return false;

            var sameDigits = new Regex(@"^(\d)\1+$");
            if (sameDigits.IsMatch(cpf)) return false;

            var numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();
            var sum = 0;

            for (int i = 0; i < 9; i++)
                sum += numbers[i] * (10 - i);

            var remainder = sum % 11;
            var firstDigit = remainder < 2 ? 0 : 11 - remainder;

            if (numbers[9] != firstDigit) return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += numbers[i] * (11 - i);

            remainder = sum % 11;
            var secondDigit = remainder < 2 ? 0 : 11 - remainder;

            return numbers[10] == secondDigit;
        }
    }
}
