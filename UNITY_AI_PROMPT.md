# Unity Project — AI Development Prompt (Architecture & Code Rules)

Використовуй цей документ як головний контекст для розробки нових задач, виправлення багів та рефакторингу в Unity-проєкті. Правила та довідковий код нижче можна перевикористовувати в інших проєктах без посилань на конкретні репозиторії.

---

## 1. Архітектура: MVC + Service

- **Model** — модель даних. Тільки дані та їх структура. Без логіки відображення та без прямого доступу до Unity API, де це можна уникнути.
- **Service** — бізнес-логіка. Вся логіка обробки даних, правил гри, інтеграцій. Сервіси реєструються через Zenject і отримують залежності через `[Inject] Construct(...)`.
- **Controller** — міст між Service та View. Обробляє події UI (кліки, введення), викликає методи сервісів, не містить бізнес-логіки. Роль контролера часто виконують екрани (наслідок BaseScreen) або окремі компоненти, що підписані на події View.
- **View** — відображення. Має доступ до даних для відображення (наприклад, через `Init(model)` або підписку на події моделі). **View не приймає рішень і не викликає бізнес-логіку** — лише оновлює UI. Події UI передаються в Controller, далі — у Service.

**Потік:** UI event → Controller → Service. View отримує дані від Model/Service лише для відображення.

---

## 2. Dependency Injection (Zenject)

- Інтерфейси для сервісів: `IXxxService`, реалізація — клас (часто не MonoBehaviour).
- Інжекція через метод: `[Inject] private void Construct(IDependency dependency)` — приватне поле `_dependency` заповнюється інжектором.
- Реєстрація в Installer: `Container.Bind<IXxxService>().To<XxxService>().AsSingle().WithArguments(...)`.
- MonoBehaviour-класи отримують залежності через `[Inject] private void Construct(...)`; не використовуй публічні поля для інжектованих сервісів.

---

## 3. Екрани та екранні сервіси

- Екрани наслідують базовий клас екрана (див. довідковий код нижче). Відкриття/закриття — через менеджер екранів або екранний сервіс, а не пряме вмикання/вимикання GameObject з інших місць.
- Екран: підписка на кнопки в `Start`/`Awake`, відписка в `OnDestroy`. Обробник події викликає метод сервісу або закриває екран, без бізнес-логіки в екрані.
- Екранний сервіс наслідує базовий менеджер екранів і надає методи типу `ShowSomeScreen()`; реєструється в Zenject.

---

## 4. Іменування

- **Приватні поля (не SerializeField):** `_camelCase`.
- **Приватні SerializeField:** `camelCase`.
- **Публічні властивості:** `PascalCase`.
- **Методи та події:** `PascalCase` для публічних; приватні обробники подій — наприклад `CloseButton_OnClick`, `ItemView_OnClicked`.
- **Інтерфейси:** префікс `I` — `IWeatherService`, `IScreen`.

---

## 5. Async: UniTask

- Для асинхронного коду в геймплеї використовуй **UniTask** (Cysharp.UniTask), де потрібна інтеграція з Unity lifecycle (`UniTask.Delay`, `UniTask.DelaySeconds`).
- Публічні API сервісів можуть повертати `UniTask` або `UniTask<T>`.
- Де потрібна підтримка `CancellationToken` або зовнішні API — можна використовувати `Task`.

---

## 6. Odin (Sirenix)

- Де потрібна серіалізація словників або складних структур у редакторі — використовуй `SerializedMonoBehaviour` замість `MonoBehaviour`.
- Атрибути: `[ShowIf(nameof(booleanField))]` для умовного відображення полей, `[Header("...")]`, `[Button]` для кнопок в інспекторі — лише де потрібно.

---

## 7. Логіка та повторення коду (DRY)

- Повторювану логіку виноси в спільний сервіс, базовий клас або допоміжний клас. Зміни — в одному місці.
- Логіка має рухатися вгору по ієрархії до спільного коду.

---

## 8. Простота (Keep It Simple)

- Не ускладнюй код без потреби. Уникай надмірного LINQ; перевага читабельним циклам там, де це очевидніше.

---

## 9. Коментарі

- Коментарі не потрібні, якщо код зрозумілий. Додавай їх лише там, де логіка справді неочевидна.

---

## 10. Редагування префабів і сцен

- **Не змінюй префаби (.prefab) та сцени (.unity).** Вноси зміни лише в скрипти (.cs) та, за потреби, в .asset, якщо це не змінює ієрархію або посилання без явного запиту.

---

## 11. Логування

- Замість `Debug.Log` / `Debug.LogError` використовуй проєктний API логування. Якщо в проєкті немає такого API — можна використати патерн нижче (довідковий код).

---

## 12. Структура та неймспейси

- Неймспейси мають відповідати структурі папок і наявному коду в поточному проєкті.

---

## 13. Обмеження інструментів

- Орієнтуйся на плагіни та пакети, які вже є в проєкті. Не пропонуй рішення, які потребують інших плагінів, без явного запиту.
- Безкоштовні доповнення (наприклад ZenExtended для Zenject+UniTask) можна запропонувати окремо, без зміни коду без підтвердження.

---

## 14. Стиль відповіді та невизначеність

- Відповідай формально. Посилайся на реальні джерела в поточному проєкті (файли, класи, методи), де це можливо — не вигадуй файли чи API.
- Якщо щось незрозуміло — **перепитай**, а не вгадуй.

---

## 15. Довідковий код (для перевикористання в проєктах)

Нижче наведено мінімальний код, який можна скопіювати або адаптувати в будь-якому Unity-проєкті. При необхідності додай власні типи (наприклад Guid для екранів) або спрости (наприклад, прибрати Odin-атрибути, якщо Odin відсутній).

### 15.1. Інтерфейс екрана та базовий екран

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface IScreen
{
    event EventHandler<IScreen> OnOpened;
    event EventHandler<IScreen> OnClosed;
    string Key { get; }
    Task Open(CancellationToken cancellationToken = default);
    Task Close(CancellationToken cancellationToken = default);
    bool IsOpen();
}

public class BaseScreen : MonoBehaviour, IScreen
{
    [SerializeField] private string _key;
    [SerializeField] private bool hasCloseButton;
    [SerializeField] private Button closeButton;

    private TaskCompletionSource<bool> _taskCompletionSource;

    public event EventHandler<IScreen> OnOpened;
    public event EventHandler<IScreen> OnClosed;
    public string Key => _key;

    protected virtual void Awake()
    {
        if (hasCloseButton && closeButton != null)
            closeButton.onClick.AddListener(CloseButton_OnClick);
    }

    protected virtual void OnDestroy()
    {
        if (hasCloseButton && closeButton != null)
            closeButton.onClick.RemoveListener(CloseButton_OnClick);
    }

    private void CloseButton_OnClick() => Close();

    public virtual async Task Open(CancellationToken cancellationToken = default)
    {
        _taskCompletionSource = new TaskCompletionSource<bool>();
        gameObject.SetActive(true);
        OnOpened?.Invoke(this, this);
        _taskCompletionSource.TrySetResult(true);
        await _taskCompletionSource.Task;
    }

    public virtual async Task Close(CancellationToken cancellationToken = default)
    {
        _taskCompletionSource = new TaskCompletionSource<bool>();
        gameObject.SetActive(false);
        OnClosed?.Invoke(this, this);
        _taskCompletionSource.TrySetResult(true);
        await _taskCompletionSource.Task;
    }

    public virtual bool IsOpen() => gameObject.activeSelf;
}
```

### 15.2. Базовий менеджер екранів

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreenManager<TScreen> : MonoBehaviour where TScreen : BaseScreen
{
    [SerializeField] private List<TScreen> _allScreens;
    [SerializeField] private List<TScreen> _allOpenedScreens;

    public event EventHandler<IScreen> OnOpenedScreen;
    public event EventHandler<IScreen> OnClosedScreen;

    protected List<TScreen> AllScreens => _allScreens;
    protected List<TScreen> AllOpenedScreens => _allOpenedScreens;

    protected virtual void Awake()
    {
        if (_allOpenedScreens == null)
            _allOpenedScreens = new List<TScreen>();
        foreach (var screen in _allScreens)
        {
            screen.OnOpened += Screen_OnOpened;
            screen.OnClosed += Screen_OnClosed;
        }
        foreach (var s in _allScreens)
            if (s.IsOpen())
                _allOpenedScreens.Add(s);
    }

    protected virtual void OnDestroy()
    {
        foreach (var screen in _allScreens)
        {
            screen.OnOpened -= Screen_OnOpened;
            screen.OnClosed -= Screen_OnClosed;
        }
    }

    protected virtual void ShowScreen(IScreen screen)
    {
        if (screen != null)
            screen.Open();
    }

    public virtual void ShowScreen<R>() where R : BaseScreen
    {
        var screen = GetScreen<R>();
        ShowScreen(screen);
    }

    protected virtual void CloseScreen(IScreen screen)
    {
        if (screen != null)
            screen.Close();
    }

    public virtual void CloseAllScreens()
    {
        var list = new List<TScreen>(_allOpenedScreens);
        foreach (var screen in list)
            CloseScreen(screen);
    }

    protected R GetScreen<R>() where R : BaseScreen
    {
        foreach (var s in _allScreens)
            if (s is R r)
                return r;
        return null;
    }

    private void Screen_OnOpened(object sender, IScreen e)
    {
        if (e is TScreen ts && !_allOpenedScreens.Contains(ts))
            _allOpenedScreens.Add(ts);
        OnOpenedScreen?.Invoke(this, e);
    }

    private void Screen_OnClosed(object sender, IScreen e)
    {
        if (e is TScreen ts)
            _allOpenedScreens.Remove(ts);
        OnClosedScreen?.Invoke(this, e);
    }
}

public abstract class BaseScreenManager : BaseScreenManager<BaseScreen> { }
```

### 15.3. Екран як Controller (приклад)

Екран лише підписується на кнопку та викликає сервіс; бізнес-логіки в екрані немає.

```csharp
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ExampleMenuScreen : BaseScreen
{
    [SerializeField] private Button openSettingsButton;

    private IExampleScreensService _screensService;

    [Inject]
    private void Construct(IExampleScreensService screensService)
    {
        _screensService = screensService;
    }

    private void Start()
    {
        openSettingsButton.onClick.AddListener(OpenSettingsButton_OnClick);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        openSettingsButton.onClick.RemoveListener(OpenSettingsButton_OnClick);
    }

    private void OpenSettingsButton_OnClick()
    {
        _screensService.ShowSettingsScreen();
        Close();
    }
}
```

### 15.4. View — лише відображення (приклад)

View отримує модель через `Init`, підписується на події моделі та оновлює UI. Події кліку пробросуються назовні (обробляє Controller).

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExampleCellView : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private ExampleCell _cell;

    public event System.Action<int> OnClicked;

    public void Init(ExampleCell cell)
    {
        if (_cell != null)
            _cell.OnUpdated -= UpdateView;
        _cell = cell;
        _cell.OnUpdated += UpdateView;
        UpdateView();
    }

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(Button_OnClick);
    }

    private void OnDestroy()
    {
        if (_cell != null)
            _cell.OnUpdated -= UpdateView;
        if (button != null)
            button.onClick.RemoveListener(Button_OnClick);
    }

    private void Button_OnClick()
    {
        if (_cell != null)
            OnClicked?.Invoke(_cell.Id);
    }

    private void UpdateView()
    {
        if (_cell == null) return;
        countText.text = _cell.Count.ToString();
        if (iconImage != null && _cell.Icon != null)
            iconImage.sprite = _cell.Icon;
    }
}

public class ExampleCell
{
    public int Id { get; set; }
    public int Count { get; set; }
    public UnityEngine.Sprite Icon { get; set; }
    public event System.Action OnUpdated;
    public void NotifyUpdated() => OnUpdated?.Invoke();
}
```

### 15.5. Сервіс та Installer (Zenject)

```csharp
public interface IExampleService
{
    void DoWork(string id);
}

public class ExampleService : IExampleService
{
    private readonly IOtherDependency _other;

    [Zenject.Inject]
    public ExampleService(IOtherDependency other)
    {
        _other = other;
    }

    public void DoWork(string id)
    {
        _other.Process(id);
    }
}

public class ExampleInstaller : Zenject.MonoInstaller
{
    [SerializeField] private SomeProvider someProvider;

    public override void InstallBindings()
    {
        Container.Bind<IExampleService>().To<ExampleService>().AsSingle();
        Container.Bind<IOtherDependency>().To<OtherDependency>().AsSingle().WithArguments(someProvider);
    }
}
```

### 15.6. Odin — умовне поле та SerializedMonoBehaviour

```csharp
using Sirenix.OdinInspector;
using UnityEngine;

public class ExampleOdinScreen : SerializedMonoBehaviour
{
    [SerializeField] private bool useCustomButton;
    [ShowIf(nameof(useCustomButton))]
    [SerializeField] private UnityEngine.UI.Button customButton;

    [SerializeField] private System.Collections.Generic.Dictionary<string, GameObject> keyToPrefab;
}
```

### 15.7. Логування (мінімальний API для проєкту)

Якщо в проєкті немає централізованого логування — можна додати статичний клас і викликати його замість `Debug.Log`:

```csharp
using UnityEngine;

public static class Debugging
{
    private static bool _enabled = true;

    public static void SetEnabled(bool enabled) => _enabled = enabled;

    public static void Log<T>(T context, string message)
    {
        if (_enabled)
            Debug.Log($"[{typeof(T).Name}] {message}");
    }

    public static void Error<T>(T context, string message)
    {
        if (_enabled)
            Debug.LogError($"[{typeof(T).Name}] {message}");
    }
}
```

Використання: `Debugging.Log(this, "State changed");`, `Debugging.Error(this, "Invalid value");`.

---

## Короткий чеклист для AI

1. **Архітектура:** Model = дані; Service = бізнес-логіка; Controller = події UI → сервіс; View = лише відображення, події UI → Controller.
2. **DI:** інтерфейси для сервісів, `[Inject] private void Construct(...)`.
3. **Екрани:** базовий екран + менеджер екранів (код вище), відкриття через сервіс, обробники кнопок викликають сервіс або `Close()`.
4. **Іменування:** `_camelCase` приватні поля, `camelCase` SerializeField, `PascalCase` публічні члени та інтерфейси `I*`.
5. **Async:** UniTask для геймплею та Unity lifecycle.
6. **Odin:** SerializedMonoBehaviour та атрибути лише де потрібно.
7. **DRY:** спільна логіка вгору по ієрархії, один місце зміни.
8. **Простота:** не ускладнювати без причини, обмежений LINQ.
9. **Без зайвих коментарів.**
10. **Не редагувати .prefab та .unity.**
11. **Лог:** проєктний API (наприклад `Debugging.Log` / `Debugging.Error`) або код з п. 15.7.
12. **Неймспейси** узгоджені зі структурою папок і наявним кодом у проєкті.
13. **Не вгадувати** — при неяснощах запитати.
