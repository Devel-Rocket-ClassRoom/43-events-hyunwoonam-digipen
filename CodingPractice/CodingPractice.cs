using System;
using System.Collections.Generic;
using System.Text;

class Button
{
    public event ButtonClickHandler Click;

    public void OnClick()
    {
        Click?.Invoke();
    }
}

class Player
{
    public int _health = 100;

    public event Action<int> DamageTaken;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Console.WriteLine($"플레이어 체력: {_health}");
        DamageTaken?.Invoke(_health);
    }
}

class HealthBar
{
    public void OnPlayerDamaged(int currHealth)
    {
        Console.WriteLine($"[UI] 체력바 업데이트: {currHealth}%");
    }
}

class SoundManager
{
    public void OnPlayerDamaged(int currentHealth)
    {
        Console.WriteLine("[Sound] 피격 효과음 재생");
    }
}

class Timer
{
    public event Action Tick;
    private int count;
    public void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            count++;
            Console.WriteLine($"타이머: {count}초");
            Tick?.Invoke();
        }
    }
}

class Logger
{
    public void OnTick()
    {
        Console.WriteLine("[Logger] 틱 기록됨");
    }
}

class Sensor
{
    public event Action<string> Alert;

    public void Detect(string message)
    {
        Console.WriteLine($"감지: {message}");
        Alert?.Invoke(message);
    }
}

class GameCharacter
{
    public event Action OnDeath;
    public event Action<int> OnDamaged;
    public event Action<int, string> OnAttack;

    private int _health = 100;
    private string _name;

    public GameCharacter(string name)
    {
        _name = name;
    }
    public void Attack(int damage, string targetName)
    {
        OnAttack?.Invoke(damage, targetName);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        OnDamaged?.Invoke(_health);

        if (_health <= 0)
        {
            OnDeath?.Invoke();
        }

    }
}

class PriceChangedEventArgs : EventArgs
{
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal ChangePercent { get; set; }

    public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
    {
        OldPrice = oldPrice;
        NewPrice = newPrice;

        if (oldPrice != 0)
        {
            ChangePercent = (newPrice - oldPrice) / oldPrice * 100;
        }
    }
}

class Stock
{
    public string _symbol;
    public decimal _price;

    public event EventHandler<PriceChangedEventArgs> PriceChanged;

    public Stock(string symbol, decimal initialPrice)
    {
        _symbol = symbol;
        _price = initialPrice;
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value)
            {
                return;
            }

            decimal oldPrice = _price;
            _price = value;

            OnPriceChanged(new PriceChangedEventArgs(oldPrice, _price));
        }
    }

    protected virtual void OnPriceChanged(PriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }

    public override string ToString()
    {
        return $"{_symbol}: {_price:C}";
    }
}

class FuelEventArgs : EventArgs
{
    public int Fuel { get; }
    public string Warning { get; }
    public FuelEventArgs(int fuel, string warning)
    {
        Fuel = fuel;
        Warning = warning;
    }
}

class Car
{
    private int fuelLevel;

    public event EventHandler<FuelEventArgs> FuelLow;
    public event Action<int> FuelChanged;

    public Car(int init)
    {
        fuelLevel = init;
    }
    public int FuelLevel => fuelLevel;

    public void Drive()
    {
        if (fuelLevel <= 0)
        {
            Console.WriteLine("연료 없음! 운전 불가");
            return;
        }

        fuelLevel -= 10;
        Console.WriteLine($"운전 중... 연료: {fuelLevel}%");

        FuelChanged?.Invoke(fuelLevel);

        if (fuelLevel <= 0)
        {
            OnFuelLow(new FuelEventArgs(fuelLevel, "연료가 바닥났습니다!"));
        }
        else if (fuelLevel <= 20)
        {
            OnFuelLow(new FuelEventArgs(fuelLevel, "연료가 부족합니다!"));
        }
    }

    protected virtual void OnFuelLow(FuelEventArgs e)
    {
        FuelLow?.Invoke(this, e);
    }
}

class Dashboard
{
    public void Subscribe(Car car)
    {
        car.FuelChanged += OnFuelChanged;
        car.FuelLow += OnFuelLow;
    }

    public void Unsubscribe(Car car)
    {
        car.FuelChanged -= OnFuelChanged;
        car.FuelLow -= OnFuelLow;
    }

    private void OnFuelChanged(int fuelLevel)
    {
        string gauge = new string('█', fuelLevel / 10);
        Console.WriteLine($"[대시보드] 연료 게이지: {gauge}");
    }

    private void OnFuelLow(object sender, FuelEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[경고!] {e.Warning} (잔량: {e.Fuel}%)");
        Console.ResetColor();
    }
}

class SecurePublisher
{
    private EventHandler _event;
    private readonly object _lock = new object();

    public event EventHandler _Event
    {
        add
        {
            lock(_lock)
            {
                Console.WriteLine($"구독자 추가: {value.Method.Name}");
                _event += value;
            }
        }
        remove
        {
            lock (_lock)
            {
                Console.WriteLine($"구독자 제거: {value.Method.Name}");
                _event -= value;
            }
        }
    }

    public void Raise()
    {
        _event?.Invoke(this, EventArgs.Empty);
    }
}

class GlobalNotifier
{
    public static event Action<string> OnGlobalMessage;

    public static void Send(string message)
    {
        Console.WriteLine($"[Global] 메시지 전송: {message}");
        OnGlobalMessage?.Invoke(message);
    }
}

class Module1
{
    public Module1()
    {
        GlobalNotifier.OnGlobalMessage += HandleMessage;
    }

    private void HandleMessage(string message)
    {
        Console.WriteLine($"[Module1] 수신: {message}");
    }
}

class Module2
{
    public Module2()
    {
        GlobalNotifier.OnGlobalMessage += HandleMessage;
    }

    private void HandleMessage(string message)
    {
        Console.WriteLine($"[Module2] 수신: {message}");
    }
}