namespace api.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

        int age = today.Year - dob.Year;

        if (dob > today.AddYears(-age))
            age--; // age - 1

        return age;
    }
}

/*
dob = 1989-07-10

today = 2025-10-23

    36      2025        1989
int age = today.Year - dob.Year;

*/
