using System.Reflection;

namespace PaymentService.Application;
public static class AssemblyReference
{
    public static Assembly Assembly { get; } = typeof(AssemblyReference).Assembly;
}



