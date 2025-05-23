using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object? parameter) => _execute(parameter);
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    public void RaiseCanExecuteChanged()
        => CommandManager.InvalidateRequerySuggested();
}
class RelayCommand<T> : ICommand
{
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public RelayCommand( Action<T> execute, Predicate<T?>? canExecute = null)
    {
        if (execute == null)
            throw new ArgumentNullException("execute");
        _canExecute = canExecute!;
        _execute = execute;
    }

    public bool CanExecute(object? parameter)
    {
        try
        {
            return _canExecute == null ? true : _canExecute((T)parameter!);
        }
        catch
        {
            return true;
        }
    }

    public void Execute(object? parameter)
    {
        _execute((T)parameter!);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}