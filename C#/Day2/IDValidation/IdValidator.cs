namespace Day2;

internal class IdValidator : IIdValidator
{
    public IdValidator() { }

    public bool ValidateId(string id)
    {
        int digits = id.ToString().Length;
        // id must be even digits to be something repeated twice.
        if (digits % 2 != 0)
        {
            return true;
        }

        // check if 2nd half is the first half.
        if (id.Substring(0, (digits / 2)) == id.Substring(digits / 2))
        {
            return false;
        }

        return true;
    }
}
