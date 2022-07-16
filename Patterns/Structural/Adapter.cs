namespace Adapter
{
    // Целевой класс объявляет интерфейс, с которым может работать клиентский
    // код.
    public interface ITarget
    {
        string GetRequest();
    }

    // Адаптируемый класс содержит некоторое полезное поведение, но его
    // интерфейс несовместим  с существующим клиентским кодом. Адаптируемый
    // класс нуждается в некоторой доработке,  прежде чем клиентский код сможет
    // его использовать.
    class Adaptee
    {
        public string GetSpecificRequest()
        {
            return "Specific request.";
        }
    }

    // Адаптер делает интерфейс Адаптируемого класса совместимым с целевым
    // интерфейсом.
    class Adapter_ : ITarget
    {
        private readonly Adaptee _adaptee;

        public Adapter_(Adaptee adaptee)
        {
            _adaptee = adaptee;
        }

        public string GetRequest()
        {
            return $"This is '{_adaptee.GetSpecificRequest()}'";
        }
    }

    class Adapter
    {
        public static void Start()
        {
            Adaptee adaptee = new Adaptee();
            ITarget target = new Adapter_(adaptee);

            Console.WriteLine("Adaptee interface is incompatible with the client.");
            Console.WriteLine("But with adapter client can call it's method.");

            Console.WriteLine(target.GetRequest());
        }
    }
}