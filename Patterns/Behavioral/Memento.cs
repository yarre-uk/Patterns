namespace Memento
{
    // Создатель содержит некоторое важное состояние, которое может со временем
    // меняться. Он также объявляет метод сохранения состояния внутри снимка и
    // метод восстановления состояния из него.
    class Originator
    {
        // Для удобства состояние создателя хранится внутри одной переменной.
        private string _state;

        public Originator(string state)
        {
            _state = state;
            Console.WriteLine("Originator: My initial state is: " + state);
        }

        // Бизнес-логика Создателя может повлиять на его внутреннее состояние.
        // Поэтому клиент должен выполнить резервное копирование состояния с
        // помощью метода save перед запуском методов бизнес-логики.
        public void DoSomething()
        {
            Console.WriteLine("Originator: I'm doing something important.");
            _state = GenerateRandomString(30);
            Console.WriteLine($"Originator: and my state has changed to: {_state}");
        }

        private string GenerateRandomString(int length = 10)
        {
            string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;

            while (length > 0)
            {
                result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];

                Thread.Sleep(12);

                length--;
            }

            return result;
        }

        // Сохраняет текущее состояние внутри снимка.
        public IMemento Save()
        {
            return new ConcreteMemento(_state);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            _state = memento.GetState();
            Console.Write($"Originator: My state has changed to: {_state}");
        }
    }

    // Интерфейс Снимка предоставляет способ извлечения метаданных снимка, таких
    // как дата создания или название. Однако он не раскрывает состояние
    // Создателя.
    public interface IMemento
    {
        string GetName();

        string GetState();

        DateTime GetDate();
    }

    // Конкретный снимок содержит инфраструктуру для хранения состояния
    // Создателя.
    class ConcreteMemento : IMemento
    {
        private string _state;

        private DateTime _date;

        public ConcreteMemento(string state)
        {
            _state = state;
            _date = DateTime.Now;
        }

        // Создатель использует этот метод, когда восстанавливает своё
        // состояние.
        public string GetState()
        {
            return _state;
        }

        // Остальные методы используются Опекуном для отображения метаданных.
        public string GetName()
        {
            return $"{_date} / ({_state.Substring(0, 9)})...";
        }

        public DateTime GetDate()
        {
            return _date;
        }
    }

    // Опекун не зависит от класса Конкретного Снимка. Таким образом, он не
    // имеет доступа к состоянию создателя, хранящемуся внутри снимка. Он
    // работает со всеми снимками через базовый интерфейс Снимка.
    class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private Originator _originator = null;

        public Caretaker(Originator originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("\nCaretaker: Saving Originator's state...");
            _mementos.Add(_originator.Save());
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                return;
            }

            var memento = _mementos.Last();
            _mementos.Remove(memento);

            Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

            try
            {
                _originator.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("Caretaker: Here's the list of mementos:");

            foreach (var memento in _mementos)
            {
                Console.WriteLine(memento.GetName());
            }
        }
    }

    class Memento
    {
        public static void Start(string[] args)
        {
            // Клиентский код.
            Originator originator = new Originator("Super-duper-super-puper-super.");
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            Console.WriteLine();
            caretaker.ShowHistory();

            Console.WriteLine("\nClient: Now, let's rollback!\n");
            caretaker.Undo();

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine();
        }
    }
}