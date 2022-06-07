using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmApp.ViewModels
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> excute;          //실행처리를 위한 Generic
        private readonly Predicate<T> canExcute;    //실행여부를 판단하는 Generic

        //실행여부에 따라서 이벤트를 추가해주거나 삭제하는 이벤트핸들러
        public event EventHandler CanExecuteChanged
        {
            add 
            {
                CommandManager.RequerySuggested += value;
            }
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExcute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            excute((T)parameter);
        }

        /// <summary>
        /// excute만 파라미터 받는 생성자
        /// </summary>
        /// <param name="excute"></param>
        public RelayCommand(Action<T> excute) : this(excute, null) {}

        /// <summary>
        /// excute, canExcute를 파라미터로 받는 생성자
        /// </summary>
        /// <param name="excute"></param>
        /// <param name="canExcute"></param>
        public RelayCommand(Action<T> excute, Predicate<T> canExcute = null)
        {
            this.excute = excute ?? throw new ArgumentNullException(nameof(excute));
            this.canExcute = canExcute;
        }
    }
}
