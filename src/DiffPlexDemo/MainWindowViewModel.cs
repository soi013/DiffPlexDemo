using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DiffPlexDemo
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private string _OldText;
        public string OldText
        {
            get => _OldText;
            set
            {
                if (_OldText == value)
                    return;
                _OldText = value;
                RaisePropertyChanged();
            }
        }


        private string _NewText;
        public string NewText
        {
            get => _NewText;
            set
            {
                if (_NewText == value)
                    return;
                _NewText = value;
                RaisePropertyChanged();
            }
        }


        private SideBySideDiffModel _DiffModel;
        public SideBySideDiffModel DiffModel
        {
            get => _DiffModel;
            set
            {
                if (_DiffModel == value)
                    return;
                _DiffModel = value;
                RaisePropertyChanged();
            }
        }


        public MainWindowViewModel()
        {
            OldText =
@"public class Person
{
    public string FirstName { get; set; } = ""Anakin"";
    public string LastName { get; set; } = ""Skywalker"";
    public int Age { get; set; } = 9;
    public string Place { get; set; } = ""Tatooine"";
}";

            NewText =
@"public class Person
{
    public string NickName { get; set; } = ""Darth Vader"";
    public string FirstName { get; set; } = ""Anakin"";
    public string LastName { get; set; } = ""Skywalker"";
    public int Age { get; set; } = 41;
}";

            var builder = new SideBySideDiffBuilder(new Differ());
            DiffModel = builder.BuildDiffModel(OldText, NewText);
        }
    }
}
