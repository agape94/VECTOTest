
namespace TestFramework
{
    public interface IOperator
    {
        void Apply(double lhs, double[] rhs, string errorMessage = "");
        string Symbol();
        string InverseSymbol();
        string ToString();
    };
}