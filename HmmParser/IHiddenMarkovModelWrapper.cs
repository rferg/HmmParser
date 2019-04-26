namespace HmmParser
{
    public interface IHiddenMarkovModelWrapper
    {
        int[] Decide(int[] input);
    }
}
