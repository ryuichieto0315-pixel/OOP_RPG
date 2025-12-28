namespace OOP_RPG
{
    internal interface ICommand
    {
        public void ActivateCommand(ICallback subj);
    }
}