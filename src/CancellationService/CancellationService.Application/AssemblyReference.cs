using System.Reflection;

namespace CancellationService.Application;
public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}