namespace HmmNameParser
{
    public interface IHiddenMarkovModelWrapper
    {
        int[] Decide(int[] input);
    }
}
